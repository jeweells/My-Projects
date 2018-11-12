using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ServerLogger;

namespace FastNotesUpdater
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Private Members

        /// <summary>
        /// Helps calculate the speed, time, etc related to the transfer
        /// </summary>
        TransferHelper.TransferHelper transferHelper;

        /// <summary>
        /// Used for anything when trying to access it from another thread
        /// </summary>
        /// <param name="o"></param>
        delegate void AnythingDelegate(object o);


        /// <summary>
        /// Used for anything when trying to acces it from another thread
        /// </summary>
        /// <param name="o"></param>
        delegate void WindowDelegate(Window o);

        /// <summary>
        /// Used to get the text of a TextBlock from another thread
        /// </summary>
        delegate string GetTextDelegate(TextBlock targetTextBlock);

        /// <summary>
        /// Used to change the text of a TextBlock from another thread
        /// </summary>
        delegate void ChangeTextDelegate(TextBlock targetTextBlock, string newText);

        /// <summary>
        /// Every this ms a function will be called to show the progress made while downloading
        /// </summary>
        int msPerDownloadInfo = 500;

        /// <summary>
        /// This handles the event that is called when downloading
        /// </summary>
        Checker.DownloaderEventHandler deh;

        /// <summary>
        /// This is the base model of this window (Gives a beautiful interface)
        /// </summary>
        WindowViewModel wvm;

        #endregion

        #region Constructor
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = wvm = new WindowViewModel(this);
            // Better if not radius
            wvm.WindowRadius = 0;
            loadingBar.Progress = 0;
            loadingBar.Visibility = Visibility.Collapsed;
            double InitialHeight = Height;
            Height -= 100;
            long totalSize = 0;
            long acumulatedSize = 0;

            // When this form is loaded do this
            Loaded += (x, y) =>
            {
                Task.Run(() => {
                    // Run it asynchronously
                    try
                    {
                        // Get the names of the files that is going to download
                        Checker.ProgramInfo pi = GetNeededFileNames();
                        if (pi == null || pi.Files == null) throw new Exception("Unknown error after trying to get the needed file names");
                        List<string> names = new List<string>(pi.Files.Count); // Initializes with the known size to avoid increasing the size of the list

                        pi.Files.ForEach((file) =>
                        {
                            names.Add(file.Name); // Get the name of the file to download
                            totalSize += file.Size; // Get the total size to download (the sum of all the files size)
                        });

                        transferHelper = new TransferHelper.TransferHelper(0, 0, totalSize, 0, msPerDownloadInfo);

                        this.Dispatcher.BeginInvoke(new AnythingDelegate((w) => ((Window)w).Height = InitialHeight), this);
                        loadingBar.Dispatcher.BeginInvoke(new AnythingDelegate((lb) => ((LoadingBar)lb).Visibility = Visibility.Visible), loadingBar);
                        if (DownloadAllFiles(names))
                        {
                            // It returned true so everything went well
                            MessageBox.Show("The application was updated successfully!");
                        }
                        // Open fastnotes again
                        Process.Start(System.IO.Path.Combine(Environment.CurrentDirectory, "FastNotes.exe"));
                        this.Dispatcher.BeginInvoke(new WindowDelegate(CloseThis), this);
                    }
                    catch (Exception e)
                    {
                        Dispatcher.BeginInvoke(new AnythingDelegate((o) => MessageBox.Show(e.ToString())), null);
                    }
                });

            };
            // Create the function that is going to inform on the interface the progress
            deh = new Checker.DownloaderEventHandler();

            deh.OnDownloading += (prevbytes, bytesnow, totalbytes, msPassed) =>
            {
                // By converting this object to string it shows bytesDownloaded/totalBytes speed and the time left given these arguments
                transferHelper.Update(acumulatedSize, acumulatedSize += bytesnow - prevbytes);
                NewText(downloadSpeed, transferHelper.ToString("Downloading {BN}/{TB} {WU} at {S} {SU} ({TL} {TU} left)"));
                loadingBar.Dispatcher.BeginInvoke(new AnythingDelegate((e) => ((LoadingBar)e).Progress = (double)acumulatedSize / totalSize), loadingBar);
            };

        }
        #endregion

      
        #region Private Helpers
        /// <summary>
        /// Closes the window (Used to close the window from another thread)
        /// </summary>
        /// <param name="e"></param>
        void CloseThis(Window e) 
        {
            e.Close();
        }

        string GetTextDelegateImplemented(TextBlock targetTextBlock)
        {
            return targetTextBlock.Text;
        }

        string GetText(TextBlock textBlock)
        {
            return (string)textBlock.Dispatcher.BeginInvoke(new GetTextDelegate(GetTextDelegateImplemented), textBlock).Result;
        }

        /// <summary>
        /// Changes the text of the targetTextBlock
        /// </summary>
        void ChangeText(TextBlock targetTextBlock, string newText)
        {
            targetTextBlock.Text = newText;
        }

        /// <summary>
        /// Changes the text shorter..,
        /// </summary>
        /// <param name="t"></param>
        /// <param name="text"></param>
        void NewText(TextBlock t, string text)
        {
            t.Dispatcher.BeginInvoke(new ChangeTextDelegate(ChangeText), t, text);
        }


        #endregion

        /// <summary>
        /// Finds the files that must download to be up to date
        /// </summary>
        /// <returns></returns>
        public Checker.ProgramInfo GetNeededFileNames()
        {
            NewText(logLabel, "Downloading new version info...");
            Checker.ProgramInfo serverInfo = Checker.GetProgramInfo();
            NewText(logLabel, "Analyzing current version info...");
            Checker.ProgramInfo currentPI = Checker.ProgramInfo.Get(Environment.CurrentDirectory);
            Checker.ProgramInfo names = new Checker.ProgramInfo();
            names.Files = new List<Checker.FileInfo>();
            // Files that are in newest must exist in current
            // Files that exist in current will be replaced if they're different of the newest

            NewText(logLabel, "Checking differences...");

            foreach (var file in serverInfo.Files)
            {

                NewText(logLabel, $"Checking {file.Name}...");


                Checker.FileInfo fi = currentPI.Files.Find((x) => file.Name == x.Name);
                if (fi == null ||
                    fi.Size != file.Size ||
                    fi.CheckSum != file.CheckSum ||
                    fi.ShiftedCheckSum != file.ShiftedCheckSum)
                {
                    NewText(logLabel, GetText(logLabel) + "Old!");
                    names.Files.Add(file);
                    if (file.Name == "FastNotesUpdater.exe") // A new updater was found
                    {
                        List<string> n = new List<string>();
                        n.Add(file.Name);
                        n.AddRange(Checker.GetUpdaterDependencies());
                        // Download only the updater and its dependencies.. *** Careful ***
                        if (DownloadAllFiles(n))
                        {
                            // Updater downloaded -> open it and close this one
                            Process.Start(System.IO.Path.Combine(Environment.CurrentDirectory, "FastNotesUpdater.exe"));
                            this.Dispatcher.BeginInvoke(new WindowDelegate(CloseThis), this);
                        }
                        else
                        {
                            // Operation canceled -> close it and open fastnotes
                            Process.Start(System.IO.Path.Combine(Environment.CurrentDirectory, "FastNotes.exe"));
                            this.Dispatcher.BeginInvoke(new WindowDelegate(CloseThis), this);
                        }
                    }
                }
                else
                {
                    NewText(logLabel, GetText(logLabel) + "Ok!");
                }
            }
            return names;
        }

        /// <summary>
        /// Downloads all the files in the list managing dialogs, progress, errors and so on
        /// </summary>
        /// <param name="fileNames"></param>
        /// <returns></returns>
        public bool DownloadAllFiles(List<string> fileNames)
        {

            List<string> backupFiles = new List<string>();
            foreach (var name in fileNames)
            {
                try
                {
                    NewText(logLabel, $"Creating backup of '{name}'...");
                    MakeBackupFile(name);
                    backupFiles.Add(name);
                    NewText(logLabel, $"Downloading '{name}'...");
                    DownloadFile(name);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                    NewText(logLabel, "Restoring...");
                    foreach (var fn in backupFiles)
                    {
                        MakeFileFromBackup(fn);
                    }
                    NewText(logLabel, "Done.");
                    return false;
                }
            }
            foreach (var fn in backupFiles)
            {
                try
                {
                    System.IO.File.Delete(System.IO.Path.Combine(Environment.CurrentDirectory, fn + ".bak"));
                }
                catch { }
            }
            return true;

        }

        /// <summary>
        /// Renames a file with .bak at the end
        /// </summary>
        /// <param name="name"></param>
        public void MakeBackupFile(string name)
        {
            string path = System.IO.Path.Combine(Environment.CurrentDirectory, name);
            if (System.IO.File.Exists(path))
            {
                if (System.IO.File.Exists(path + ".bak")) System.IO.File.Delete(path + ".bak");
                System.IO.File.Move(path, path + ".bak");
            }
        }

        /// <summary>
        /// Renames a file taking the .bak out
        /// </summary>
        /// <param name="name"></param>
        public void MakeFileFromBackup(string name)
        {
            string path = System.IO.Path.Combine(Environment.CurrentDirectory, name);
            if (System.IO.File.Exists(path + ".bak"))
            {
                if (System.IO.File.Exists(path)) System.IO.File.Delete(path);
                System.IO.File.Move(path + ".bak", path);
            }
        }

        /// <summary>
        /// Downloads an unique file, handling retrying if failed
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string DownloadFile(string name)
        {
            string msg = "";
            try
            {

                msg = Checker.DownloadAppFile(name, System.IO.Path.Combine(Environment.CurrentDirectory, name), deh, msPerDownloadInfo);
            }
            catch (Exception e)
            {
                if (MessageBoxResult.Yes == MessageBox.Show($"There was an error while trying to download '{name}'.\n[{e}]\nDo you want to try again?", "Error", MessageBoxButton.YesNo, MessageBoxImage.Error))
                {
                    DownloadFile(name);
                }
                else
                {
                    throw new OperationCanceledException("The update was canceled, the previous state will be restored");
                }
            }
            return msg;
        }
    }
}
