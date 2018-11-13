using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
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
using IWshRuntimeLibrary;
using Microsoft.Win32;
using ServerLogger;


namespace FastNotesUpdater
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        #region Private Members
        /// <summary>
        /// Helps to select all the text and deselected when a click is preformed and it's already selected
        /// </summary>
        string organization = "Jeweells";
        string applicationName = "FastNotes";

        string currentDirectory = Environment.CurrentDirectory;

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

        Task downloadTask;
        #endregion

        #region Public Members
        public enum Commands
        {
            Install = 0,
            Update = 1
        }
        public Commands currentCommands;

        /// <summary>
        /// The event that is fired when any child property changes its value
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

        /// <summary>
        /// Directory where all the files will be downloaded
        /// </summary>
        public string CurrentDirectory { set {
                currentDirectory = value;
                OnPropertyChanged(nameof(CurrentDirectory));
            }
            get { return currentDirectory; } }

        #endregion

        #region Constructor
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = wvm = new WindowViewModel(this);
            // Better if not radius
            wvm.WindowRadius = 0;

            long totalSize = 0;
            long acumulatedSize = 0;
            string[] commands = Environment.CommandLine.Split(' ');
            currentCommands = Commands.Install;
            loadingBar.Visibility = Visibility.Collapsed;
            downloadInfoRow.Visibility = Visibility.Collapsed;
            openFastNotesButtonRow.Visibility = Visibility.Collapsed;
            installButtonRow.Visibility = Visibility.Collapsed;
            folderPickerRow.Visibility = Visibility.Collapsed;

            foreach (var item in commands)
            {
                if (item == "update") currentCommands |= Commands.Update;
            }
            
            if (((int)currentCommands & (int) Commands.Update) == (int) Commands.Update)
            {
                // Update method
                CurrentDirectory = Environment.CurrentDirectory;
                // When this form is loaded do this
                Loaded += (x, y) =>
                {
                    PrepareToUpdate();
                    downloadTask.Start();
                };
            }
            else
            {
                // Install method
                PrepareToInstall();
            }
            // Task that downloads all the needed files
            downloadTask = new Task(() => {
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
                    // Create the function that is going to inform on the interface the progress
                    deh = new Checker.DownloaderEventHandler();

                    deh.OnDownloading += (prevbytes, bytesnow, totalbytes, msPassed) =>
                    {
                        // By converting this object to string it shows bytesDownloaded/totalBytes speed and the time left given these arguments
                        transferHelper.Update(acumulatedSize, acumulatedSize += bytesnow - prevbytes);
                        NewText(downloadSpeed, transferHelper.ToString("Downloading {BN}/{TB} {WU} at {S} {SU} ({TL} {TU} left)"));
                        loadingBar.Dispatcher.BeginInvoke(new AnythingDelegate((e) => ((LoadingBar)e).Progress = (double)acumulatedSize / totalSize), loadingBar);
                    };

                    downloadInfoRow.Dispatcher.BeginInvoke(new AnythingDelegate((lb) => ((Grid)lb).Visibility = Visibility.Visible), downloadInfoRow);
                    loadingBar.Dispatcher.BeginInvoke(new AnythingDelegate((lb) => ((LoadingBar)lb).Visibility = Visibility.Visible), loadingBar);
                    if (DownloadAllFiles(names))
                    {
                        // Configures the installation
                        if((int)currentCommands == (int)Commands.Install)
                        {
                            CreateShortcuts();
                        }
                        //CreateUninstaller();


                        // It returned true so everything went well
                        downloadInfoRow.Dispatcher.BeginInvoke(new AnythingDelegate((lb) => ((Grid)lb).Visibility = Visibility.Collapsed), downloadInfoRow);
                        loadingBar.Dispatcher.BeginInvoke(new AnythingDelegate((lb) => ((LoadingBar)lb).Visibility = Visibility.Collapsed), loadingBar);
                        openFastNotesButtonRow.Dispatcher.BeginInvoke(new AnythingDelegate((lb) => ((Grid)lb).Visibility = Visibility.Visible), openFastNotesButtonRow);
                        NewText(logLabel, "The application was updated successfully!");

                    }
                    
                    // Open fastnotes again
                }
                catch (Exception e)
                {
                    Dispatcher.BeginInvoke(new AnythingDelegate((o) => MessageBox.Show(e.ToString())), null);
                }
            });
      
           

        }


        #endregion

        #region Private Helpers

        /// <summary>
        /// Creates registers to let windows know this program is installed
        /// </summary>
        private void CreateUninstaller()
        {
            string UninstallRegKeyPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
            using (RegistryKey parent = Registry.LocalMachine.OpenSubKey(UninstallRegKeyPath, true))
            {
                if (parent == null)
                {
                    throw new Exception("Uninstall registry key not found.");
                }
                try
                {
                    RegistryKey key = null;

                    try
                    {
                        string guidText = FindFastNotesGuid();
                        if (guidText == null) guidText = Guid.NewGuid().ToString("B");

                        key = parent.OpenSubKey(guidText, true) ??
                              parent.CreateSubKey(guidText);

                        if (key == null)
                        {
                            throw new Exception(String.Format("Unable to create uninstaller '{0}\\{1}'", UninstallRegKeyPath, guidText));
                        }


                        Assembly asm = Assembly.LoadFile(System.IO.Path.Combine(CurrentDirectory, "FastNotes.exe"));
                        Version v = asm.GetName().Version;
                        string exe = "\"" + asm.CodeBase.Substring(8).Replace("/", "\\\\") + "\"";

                        key.SetValue("DisplayName", "FastNotes");
                        key.SetValue("ApplicationVersion", v.ToString());
                        key.SetValue("Publisher", "Jeweells");
                        key.SetValue("DisplayIcon", exe);
                        key.SetValue("DisplayVersion", v.ToString(2));
                        key.SetValue("URLInfoAbout", "http://www.fastnotes.tk");
                        key.SetValue("Contact", "abraham.pacheco6319@gmail.com");
                        key.SetValue("InstallDate", DateTime.Now.ToString("yyyyMMdd"));
                        key.SetValue("UninstallString", exe + " /uninstallprompt");
                    }
                    finally
                    {
                        if (key != null)
                        {
                            key.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(
                        "An error occurred writing uninstall information to the registry.  The service is fully installed but can only be uninstalled manually through the command line.",
                        ex);
                }
            }
        }

        private string FindFastNotesGuid()
        {
            string UninstallRegKeyPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
            using (RegistryKey parent = Registry.LocalMachine.OpenSubKey(UninstallRegKeyPath, true))
            {
                foreach (var item in parent.GetSubKeyNames())
                {
                    try
                    {

                        using (var child = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\" + item, false))
                        {

                            if (child.GetValue("DisplayName").ToString() == "FastNotes")
                            {
                               return item;
                            }

                        }
                    }
                    catch { }

                }
            }
            return null;
        }


        /// <summary>
        /// Prepares all the elements needed for the install interface, make sure you don't call this twice... never
        /// </summary>
        void PrepareToInstall()
        {
            string dialogSelectFolder = "Select the installation folder:";
            folderPickerRow.Visibility = Visibility.Visible;
            installButtonRow.Visibility = Visibility.Visible;
            logInfoRow.Visibility = Visibility.Visible;
            CurrentDirectory = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), organization, applicationName);
            chooseBtn.Click += (sender, e) =>
            {
                var fbd = new System.Windows.Forms.FolderBrowserDialog();
                fbd.Description = dialogSelectFolder;
                fbd.SelectedPath = CurrentDirectory;
                if (System.Windows.Forms.DialogResult.OK == fbd.ShowDialog(GetIWin32Window(this)))
                {
                    if (!fbd.SelectedPath.EndsWith("FastNotes"))
                    {
                        fbd.SelectedPath = System.IO.Path.Combine(fbd.SelectedPath, "FastNotes");
                    }
                    CurrentDirectory = fbd.SelectedPath;
                }
            };
            logLabel.Text = dialogSelectFolder;
            installBtn.Click += (sender, e) =>
            {
                // Prepares everything to download/update the app
                CloseInstallInterface();
                try
                {
                    System.IO.Directory.CreateDirectory(CurrentDirectory); // Makes sure everything works properly
                }
                catch
                {

                }
                PrepareToUpdate();
                // Copies this executable on the path, so that it doesn't need to download it again!
                Task.Run(() =>
                {
                    var ass = System.Reflection.Assembly.GetExecutingAssembly();
                    string targetLocation = System.IO.Path.Combine(CurrentDirectory, "FastNotesUpdater.exe");
                    if(targetLocation != ass.Location)
                    {
                        NewText(logLabel, "Copying the updater...");
                        System.IO.File.Copy(ass.Location, targetLocation, true);
                    }
                    downloadTask.Start();
                });
            };
        }

        /// <summary>
        /// Closes the install interface so that another interface can be opened without interceptions
        /// </summary>
        void CloseInstallInterface()
        {
            installButtonRow.Visibility = Visibility.Collapsed;
            logInfoRow.Visibility = Visibility.Collapsed;
            folderPickerRow.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Prepares all the elements needed for the update interface
        /// </summary>
        void PrepareToUpdate()
        {
            logInfoRow.Visibility = Visibility.Visible;
            downloadInfoRow.Visibility = Visibility.Collapsed;
            loadingBar.Visibility = Visibility.Collapsed;
            loadingBar.Progress = 0;
            openFastNotesButton.Click += (sender, e) =>
            {
                Process.Start(System.IO.Path.Combine(CurrentDirectory, "FastNotes.exe"));
                Close();
            };
        }

        /// <summary>
        /// Closes the update interface so that another interface can be opened without interceptions
        /// </summary>
        void CloseUpdateInterface()
        {
            logInfoRow.Visibility = Visibility.Collapsed;
            downloadInfoRow.Visibility = Visibility.Collapsed;
            loadingBar.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Use this to translate this wpf owner to windows forms owner
        /// </summary>
        /// <param name="visual"></param>
        /// <returns></returns>
        static System.Windows.Forms.IWin32Window GetIWin32Window(System.Windows.Media.Visual visual)
        {
            var source = System.Windows.PresentationSource.FromVisual(visual) as System.Windows.Interop.HwndSource;
            System.Windows.Forms.IWin32Window win = new OldWindow(source.Handle);
            return win;
        }

        /// <summary>
        /// This class helps to access dialogs from windows forms
        /// </summary>
        private class OldWindow : System.Windows.Forms.IWin32Window
        {
            private readonly System.IntPtr _handle;
            public OldWindow(System.IntPtr handle)
            {
                _handle = handle;
            }

            #region IWin32Window Members
            System.IntPtr System.Windows.Forms.IWin32Window.Handle
            {
                get { return _handle; }
            }
            #endregion
        }

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

        /// <summary>
        /// Gets the text of a textblock when it's needed to access it from another thread
        /// </summary>
        /// <param name="textBlock"></param>
        /// <returns></returns>
        string GetText(TextBlock textBlock)
        {
            return (string)textBlock.Dispatcher.Invoke(new GetTextDelegate(GetTextDelegateImplemented), textBlock);
        }

        /// <summary>
        /// Changes the text of the targetTextBlock
        /// </summary>
        void ChangeText(TextBlock targetTextBlock, string newText)
        {
            targetTextBlock.Text = newText;
        }

        /// <summary>
        /// Changes the text when trying to access it from another thread (shorter version)
        /// </summary>
        /// <param name="t"></param>
        /// <param name="text"></param>
        void NewText(TextBlock t, string text)
        {
            t.Dispatcher.BeginInvoke(new ChangeTextDelegate(ChangeText), t, text);
        }


        #endregion

        #region Public Methods

        /// <summary>
        /// Creates a shortcut for the file
        /// </summary>
        public void CreateShortcuts()
        {
            string pathToExe = System.IO.Path.Combine(CurrentDirectory, "FastNotes.exe");
            string commonStartMenuPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonStartMenu);
            string appStartMenuPath = System.IO.Path.Combine(commonStartMenuPath, "Programs", "FastNotes");

            if (!System.IO.Directory.Exists(appStartMenuPath))
                System.IO.Directory.CreateDirectory(appStartMenuPath);

            string shortcutLocation = System.IO.Path.Combine(appStartMenuPath, "FastNotes" + ".lnk");
            if (System.IO.File.Exists(shortcutLocation)) return;
            WshShell shell = new WshShell();
            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutLocation);

            shortcut.Description = "Simple and useful quick notes for anything";
            //shortcut.IconLocation = @"C:\Program Files (x86)\TestApp\TestApp.ico"; //uncomment to set the icon of the shortcut
            shortcut.TargetPath = pathToExe;
            shortcut.Save();
        }

        /// <summary>
        /// Finds the files that must download to be up to date
        /// </summary>
        /// <returns></returns>
        public Checker.ProgramInfo GetNeededFileNames()
        {
            NewText(logLabel, "Downloading new version info...");
            Checker.ProgramInfo serverInfo = Checker.GetProgramInfo();
            NewText(logLabel, "Analyzing current version info...");
            Checker.ProgramInfo currentPI = Checker.ProgramInfo.Get(CurrentDirectory);
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
                        string[] dependencies = Checker.GetUpdaterDependencies();
                        if(dependencies != null)
                            n.AddRange(dependencies);
                        // Download only the updater and its dependencies.. *** Careful ***
                        if (DownloadAllFiles(n))
                        {
                            // Updater downloaded -> open it and close this one
                            Process.Start(System.IO.Path.Combine(CurrentDirectory, "FastNotesUpdater.exe"), "update");
                            this.Dispatcher.BeginInvoke(new WindowDelegate(CloseThis), this);
                        }
                        else
                        {
                            // Operation canceled -> close it and open fastnotes
                            Process.Start(System.IO.Path.Combine(CurrentDirectory, "FastNotes.exe"));
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
                    System.IO.File.Delete(System.IO.Path.Combine(CurrentDirectory, fn + ".bak"));
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
            string path = System.IO.Path.Combine(CurrentDirectory, name);
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
            string path = System.IO.Path.Combine(CurrentDirectory, name);
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

                msg = Checker.DownloadAppFile(name, System.IO.Path.Combine(CurrentDirectory, name), deh, msPerDownloadInfo);
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

        /// <summary>
        /// Call this to fire a <see cref="PropertyChanged"/> event
        /// </summary>
        /// <param name="name"></param>
        public void OnPropertyChanged(string name)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        #endregion
    }
}
