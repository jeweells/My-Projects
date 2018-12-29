using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.Windows.Controls.Primitives;
using System.Diagnostics;
using System.Threading;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using Controls;

namespace FastNotes
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        #region Private Members

        #region Hide when this process isn't the main window

        delegate void WinEventDelegate(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);

        [DllImport("user32.dll")]
        static extern IntPtr SetWinEventHook(uint eventMin, uint eventMax, IntPtr hmodWinEventProc, WinEventDelegate lpfnWinEventProc, uint idProcess, uint idThread, uint dwFlags);

        private const uint WINEVENT_OUTOFCONTEXT = 0;
        private const uint EVENT_SYSTEM_FOREGROUND = 3;

        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        [DllImport("user32.dll")]
        static extern IntPtr GetWindowThreadProcessId(IntPtr hWnd, out int ProcessId);

        [DllImport("user32.dll")]
        static extern IntPtr GetTopWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        static extern bool UnhookWinEvent(IntPtr hWinEventHook);

        [DllImport("user32.dll")]
        static extern long SetForegroundWindow(IntPtr hwnd);

        IntPtr m_hhook = IntPtr.Zero;
        #endregion

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
        /// Items by Uid as keys
        /// </summary>
        Dictionary<string, HighlightableMenuItem> NotifyIconItems = new Dictionary<string, HighlightableMenuItem>();

        RenameManager renameManager; 

        #region Version Paths
        static string extPathv1_0_1_0 = @"FastNotes\FastNotes\1.0.1.0";
        static string extPathv1_0_1_1 = @"Jeweells\FastNotes\1.0.1.1";

        // Config paths (ordered from older to newer)
        static string configPathv1_0_0_0 = Environment.CurrentDirectory + "//config.fn";
        static string configPathv1_0_1_0 = System.Windows.Forms.Application.UserAppDataPath.Substring(0, System.Windows.Forms.Application.UserAppDataPath.Length - extPathv1_0_1_1.Length)
            + extPathv1_0_1_0 + "//config.fn"; // Previous path (trying to move data to the new path)
        static string configPathv1_0_1_1 = System.Windows.Forms.Application.UserAppDataPath.Substring(0, System.Windows.Forms.Application.UserAppDataPath.Length - extPathv1_0_1_1.Length)
            + extPathv1_0_1_1 + "//config.fn"; // Previous path (trying to move data to the new path)

        static string configPathv1_0_1_3 = System.IO.Path.Combine(System.Windows.Forms.Application.UserAppDataPath.Substring(0, System.Windows.Forms.Application.UserAppDataPath.Length - System.Windows.Forms.Application.ProductVersion.Length), "1.0.1.3", "config.fn");

        static string configPath = System.IO.Path.Combine(Desktop.GetAnyDesktopFolder(), "config.fn");// This is current


        // Data paths (ordered from older to newer)
        static string dataFilePathv1_0_0_0 = Environment.CurrentDirectory + "//data.fn";
        static string dataFilePathv1_0_1_0 = System.Windows.Forms.Application.UserAppDataPath.Substring(0, System.Windows.Forms.Application.UserAppDataPath.Length - extPathv1_0_1_1.Length)
            + extPathv1_0_1_0 + "//data.fn"; // Previous path (Trying to move data to the new path)
        static string dataFilePathv1_0_1_1 = System.Windows.Forms.Application.UserAppDataPath.Substring(0, System.Windows.Forms.Application.UserAppDataPath.Length - extPathv1_0_1_1.Length)
            + extPathv1_0_1_1 + "//data.fn"; // Previous path (Trying to move data to the new path)

        static string dataFilePathv1_0_1_3 = System.IO.Path.Combine(System.Windows.Forms.Application.UserAppDataPath.Substring(0, System.Windows.Forms.Application.UserAppDataPath.Length - System.Windows.Forms.Application.ProductVersion.Length), "1.0.1.3", "data.fn");
       
        #endregion

        /// <summary>
        /// List of items that will appear on the desktops tag, representing the name and access of each desktop
        /// </summary>
        List<MenuItem> DesktopItems;

        /// <summary>
        /// Timer that saves the data automatically
        /// </summary>
        System.Windows.Threading.DispatcherTimer timerToSave;

        /// <summary>
        /// This delegate handles when the program hides automatically when it's not longer the foreground window
        /// </summary>
        WinEventDelegate autoHideOnBarDelegate;
        #endregion

        #region Public Members


        /// <summary>
        /// Avoids problems when a note is closed and another one gets activated
        /// </summary>
        public bool IsCurrentDesktopClosing { get; private set; } = false;

        /// <summary>
        /// Allows to perform any save existed, if it's false nothing will be saved
        /// </summary>
        public static bool Savable { get; set; } = true;

        /// <summary>
        /// If true, saves when the application is going to be closed
        /// </summary>
		public bool SaveWhenClose { get; set; } = true;

        /// <summary>
        /// Handles whether the data is saved or not, so that the timer performs less actions
        /// </summary>
		public static bool DataSaved { get; set; } = true;

        /// <summary>
        /// Handles whether the config is saved or not, so that the timer perfoms less actions
        /// </summary>
		public static bool ConfigSaved { get; set; } = true;

        /// <summary>
        /// Current configuration that is meant to be saved in some file
        /// </summary>
        public static Config CurrentConfig { get; set; }

        /// <summary>
        /// Current desktop visible
        /// </summary>
        public static Desktop CurrentDesktop { protected set; get; }

        #endregion

        #region Constructor

        public MainWindow()
        {
            InitializeComponent();
            Program.MainWindow = this;
            autoHideOnBarDelegate = new WinEventDelegate(HideWhenNotForegroundWindow);
            Loaded += (a, b) =>
            {
                CreateComponents();
                renameManager = new RenameManager(NotifyContextMenu);

                Closing += new System.ComponentModel.CancelEventHandler(MainWindow_Closing);
                LoadConfig();

                InitializeNotifyIconItems(); // Initializes the dictionary of our notify icon context menu items using DFS where Keys are the Uid's of each element

                NotifyIconItems["RunAtStartupMenuItem"].IsChecked = CurrentConfig.runAtStartup;
                NotifyIconItems["AutomaticallyHideOnBar"].IsChecked = CurrentConfig.automaticallyHideOnBar;
                NotifyIconItems["askWhenClosingANote"].IsChecked = CurrentConfig.askWhenClosingANote;


                LoadData(); // Initializes if the data doesn't exist
            };
            ContentRendered += (a, b) => {
                System.Threading.Tasks.Task.Run(() =>
                {
                    string newestVersion;
                    if (!ServerLogger.Checker.IsNewestVersion(CurrentConfig.lastVersionChecked, out newestVersion))
                    {
                        Application.Current.Dispatcher.BeginInvoke((Action)(() =>
                        {
                            AskToInstallNewVersion(CurrentConfig.lastVersionChecked = newestVersion);
                        }));
                    }
                });
                HookForegroundWindowChanged();
               
            };
        }


        #endregion

        #region Private Methods


        /// <summary>
        ///  Initializes the dictionary of our notify icon context menu items using DFS where Keys are the Uid's of each element
        /// </summary>
        /// <param name="items"></param>
        private void InitializeNotifyIconItems(ItemCollection items = null)
        {
            if (items == null) items = NotifyContextMenu.Items;
            foreach (var item in items)
            {
                if (item.GetType() == typeof(HighlightableMenuItem))
                {
                    var itemCasted = (HighlightableMenuItem)item;
                    if (itemCasted.Uid != "")
                    {
                        NotifyIconItems.Add(itemCasted.Uid, itemCasted);
                    }
                    InitializeNotifyIconItems(itemCasted.Items); // DFS
                }
            }
        }

        /// <summary>
        /// Initializes all the objects for this window
        /// </summary>
        private void CreateComponents()
        {
            timerToSave = new System.Windows.Threading.DispatcherTimer();
            timerToSave.Tick += new EventHandler(OnTimerToSaveTick);
            timerToSave.Interval = new TimeSpan(0, 1, 0);
            Loaded += (x, y) => timerToSave.Start();
            System.ComponentModel.IContainer cnt = new System.ComponentModel.Container();
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
        /// Deserializes the config file into a Config class
        /// </summary>
        /// <returns></returns>
        bool LoadConfigAux()
        {
            try
            {
                XmlSerializer xs = new XmlSerializer(typeof(Config));
                using (StreamReader sr = new StreamReader(configPath))
                {
                    CurrentConfig = (Config)xs.Deserialize(sr);
                }
            }
            catch (Exception e)
            {
                Program.WriteExceptionToLog(e);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Loads the configuration
        /// </summary>
        private void LoadConfig()
        {
            if (!File.Exists(configPath))
            {
                // Takes the newer version
                if(File.Exists(configPathv1_0_1_3))
                {
                    MoveDataAndBackupProcedure(configPathv1_0_1_3, configPath);
                }
                if (File.Exists(configPathv1_0_1_1))
                {
                    MoveDataAndBackupProcedure(configPathv1_0_1_1, configPath);
                }
                else if (File.Exists(configPathv1_0_1_0))
                {
                    MoveDataAndBackupProcedure(configPathv1_0_1_0, configPath);
                }
                else if (File.Exists(configPathv1_0_0_0)) // Translate data
                {
                    MoveDataAndBackupProcedure(configPathv1_0_0_0, configPath);
                }
            }

            if (File.Exists(configPath)) // There's already a config file for this version
            {
                if (!LoadConfigAux()) // Deserializes the configuration to CurrentConfig
                {
                    // Error while loading configuration
                    Controls.MessageBox.Error(this, "Data error", "There was an error while loading the configuration", Controls.MessageBox.Accept);
                }

                if (CurrentConfig == null) // It wasn't initialized so we initialize it
                {
                    CurrentConfig = new Config();
                }

                // If we load the configuration but the data inside is in blank, we create new data
                if (CurrentConfig.desktops == null || CurrentConfig.desktops.Count == 0)
                {
                    CurrentConfig.desktops = new List<Desktop>();
                    CurrentConfig.desktops.Add(new Desktop());

                    CurrentDesktop = CurrentConfig.desktops[0];
                    CurrentConfig.selectedDesktop = 0;
                    CurrentDesktop.Name = NiceDesktopName.Random();
                    CurrentDesktop.MakeDir();
                }
                else
                {
                    try
                    {
                        // The data is correct, let's set the current desktop
                        CurrentDesktop = CurrentConfig.desktops[CurrentConfig.selectedDesktop];
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        // An unhandled error might have been thrown, we don't want the app to crash forever
                        CurrentConfig.selectedDesktop = 0;
                        CurrentDesktop = CurrentConfig.desktops[0];
                    }
                }
            }
            else
            {
                // Getting in here means there's no configuration found
                XmlSerializer xs = new XmlSerializer(typeof(Config));

                // Initialize the configuration
                CurrentConfig = new Config();
                CurrentConfig.desktops = new List<Desktop>();
                CurrentConfig.desktops.Add(new Desktop());
                // Initialize the desktop
                CurrentDesktop = CurrentConfig.desktops[0];
                CurrentConfig.selectedDesktop = 0;
                CurrentDesktop.Name = NiceDesktopName.Random();
                CurrentDesktop.MakeDir();

                try
                {
                    // Serialize the configuration
                    using (FileStream f = File.Create(configPath))
                    {
                        xs.Serialize(f, CurrentConfig);
                    }
                }
                catch (UnauthorizedAccessException)
                {
                    Controls.MessageBox.Error(this, "Unauthorized access", 
                        "The configuration file can't be accessed. The program can continue but any configuration might get lost.", 
                        Controls.MessageBox.Accept);
                }
                // Set the run at startup register key
                Program.RunAtStartup(CurrentConfig.runAtStartup);
            }

            // Make the desktops appear on the contextmenustrip tool
            DesktopItems = new List<MenuItem>(CurrentConfig.desktops.Count);

            // Add items to be visible by the user
            for (int i = 0; i < CurrentConfig.desktops.Count; i++)
            {
                DesktopsItemContainer.Items.Add(CreateDesktopMenuItem(i).MenuItem);
            }
        }

        /// <summary>
        /// Creates the item of the desktop already existed on "CurrentConfig.desktops".
        /// Be sure it exists before calling this
        /// </summary>
        DesktopInfo CreateDesktopMenuItem(int index)
        {

            // Retrieves the existing desktop
            Desktop thisDesktop = CurrentConfig.desktops[index];
            // Creates the item that contains the name of the desktop
            var desktopEditableName = new TextBox() {
                FocusVisualStyle =null,
                Text = Desktop.GetNameFromEscaped(thisDesktop.Name),
                IsEnabled = false,
                Style = (Style) Resources["MenuItemTextBox"]};
      

            var thisDesktopItem = new HighlightableMenuItem() {
                Header = desktopEditableName,
                Style = (Style)Resources["MenuItemEditable"],
                StaysOpenOnClick = true };
            renameManager.ApplyItemEvents(desktopEditableName, thisDesktop, thisDesktopItem);

            // Saves the item created
            DesktopItems.Insert(index, thisDesktopItem);

            // Creating desktop modifier items
            var deleteItem = new HighlightableMenuItem() { Header = "Delete", Icon = Application.Current.Resources["DeleteDesktopIcon"] };
            var renameItem = new HighlightableMenuItem() { Header = "Rename", StaysOpenOnClick=true, Icon = Application.Current.Resources["RenameIcon"] };
            var openItem = new HighlightableMenuItem() { Header = "Open", Icon = Application.Current.Resources["PlayIcon"] };
            var importItem = new HighlightableMenuItem() { Header = "Import", Icon = Application.Current.Resources["ImportDataInDestkopIcon"] };
            var exportItem = new HighlightableMenuItem() { Header = "Export", Icon = Application.Current.Resources["ExportDataIcon"] };
            // End Creating desktop modifier items

            // Adding Events
            deleteItem.Click += (x, y) =>
            {
                if (DeleteDesktop(thisDesktopItem, DesktopItems.FindIndex((elm) => elm == thisDesktopItem)))
                {
                        Controls.MessageBox.Success(this, null,
                            thisDesktop.Name + " was deleted successfully!",
                            Controls.MessageBox.Ok);
                        NotifyContextMenu.IsOpen = true;
                        DesktopsItemContainer.IsSubmenuOpen = true;
                }
            };


            renameItem.Click += (x, y) =>
            {
                renameManager.Prepare(desktopEditableName, thisDesktopItem, thisDesktop);
                thisDesktopItem.IsSubmenuOpen = false;// Closes the submenu
            };
            // Sender needs to be the desktop button not the load button, since the sender will be checked
            openItem.Click += (x, y) => ChangeDesktop(thisDesktopItem, y, DesktopItems.FindIndex((elm) => elm == thisDesktopItem));
            importItem.Click += (x, y) => ImportDataOn(thisDesktop);
            exportItem.Click += (x, y) => ExportDataOf(thisDesktop);
            // End Adding events


            thisDesktopItem.Items.Add(openItem);
            thisDesktopItem.Items.Add(renameItem);
            thisDesktopItem.Items.Add(deleteItem);
            thisDesktopItem.Items.Add(new Separator() { Style = (Style) Application.Current.Resources["FNSeparator"] });
            thisDesktopItem.Items.Add(importItem);
            thisDesktopItem.Items.Add(exportItem);


            thisDesktopItem.IsChecked = index == CurrentConfig.selectedDesktop;
            return new DesktopInfo() { Desktop = thisDesktop, MenuItem = thisDesktopItem, TextBox = desktopEditableName };
        }

        /// <summary>
        /// Loads the data, deserializes a file into a NoteData[] class.
        /// Also links the NoteForm with the data loaded, and initializes the NoteForm to be visible
        /// Returns false if something went wrong
        /// </summary>
        /// <returns></returns>
        bool LoadDataAux()
        {
            try
            {
                using (FileStream file = new FileStream(CurrentDesktop.DataFilePath, FileMode.Open))
                {
                    Controls.MessageBox.ShowUntil(this, "Opening " + Desktop.GetNameFromEscaped(CurrentDesktop.Name),
                        "Please, wait a moment while "+ Desktop.GetNameFromEscaped(CurrentDesktop.Name) + " data is loaded...",
                        Controls.MessageBox.MessageType.Log,
                        null, () =>
                        {
                            BinaryFormatter bf = new BinaryFormatter();
                            var deserializedFile = bf.Deserialize(file);
                            LinkedList<NoteWindow> notes = new LinkedList<NoteWindow>(); // Here we store the notes we initialize
                            Dispatcher.Invoke(() => {
                                foreach (NoteData_1_0_1_4 individualData in DataLoader.GetNewestData(deserializedFile).Reverse<object>())
                                {
                                    var newNote = Program.CreateNote() as NoteWindow;
                                    newNote.DataHandled = true;
                                    newNote.Loaded += (a, b) =>
                                    {
                                        newNote.Data = individualData;
                                    };
                                    newNote.ContentRendered += (a, b) => newNote.Owner = this;
                                    newNote.Opacity = 0; // We dont want this note to be shown right now (it'll be shown one by one, it's looks very obvious and it doesnt' look nice)
                                    newNote.Show();
                                    notes.AddLast(newNote); // Add the note to the ones that have been initialized

                                }
                                foreach (var note in notes) // Here we know they're been initialized, so what's left is to show them all 
                                                            // Now all of them will appear with ease and very likely it won't be easy to notice that one by one they appeared
                                {
                                    note.Opacity = 1;
                                }
                            });
                        
                        });
                    if (Program.AvailableNotes.Count == 0)
                    {
                        var newNote = Program.CreateNote();
                        newNote.ContentRendered += (a, b) => newNote.Owner = this;
                        newNote.Show();
                    }
                }
            }
            catch(Exception e)
            {
                Controls.MessageBox.Error(this, null, e.Message, Controls.MessageBox.Ok);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Copies the data file to a .bak file
        /// </summary>
		bool ReplaceDataForBackup()
        {
            try
            {
                File.Copy(CurrentDesktop.DataFilePath + ".bak", CurrentDesktop.DataFilePath, true);
            }
            catch (UnauthorizedAccessException)
            {
                Controls.MessageBox.Error(this, "Unauthorized access", 
                    "The data file is in use or there are not enough permissions. The application will close.",
                    Controls.MessageBox.Ok);
                SaveWhenClose = false;
                Close();
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Moves filepath and filepath.bak to newFilePath and newFilePath.bak respectively,
        /// Even if the .bak file is hidden
        /// </summary>
        void MoveDataAndBackupProcedure(string filepath, string newFilePath)
        {
            Directory.CreateDirectory(Desktop.GetAnyDesktopFolder());
            File.Copy(filepath, newFilePath);
            File.Delete(filepath);
            string backuppath = filepath + ".bak";
            if (File.Exists(backuppath))
            {
                File.SetAttributes(backuppath, File.GetAttributes(backuppath) & ~FileAttributes.Hidden);
                string newbackuppath = newFilePath + ".bak";
                File.Copy(backuppath, newbackuppath);
                File.SetAttributes(newbackuppath, File.GetAttributes(backuppath) | FileAttributes.Hidden);
                File.Delete(backuppath);
            }
        }

        /// <summary>
        /// Happens when this form is closing, we want to save the data and the configuration before it closes
        /// </summary>
		private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (SaveWhenClose)
            {
                try
                {
                    SaveProcedure();
                }
                catch(Exception ex)
                {
                    if (false == (Controls.MessageBox.Error(this, "Error", "There was an error while trying to save the data:\n" + ex.Message
                            + "\nYour data might get lost. Do you want to continue?", Controls.MessageBox.YesNo) ?? false))
                    {
                        // The user wants to keep its data therefore we cancel this closing event
                        e.Cancel = true;
                        return;
                    }
                }
            }
            foreach (NoteWindow nw in Program.AvailableNotes)
            {
                nw.CloseWithTheLastNoteOpened = true;
                nw.ForgetDataWhenClosing = true;
            }
            IsCurrentDesktopClosing = true;
            Application.Current.Shutdown();
        }


        /// <summary>
        /// Saves the data if it wasn't saved and if it's savable every time the timer ticks
        /// </summary>
		private void OnTimerToSaveTick(object sender, EventArgs e)
        {
            try
            {
                SaveProcedure();
            }
            catch { }
        }

        /// <summary>
        /// Handles when the user checks/unchecks the run at startup button
        /// </summary>
        private void RunAtStartup_MenuItemClick(object sender, RoutedEventArgs e)
        {
            var tsmi = ((MenuItem)sender);
            Program.RunAtStartup(tsmi.IsChecked ^= true);
            ConfigSaved = false;
        }

        /// <summary>
        /// Handles when the user clicks the exit button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Exit_MenuItemClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Happens when the 'New Desktop' button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewDesktop_MenuItemClick(object sender, RoutedEventArgs e)
        {
            CreateDesktop();
        }

        /// <summary>
        /// Happens when the user clicks on the 'Delete All Desktops' button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteAllDesktops_MenuItemClick(object sender, RoutedEventArgs e)
        {
            if (DeleteAllDesktops())
            {
                Controls.MessageBox.Success(this, null, "All desktops were deleted successfully!", Controls.MessageBox.Ok);
            }
        }

        /// <summary>
        /// Happens when the user clicks on the "Import Desktops" button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImportDesktops_MenuItemClick(object sender, RoutedEventArgs e)
        {
            ImportZippedData();
        }

        /// <summary>
        /// Happens when the user clicks on the "Export All Desktops" button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExportAllDesktops_MenuItemClick(object sender, RoutedEventArgs e)
        {
            ExportZippedData();
        }

        /// <summary>
        /// Happens when the user clicks on the "Update" button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Update_MenuItemClick(object sender, RoutedEventArgs e)
        {
            var waitingDialog = new Controls.MessageBox();
            System.Threading.CancellationTokenSource tokenSource = new System.Threading.CancellationTokenSource();
            System.Threading.CancellationToken token = tokenSource.Token;
            
            System.Threading.Tasks.Task.Run(() => 
            {
                string nv;
                bool isNewest;
                try
                {
                    isNewest = ServerLogger.Checker.IsNewestVersion(System.Windows.Forms.Application.ProductVersion, out nv);
                }
                catch
                {
                    try
                    {
                        Dispatcher.Invoke(() => { waitingDialog.DialogResult = true; });
                    }
                    catch { } // Maybe this throws an exception if there's nothing to close
                    Controls.MessageBox.Error(this, "Server timeout",
                        "It wasn't possible to connect with the server right now. Please, try again later.",
                        Controls.MessageBox.Ok);
                    return;
                }
                // Installation proceeds
                try
                {
                    Dispatcher.Invoke(() => { waitingDialog.DialogResult = true; });
                }
                catch { } // Maybe this throws an exception if there's nothing to close
                if (!token.IsCancellationRequested)
                {
                    Dispatcher.Invoke(() =>
                    {
                        if (isNewest)
                        {

                            Controls.MessageBox.Success(this, "FastNotes updated",
                                $"You already have the last version ({System.Windows.Forms.Application.ProductVersion}) installed.",
                                Controls.MessageBox.Ok);

                        }
                        else
                        {
                            AskToInstallNewVersion(nv);
                        }
                    });
                }

            }, token);
            if(true != (waitingDialog.ShowFromInstance("Connecting with the server",
                "Checking if there's a new version available...",
                Controls.MessageBox.MessageType.Log, Controls.MessageBox.Cancel) ?? false))
            {
                // Task cancelled
                try
                {
                    tokenSource.Cancel();
                }
                catch { }
            }
        }

        /// <summary>
        /// Asks if you want to install a new version (which is "newVersion") and if the answer is Yes, it calls "InstallNewVersion()"
        /// </summary>
        /// <param name="newVersion"></param>
        /// <param name="dialogAnswer">If null the user will be asked whether to install it or not</param>
        private void AskToInstallNewVersion(string newVersion, bool? dialogAnswer = null)
        {
            if (dialogAnswer == false) return;
            if (dialogAnswer == true || true == (Controls.MessageBox.Question(this, "New version found",
                $"There's a new version available ({newVersion}). Do you want to install it?",
                Controls.MessageBox.YesNo) ?? false)) // If the user wants to install it
            {
                while (true)
                {
                    try
                    {
                        InstallNewVersion();
                        return;
                    }
                    catch (Exception e)
                    {
                        bool tryAgain = Controls.MessageBox.Error(this, "Unexpected error happened", e.Message, new Dictionary<string, Controls.MessageBox.ButtonType>()
                        {
                            {"Try again" , Controls.MessageBox.ButtonType.Accept},
                            {"Cancel", Controls.MessageBox.ButtonType.Cancel }
                        }) ?? false;
                        if (tryAgain) continue;
                        else return;
                    }
                }
            }
        }

        /// <summary>
        /// Saves the data, opens the updater and closes the app
        /// </summary>
        private void InstallNewVersion()
        {
            Savable = true;
            DataSaved = false;
            ConfigSaved = false;
            SaveConfig();
            SaveData();
            try
            {
                var process = new System.Diagnostics.ProcessStartInfo(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FastNotesUpdater.exe"), "update");
                process.Verb = "runas"; // Run as administrator

                System.Diagnostics.Process.Start(process);
            }
            catch(Exception ex)
            {
                Controls.MessageBox.Error(this, null, "There was an error while trying to open the updater. Is it ubicated on \""+ System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FastNotesUpdater.exe") + "\"?\n" + ex.Message, Controls.MessageBox.Ok);
            }
            Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        /// <summary>
        /// Hides or show the window and its note (this method is called when the corresponding item is clicked)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HideShow_MenuItemClick(object sender, RoutedEventArgs e)
        {
            ToggleHideShow();
        }

        /// <summary>
        /// Hooks the event to know when the foreground window changes
        /// </summary>
        void HookForegroundWindowChanged()
        {
            if (m_hhook == IntPtr.Zero)
            {
                m_hhook = SetWinEventHook(
                    eventMin: EVENT_SYSTEM_FOREGROUND,
                    eventMax: EVENT_SYSTEM_FOREGROUND,
                    hmodWinEventProc: IntPtr.Zero,
                    lpfnWinEventProc: autoHideOnBarDelegate,
                    idProcess: 0,
                    idThread: 0,
                    dwFlags: WINEVENT_OUTOFCONTEXT);
            }
        }

        /// <summary>
        /// Unhooks the event to know when the foreground window changes
        /// </summary>
        void UnhookForegroundWindowChanged()
        {
            if (m_hhook != IntPtr.Zero)
            {
                UnhookWinEvent(m_hhook);
                m_hhook = IntPtr.Zero;
            }
        }

        /// <summary>
        /// Hides the window when it's not the foreground window
        /// </summary>
        void HideWhenNotForegroundWindow(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
        {
            IntPtr handle = IntPtr.Zero;
            handle = GetForegroundWindow(); // Gets the foreground window
            int pid;
            GetWindowThreadProcessId(handle, out pid); // Get the process id of that window
            string processName = Process.GetProcessById(pid).ProcessName;
            if (processName != "FastNotes" && Visibility == Visibility.Visible) // If the process id is different from mine, hide the window
            {
                if (CurrentConfig.automaticallyHideOnBar)
                    HideWindow();// Hide
                try
                {
                    SaveProcedure();
                }
                catch
                {

                }
            }
        }

        /// <summary>
        /// Ocurrs when the user clicks on the 'Automatically hide on bar' item and hooks the windows event to check when the foreground window changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AutomacaticallyHideOnBar_MenuItemClick(object sender, RoutedEventArgs e)
        {
            var item = ((HighlightableMenuItem)sender);
            CurrentConfig.automaticallyHideOnBar = item.IsChecked = !item.IsChecked;
            ConfigSaved = false;
        }

        /// <summary>
        /// Ocurrs when the user clicks on the notify icon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NotifyIcon_TrayLeftMouseDown(object sender, RoutedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                HideWindow();
            }
            else
            {
                ShowWindow();
                SetForegroundWindow(new WindowInteropHelper(this).Handle);
            }
        }

        private void AskWhenClosingANote_MenuItemClick(object sender, RoutedEventArgs e)
        {
            var item = ((HighlightableMenuItem)sender);
            CurrentConfig.askWhenClosingANote = item.IsChecked = !item.IsChecked;
            ConfigSaved = false;
        }


        #endregion

        #region Protected Methods
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Saves 'CurrentConfig' into a config file
        /// </summary>
        /// <returns></returns>
        public bool SaveConfig()
        {
            if (!Savable) return true;
            XmlSerializer xs = new XmlSerializer(typeof(Config));
            try
            {
                using (FileStream f = File.Create(configPath))
                {
                    xs.Serialize(f, CurrentConfig);
                }
            }
            catch (Exception e)
            {
                Program.WriteExceptionToLog(e);
                return false;
            }
            ConfigSaved = true;
            return true;
        }

        /// <summary>
        /// Saves all the data
        /// </summary>
        public void SaveProcedure()
        {
            if (Savable)
            {
                Debug.WriteLine($"Saved (data: {!DataSaved}, config: {!ConfigSaved})");
                if (!DataSaved) SaveData();
                if (!ConfigSaved) SaveConfig();
            }
        }

        /// <summary>
        /// Exports the data of the desktop to a fn file, the dialog is included
        /// </summary>
        public void ExportDataOf(Desktop d)
        {
            System.Windows.Forms.SaveFileDialog sfd = new System.Windows.Forms.SaveFileDialog();
            sfd.DefaultExt = ".fn";
            sfd.Title = "Select where to export " + d.Name;
            sfd.Filter = "FastNotes data file (*.fn)|*.fn|All files (*.*)|*.*";
            sfd.FileName = d.GetEscapedName() + ".fn";

            if (System.Windows.Forms.DialogResult.OK == sfd.ShowDialog(GetIWin32Window(this)))
            {
                try
                {
                    DataSaved = false;
                    SaveData();
                    File.Copy(d.DataFilePath, sfd.FileName, true);
                }
                catch (Exception e)
                {
                    Program.WriteExceptionToLog(e);
                    Controls.MessageBox.Error(this, e.Message,
                        "There was an error and the desktop couldn't be exported. Make sure you select the right place to export it.",
                        Controls.MessageBox.Ok);
                }
            }
        }

        /// <summary>
        /// Deletes the desktop
        /// </summary>
        /// <param name="renameBtn">Button in the contextmenustrp tool</param>
        /// <param name="i">Index of the desktop to be renamed</param>
        /// <param name="saveAfterDelete">Saves the data and configuration after the desktop is deleted if true</param>
        /// <param name="changeIfCurrent"></param>
        /// <returns></returns>
        public bool DeleteDesktop(MenuItem renameBtn, int i, bool saveAfterDelete = true, bool changeIfCurrent = true)
        {
            // Ask if you're sure
            Savable = false;// We don't want it to be saved by the timer when we just deleted a desktop
            if (!CurrentConfig.desktops[i].Delete()) // Deletes its data
                return false;
            DesktopItems.Remove(renameBtn); // Deletes it from the list of desktop items (from the context menu)
            DesktopsItemContainer.Items.Remove(renameBtn);// Deletes from the interface

            CurrentConfig.desktops.RemoveAt(i); // Deletes it from the list of desktops (desktop class)

            if (CurrentConfig.desktops.Count == 0)
            {
                Desktop d = CreateDesktopData(NiceDesktopName.Random()).Desktop;
                
                ChangeDesktop(DesktopItems[0], null, 0, true, false);
            }
            else if (CurrentConfig.selectedDesktop > i) CurrentConfig.selectedDesktop--; // Shifts the current desktop
            else if (CurrentConfig.selectedDesktop == i)
            {
                // Change to a default desktop (0 in this case)
                if (changeIfCurrent) ChangeDesktop(DesktopItems[0], null, 0, true, false);
            }
            Savable = true;  // Now we can save changes
            if (saveAfterDelete)
            {
                ConfigSaved = false; // The configuration has changed
                SaveConfig(); // Save the configuration
                SaveData();
            }
            return true;
        }

        /// <summary>
        /// Changes the current desktop to the clickedIdx desktop
        /// </summary>
        /// <param name="sender">Menu Item which header is the name of the desktop</param>
        /// <param name="e"></param>
        /// <param name="clickedIdx"></param>
        /// <param name="forceChange"></param>
        public void ChangeDesktop(object sender, EventArgs e, int clickedIdx, bool forceChange = false, bool saveWhenClosing = true)
        {
            if (!forceChange && clickedIdx == CurrentConfig.selectedDesktop)
            {
                ShowWindow();
                return;
            }
            try
            {
                DesktopItems[CurrentConfig.selectedDesktop].IsChecked = false; // This is not selected anymore
            }
            catch (ArgumentOutOfRangeException)
            {
                // Does nothing, this happens when the last one is deleted and also selected as current
            }
            var menuItem = ((MenuItem)sender);
            if (menuItem != null)
            {
                menuItem.IsChecked = true;
            }
            CloseCurrentDesktop(saveWhenClosing);
            CurrentDesktop = CurrentConfig.desktops[clickedIdx];
            CurrentConfig.selectedDesktop = clickedIdx;
            LoadData();
            ShowWindow();
            DataSaved = false;
            ConfigSaved = false;

            SaveConfig();
            SaveData();
        }

        /// <summary>
        /// Loads data of the CurrentDesktop.DataFilePath, handles if the file doesn't exist and loads its backup.
        /// Creates a note if the data doesn't exist or if an error happens notifies the user before creating it
        /// </summary>
		public void LoadData()
        {
            if (!File.Exists(CurrentDesktop.DataFilePath))
            {
                // Ordered by newer to older
                Directory.CreateDirectory(CurrentDesktop.FolderPath);
                if (File.Exists(CurrentDesktop.DataFilePath1_0_1_3))
                {
                    MoveDataAndBackupProcedure(CurrentDesktop.DataFilePath1_0_1_3, CurrentDesktop.DataFilePath);
                }
                if (File.Exists(dataFilePathv1_0_1_1))
                {
                    MoveDataAndBackupProcedure(dataFilePathv1_0_1_1, CurrentDesktop.DataFilePath);
                }
                else if (File.Exists(dataFilePathv1_0_1_0))
                {
                    MoveDataAndBackupProcedure(dataFilePathv1_0_1_0, CurrentDesktop.DataFilePath);
                }
                else if (File.Exists(dataFilePathv1_0_0_0))
                {
                    MoveDataAndBackupProcedure(dataFilePathv1_0_0_0, CurrentDesktop.DataFilePath);
                }
            }

            if (File.Exists(CurrentDesktop.DataFilePath))
            {
                if (!LoadDataAux()) // Some error
                {
                    // Replace the backup with the real data, then load the data again
                    if (!ReplaceDataForBackup() || !LoadDataAux())
                    {
                        // One of the above procedures went wrong
                        if(true == (Controls.MessageBox.Error(this, "Data error", 
                            "There was an error with the data. The application will create a blank note, the remaining data might get lost. Do you want to continue?",
                            new Dictionary<string, Controls.MessageBox.ButtonType>() {
                                { "Yes", Controls.MessageBox.ButtonType.Accept },
                                { "No, I want to keep my data", Controls.MessageBox.ButtonType.Cancel } }) ?? false))
                        {
                            // User chooses to ignore this error and proceeds
                            var f = Program.CreateNote();
                            f.ContentRendered += (a, b) => f.Owner = this;

                            f.Show();
                        }
                        else
                        {
                            // Close the application
                            Savable = false;
                            SaveWhenClose = false;
                            Controls.MessageBox.Log(this, "Closing", "The application will close", Controls.MessageBox.Ok);
                            Close();

                        }
                    }
                }
            }
            else
            {
                var f = Program.CreateNote();
                f.ContentRendered += (a, b) => f.Owner = this;
                f.Show();
            }
        }

        /// <summary>
        /// Saves the data of the current desktop, creates a backup if it already exists
        /// </summary>
		public void SaveData()
        {
            if (!Savable) return;
            // Get the data of all the notes visible for the current desktop
            NoteData_1_0_1_4[] currentData = new NoteData_1_0_1_4[Program.AvailableNotes.Count];
            int i = 0;
            foreach (NoteWindow note in Program.AvailableNotes)
            {
                currentData[i++] = note.Data;
            }
            // Create backup
            if (File.Exists(CurrentDesktop.DataFilePath))
            {
                File.SetAttributes(CurrentDesktop.DataFilePath, File.GetAttributes(CurrentDesktop.DataFilePath) & ~FileAttributes.Hidden);
                string backuppath = CurrentDesktop.DataFilePath + ".bak";
                if (File.Exists(backuppath)) File.SetAttributes(backuppath, File.GetAttributes(backuppath) & ~FileAttributes.Hidden);
                File.Copy(CurrentDesktop.DataFilePath, backuppath, true);
                File.SetAttributes(backuppath, File.GetAttributes(backuppath) | FileAttributes.Hidden);
            }
            else
            {
                CurrentDesktop.MakeDir();
            }
            using (FileStream file = File.Create(CurrentDesktop.DataFilePath))
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(file, currentData);
                DataSaved = true;
            }
        }

        /// <summary>
        /// Explorts all the desktops data into a zip file with extension .fnz
        /// </summary>
        public void ExportZippedData()
        {
            System.Windows.Forms.SaveFileDialog sfd = new System.Windows.Forms.SaveFileDialog();
            DateTime time = DateTime.Now;
            sfd.FileName = "FastNotes Zipped Data " + time.ToString("yyyy.MMMM.dd.H.mm.ss") + ".fnz";
            sfd.Filter = "FastNotes zipped data file (*.fnz)|*.fnz|All files (*.*)|*.*";
            if (System.Windows.Forms.DialogResult.OK == sfd.ShowDialog(GetIWin32Window(this)))
            {
                using (ZipArchive zip = ZipFile.Open(sfd.FileName, ZipArchiveMode.Create))
                {
                    SaveConfig();
                    SaveData();
                    foreach (var d in CurrentConfig.desktops)
                    {
                        zip.CreateEntryFromFile(d.DataFilePath, d.GetEscapedName() + "/data.fn");
                    }
                }
            }
        }

        /// <summary>
        /// Imports the -user selected- data to a selected desktop
        /// </summary>
        /// <param name="desktop">Desktop where the data will be imported</param>
        public void ImportDataOn(Desktop desktop)
        {
            System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog();
            ofd.Filter = "FastNotes data file (*.fn)|*.fn|All files (*.*)|*.*";
            ofd.FilterIndex = 0;
            ofd.Multiselect = false;
            if (System.Windows.Forms.DialogResult.OK == ofd.ShowDialog(GetIWin32Window(this)))
            {
                if(true == (Controls.MessageBox.Warning(this, "Replacing desktop",
                    "If you continue, the data of '" + Desktop.GetNameFromEscaped(desktop.Name) + "' will be lost. Are you sure you want to continue?",
                    Controls.MessageBox.YesNo) ?? false))
                {
                    // User chooses to replace the desktop data
                    try
                    {
                        File.Copy(ofd.FileName, desktop.DataFilePath, true);
                    }
                    catch (Exception e)
                    {
                        Controls.MessageBox.Error(this, "Desktop couldn't be imported",
                            "There was an error while importing the file.\n" + e.Message,
                            Controls.MessageBox.Ok);
                        Program.WriteExceptionToLog(e);
                        return;
                    }
                    if (desktop == CurrentDesktop)
                    {
                        CloseCurrentDesktop(false);
                        LoadData();
                    }
                    Controls.MessageBox.Success(this, null, Desktop.GetNameFromEscaped(desktop.Name) + " was imported successfully", Controls.MessageBox.Ok);
                }
            }
        }

        /// <summary>
        /// Imports either .fn and .fnz data files, it doesn't matter if the file is zipped or data alone, it will import all of them.
        /// It can select many files merged between .fn and .fnz files.
        /// </summary>
        public void ImportZippedData()
        {
            var ofd = new System.Windows.Forms.OpenFileDialog();
            ofd.Filter = "FastNotes data files (*.fnz, *.fn)|*.fnz; *.fn|All files (*.*)|*.*";
            ofd.FilterIndex = 0;
            ofd.Multiselect = true;
            if (System.Windows.Forms.DialogResult.OK == ofd.ShowDialog(GetIWin32Window(this)))
            {
                foreach (var fileName in ofd.FileNames)
                {
                    if (fileName.EndsWith(".fnz"))
                    {
                        // Zip file

                        using (ZipArchive zip = ZipFile.Open(fileName, ZipArchiveMode.Read))
                        {
                            foreach (var entry in zip.Entries)
                            {
                                string[] parts = entry.FullName.Split('/');
                                Desktop d = new Desktop();
                                d.Name = Desktop.GetNameFromEscaped(parts[0]);
                                Desktop desktopFound = null;
                                foreach (var dts in CurrentConfig.desktops)
                                {
                                    if (dts.Name == d.Name)
                                    {
                                        desktopFound = dts;
                                        break;
                                    }
                                }
                                bool replaceDesktop = true;
                                if (desktopFound != null)
                                {
                                    if( false == (Controls.MessageBox.Question(this, "Desktop already exists", 
                                        "There already exists a desktop with that name (" +Desktop.GetNameFromEscaped(d.Name) + "). Do you want to replace it?",
                                        Controls.MessageBox.YesNo) ?? false))
                                    {
                                        // User chooses not to replace the desktop

                                        replaceDesktop = false;
                                    }
                                }
                                if (replaceDesktop)
                                {
                                    if (desktopFound == null) AddDesktop(d);
                                    d.MakeDir();
                                    entry.ExtractToFile(d.DataFilePath, true);
                                    if (desktopFound == CurrentDesktop)
                                    {
                                        CloseCurrentDesktop(false); // Close it and then load it again
                                        LoadData();
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        // Data files
                        Desktop d = new Desktop();
                        string[] parts = fileName.Split(System.IO.Path.DirectorySeparatorChar);
                        parts = parts[parts.Length - 1].Split(System.IO.Path.AltDirectorySeparatorChar);
                        // c:/windows/myfilename.fn
                        // myfilename.fn
                        // myfilename <- Wanted
                        try
                        {
                            d.Name = Desktop.GetNameFromEscaped(parts[parts.Length - 1].Substring(0, parts[parts.Length - 1].Length - 3));
                        }
                        catch
                        {
                            // In case the extension isn't .fn for any reason
                            d.Name = Desktop.GetNameFromEscaped(parts[parts.Length - 1]);
                        }
                        Desktop desktopFound = null;
                        foreach (var dts in CurrentConfig.desktops)
                        {
                            if (dts.Name == d.Name)
                            {
                                desktopFound = dts;
                                break;
                            }
                        }
                        bool replaceDesktop = true;
                        if (desktopFound != null)
                        {
                            if (false == (Controls.MessageBox.Question(this, "Desktop already exists",
                                         "There already exists a desktop with that name (" + Desktop.GetNameFromEscaped(d.Name) + "). Do you want to replace it?",
                                         Controls.MessageBox.YesNo) ?? false))
                            {
                                // User chooses not to replace the desktop
                                replaceDesktop = false;
                            }
                        }
                        if (replaceDesktop)
                        {
                            if (desktopFound == null) AddDesktop(d);
                            d.MakeDir();
                            File.Copy(fileName, d.DataFilePath, true);
                            if (desktopFound == CurrentDesktop)
                            {
                                CloseCurrentDesktop(false);
                                LoadData();
                            }
                        }
                    }

                }
                ConfigSaved = false;
                DataSaved = false;
                SaveData();
                SaveConfig();
                Controls.MessageBox.Success(this, null, "All desktops were imported successfully!", Controls.MessageBox.Ok);
            }
        }

        /// <summary>
        /// Closes the current desktop 
        /// </summary>
        /// <param name="saveBeforeClosing">Whether the desktop data will be saved before closing this</param>
        public void CloseCurrentDesktop(bool saveBeforeClosing = true)
        {
            if (saveBeforeClosing)
            {
                SaveData();
                SaveConfig();
            }

            IsCurrentDesktopClosing = true;
            foreach (NoteWindow f in Program.AvailableNotes)
            {
                f.ForgetDataWhenClosing = true;
                // Close the note
                f.Close();
            }
            IsCurrentDesktopClosing = false;
            Program.AvailableNotes = new LinkedList<Window>();
            GC.Collect();
        }

        /// <summary>
        /// Creates a new desktop, a dialog to get the name of the desktop will be thrown
        /// </summary>
        public void CreateDesktop()
        {
            var desktopInfo = CreateDesktopData(NiceDesktopName.Random(), itemPosition: 0);
            renameManager.Prepare(desktopInfo.TextBox, desktopInfo.MenuItem, desktopInfo.Desktop);
        }

        /// <summary>
        /// Creates the needed data for a desktop with the name "newName" to work
        /// </summary>
        public DesktopInfo CreateDesktopData(string newName, int itemPosition = -1)
        {
            Desktop d = new Desktop();
            d.Name = newName;
            d.MakeDir();
            return AddDesktop(d, itemPosition);
        }

        /// <summary>
        /// Adds the needed elements for a desktop to work
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public DesktopInfo AddDesktop(Desktop d, int itemPosition = -1)
        {
            int idx = itemPosition == -1? CurrentConfig.desktops.Count : itemPosition;
            int startDesktopsIndex = DesktopsItemContainer.Items.Count - CurrentConfig.desktops.Count; // Needs to discard new desktop item, delete desktop item.. etc
            if (itemPosition != -1 && itemPosition <= CurrentConfig.selectedDesktop)
                CurrentConfig.selectedDesktop++; // The selected desktop changes one position
                // If the selected desktop is 0 and we insert at 0, the selected desktop should be 1 after the insertion

            CurrentConfig.desktops.Insert(idx, d);
            var desktopInfo = CreateDesktopMenuItem(idx);
            HighlightableMenuItem tmpDesktopItem = desktopInfo.MenuItem;
            if (itemPosition == -1) DesktopsItemContainer.Items.Add(tmpDesktopItem);
            else DesktopsItemContainer.Items.Insert(startDesktopsIndex + itemPosition, tmpDesktopItem);
            return desktopInfo;
        }

        /// <summary>
        /// Deletes all the desktops and creates a new one with just a blank note
        /// </summary>
        public bool DeleteAllDesktops()
        {
            Savable = false;
            int count = CurrentConfig.desktops.Count;
            bool AllDeletedSucessfully = true;
            int faileddesktops = 0;
            for (int i = 0; i < count; i++)
            {

                // Delete silently
                bool result = DeleteDesktop(DesktopItems[faileddesktops], faileddesktops, false, false);
                if (!result) faileddesktops++;
                AllDeletedSucessfully &= result;
            }
            return AllDeletedSucessfully;
        }

        /// <summary>
        /// Toogles the Hide/Show Item
        /// </summary>
        public void ToggleHideShow()
        {

            if (Visibility == Visibility.Visible)
            {
                HideWindow();
            }
            else
            {
                ShowWindow();
            }
        }

        /// <summary>
        /// Hides the window and its notes also the window icon
        /// </summary>
        public void HideWindow()
        {
            Visibility = Visibility.Hidden;
            var item = NotifyIconItems["ShowHideItem"];
            item.Header = "Show";
            try
            {
                item.Icon = Application.Current.Resources["EyeOn"];
            }
            catch { }
            Hide();
            foreach (var note in Program.AvailableNotes)
            {
                note.Visibility = Visibility.Hidden;
            }
        }

        /// <summary>
        /// Restores the state of the window and its note to default (the opposite of HideWindow)
        /// </summary>
        public void ShowWindow()
        {
            var item = NotifyIconItems["ShowHideItem"];
            item.Header = "Hide";
            try
            {
                item.Icon = Application.Current.Resources["EyeOff"];
            }
            catch { }
            Show();
            foreach (var note in Program.AvailableNotes.Reverse())
            {
                note.Visibility = Visibility.Visible;
            }
        }



        #endregion

        
    }
}
