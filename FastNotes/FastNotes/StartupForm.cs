using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Drawing.Text;
using System.Drawing;
using System.IO.Compression;


namespace FastNotes
{
	public partial class StartupForm : Form
	{
        #region Public Members

        /// <summary>
        /// Allows to perform any save existed, if it's false nothing will be saved
        /// </summary>
        public static bool Savable { get; set; } = true;

        /// <summary>
        /// If true, saves when the application is going to be closed
        /// </summary>
		public bool SaveWhenClose { get; set; } = true;

        /// <summary>
        /// Only one instance of this form
        /// </summary>
		public static StartupForm Instance;

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

        #region Private Members

        static BinaryFormatter bf = new BinaryFormatter();

		static string extPathv1_0_1_0 = @"FastNotes\FastNotes\1.0.1.0";
		static string extPathv1_0_1_1 = @"Jeweells\FastNotes\1.0.1.1";

        // Config paths (ordered from older to newer)
        static string configPathv1_0_0_0 = Environment.CurrentDirectory + "//config.fn";
        static string configPathv1_0_1_0 = Application.UserAppDataPath.Substring(0, Application.UserAppDataPath.Length - extPathv1_0_1_1.Length)
            + extPathv1_0_1_0 + "//config.fn"; // Previous path (trying to move data to the new path)
        static string configPathv1_0_1_1 = Application.UserAppDataPath.Substring(0, Application.UserAppDataPath.Length - extPathv1_0_1_1.Length)
            + extPathv1_0_1_1 + "//config.fn"; // Previous path (trying to move data to the new path)

        static string configPath = Application.UserAppDataPath + "//config.fn"; // This is current


        // Data paths (ordered from older to newer)
        static string dataFilePathv1_0_0_0 = Environment.CurrentDirectory + "//data.fn";
		static string dataFilePathv1_0_1_0 = Application.UserAppDataPath.Substring(0, Application.UserAppDataPath.Length - extPathv1_0_1_1.Length)
			+ extPathv1_0_1_0 + "//data.fn"; // Previous path (Trying to move data to the new path)
        static string dataFilePathv1_0_1_1 = Application.UserAppDataPath.Substring(0, Application.UserAppDataPath.Length - extPathv1_0_1_1.Length)
            + extPathv1_0_1_1 + "//data.fn"; // Previous path (Trying to move data to the new path)

        static string dataFilePath = Application.UserAppDataPath + "//data.fn"; // This is current

        /// <summary>
        /// List of items that will appear on the desktops tag, representing the name and access of each desktop
        /// </summary>
        List<ToolStripMenuItem> DesktopItems;

        #endregion

        #region Constructor

        public StartupForm()
		{
			Instance = this;
            InitializeComponent();
            LoadConfig();
            System.Threading.Tasks.Task.Run(() =>
            {
                string newestVersion;
                if (!ServerLogger.Checker.IsNewestVersion(CurrentConfig.lastVersionChecked, out newestVersion))
                {
                    AskToInstallNewVersion(CurrentConfig.lastVersionChecked = newestVersion);
                }
            });
            runAtStartupContextMenuStrip.Checked = CurrentConfig.runAtStartup;
            LoadData(); // Initializes if the data doesn't exist
        }

        #endregion

        #region Private Members

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
                    MessageBox.Show("There was an error while loading the configuration");
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
                    CurrentDesktop.Name = "Desktop 1";
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
                CurrentDesktop.Name = "Desktop 1";
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
                    MessageBox.Show("The configuration file can't be accessed. The program can continue but any configuration might get lost.");
                }
                // Set the run at startup register key
                Program.RunAtStartup(CurrentConfig.runAtStartup);
            }

            // Make the desktops appear on the contextmenustrip tool
            DesktopItems = new List<ToolStripMenuItem>(CurrentConfig.desktops.Count);

            // Add items to be visible by the user
            for (int i = 0; i < CurrentConfig.desktops.Count; i++)
            {
                DesktopsToolStripMenuItem.DropDownItems.Add(CreateDesktopMenuItem(i));
            }
        }

        /// <summary>
        /// Creates the item of the desktop already existed on "CurrentConfig.desktops".
        /// Be sure it exists before calling this
        /// </summary>
        ToolStripMenuItem CreateDesktopMenuItem(int index)
        {
            // Creates the item
            ToolStripMenuItem tmpDesktopBtn = new ToolStripMenuItem();
            // Retrieves the existing desktop
            Desktop thisDesktop = CurrentConfig.desktops[index];
            // Saves the item created
            DesktopItems.Add(tmpDesktopBtn);
            // Initializes the desktop item
            tmpDesktopBtn.Name = thisDesktop.Name + "StripItem";
            tmpDesktopBtn.Text = thisDesktop.Name;
            tmpDesktopBtn.Checked = index == CurrentConfig.selectedDesktop;

            ToolStripMenuItem deleteBtn = new ToolStripMenuItem();
            deleteBtn.Name = "Delete" + index + "Desktop";
            deleteBtn.Text = "Delete";
            deleteBtn.Click += (x, y) =>
            {
                if (DeleteDesktop(tmpDesktopBtn, DesktopItems.FindIndex((elm) => elm == tmpDesktopBtn)))
                {
                    MessageBox.Show(tmpDesktopBtn.Text + " was deleted successfully!");
                }
            };
            ToolStripMenuItem renameBtn = new ToolStripMenuItem();
            renameBtn.Name = "Rename" + index + "Desktop";
            renameBtn.Text = "Rename";
            renameBtn.Click += (x, y) => RenameDesktop(tmpDesktopBtn, DesktopItems.FindIndex((elm) => elm == tmpDesktopBtn));

            ToolStripMenuItem loadBtn = new ToolStripMenuItem();
            loadBtn.Name = "Load" + index + "Desktop";
            loadBtn.Text = "Load";
            // Sender needs to be the desktop button not the load button, since the sender will be checked
            loadBtn.Click += (x, y) => ChangeDesktop(tmpDesktopBtn, y, DesktopItems.FindIndex((elm) => elm == tmpDesktopBtn));
            ToolStripMenuItem importBtn = new ToolStripMenuItem();
            importBtn.Name = "Import" + index + "Desktop";
            importBtn.Text = "Import";
            importBtn.Click += (x, y) => ImportDataOn(thisDesktop);

            ToolStripMenuItem exportBtn = new ToolStripMenuItem();
            exportBtn.Name = "Export" + index + "Desktop";
            exportBtn.Text = "Export";
            exportBtn.Click += (x, y) => ExportDataOf(thisDesktop);

            ToolStripSeparator tss = new ToolStripSeparator();

            tmpDesktopBtn.DropDownItems.AddRange(new ToolStripItem[]{
                    importBtn, exportBtn,  tss, loadBtn, renameBtn, deleteBtn
                    });
            tmpDesktopBtn.Click += (x, y) => ChangeDesktop(x, y, DesktopItems.FindIndex((elm) => elm == x));
            return tmpDesktopBtn;
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
                    foreach (NoteData note in (NoteData[])bf.Deserialize(file))
                    {
                        NoteForm f = (NoteForm)Program.CreateNote();
                        f.Load += (x, y) => f.Data = note;
                        f.Show(this);
                    }
                    if (Program.availableForms.Count == 0)
                    {
                        Form f = Program.CreateNote();
                        f.Show(this);
                    }
                }
            }
            catch
            {
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
                File.Copy(dataFilePath + ".bak", dataFilePath, true);
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("The data file is in use. The application will close.");
                SaveWhenClose = false;
                Application.Exit();
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

        private void StartupForm_Activated(object sender, EventArgs e)
        {
        }

        private void StartupForm_Deactivate(object sender, EventArgs e)
        {
        }

        private void StartupForm_Resize(object sender, EventArgs e)
        {

        }

        private void StartupForm_Leave(object sender, EventArgs e)
        {
        }

        private void StartupForm_VisibleChanged(object sender, EventArgs e)
        {


        }

        /// <summary>
        /// Happens when this form is closing, we want to save the data and the configuration before it closes
        /// </summary>
		private void StartupForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (SaveWhenClose)
            {
                SaveData();
                SaveConfig();
            }
        }

        /// <summary>
        /// Saves the data if it wasn't saved and if it's savable every time the timer ticks
        /// </summary>
		private void timer1_Tick(object sender, EventArgs e)
        {
            if (Savable)
            {
                if (!DataSaved) SaveData();
                if (!ConfigSaved) SaveConfig();
            }
        }

        private void contextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        /// <summary>
        /// Handles when the user checks/unchecks the run at startup button
        /// </summary>
        private void runAtStartupContextMenuStrip_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem tsmi = ((ToolStripMenuItem)sender);
            Program.RunAtStartup(tsmi.Checked = !tsmi.Checked);
            ConfigSaved = false;
        }

        /// <summary>
        /// Handles when the user clicks the exit button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {

            Application.Exit();
        }

        /// <summary>
        /// Happens when the 'New Desktop' button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newDesktopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateDesktop();
        }

        /// <summary>
        /// Happens when the user clicks on the 'Delete All Desktops' button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void deleteAllDesktopsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteAllDesktops();
        }

        /// <summary>
        /// Happens when the user clicks on the "Import Desktops" button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void importDesktopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ImportZippedData();
        }

        /// <summary>
        /// Happens when the user clicks on the "Export All Desktops" button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exportAllDesktopsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExportZippedData();
        }

        /// <summary>
        /// Happens when the user clicks on the "Update" button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void updateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Threading.Tasks.Task.Run(() => {
                string nv;
                try
                {
                    bool available = ServerLogger.Checker.IsNewestVersion(Application.ProductVersion, out nv);
                    if (available)
                    {
                        MessageBox.Show($"You already have the last version ({Application.ProductVersion}) installed.", "FastNotes Updated");
                    }
                    else
                    {
                        AskToInstallNewVersion(nv);
                    }
                }
                catch (Exception f)
                {
                    MessageBox.Show(f.ToString());
                    MessageBox.Show("It wasn't possible to connect with the server right now. Please, try again later.");
                }
            });
            MessageBox.Show("Checking if there's a new version available...");
        }

        /// <summary>
        /// Asks if you want to install a new version (which is "newVersion") and if the answer is Yes, it calls "InstallNewVersion()"
        /// </summary>
        /// <param name="newVersion"></param>
        private void AskToInstallNewVersion(string newVersion)
        {
            if (DialogResult.Yes == MessageBox.Show($"There's a new version available ({newVersion}). Do you want to install it?", "New version found", MessageBoxButtons.YesNo))
            {
                InstallNewVersion();

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
            var process = new System.Diagnostics.ProcessStartInfo(Path.Combine(Environment.CurrentDirectory, "FastNotesUpdater.exe"), "update");
            process.Verb = "runas"; // Run as administrator
            System.Diagnostics.Process.Start(process);
            Application.Exit();
        }

        #endregion 

        #region Public Members

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
			catch(Exception e)
			{
				Program.WriteExceptionToLog(e);
				return false;
			}
			ConfigSaved = true;
			return true;
		}

        /// <summary>
        /// Exports the data of the desktop to a fn file, the dialog is included
        /// </summary>
        public void ExportDataOf(Desktop d)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.DefaultExt = ".fn";
            sfd.Title = "Select where to export " + d.Name;
            sfd.Filter = "FastNotes data file (*.fn)|*.fn|All files (*.*)|*.*";
            sfd.FileName = d.GetEscapedName() + ".fn";
            if(DialogResult.OK == sfd.ShowDialog())
            {
                try
                {
                    DataSaved = false;
                    SaveData();
                    File.Copy(d.DataFilePath, sfd.FileName, true);
                }
                catch(Exception e)
                {
                    Program.WriteExceptionToLog(e);
                    MessageBox.Show("There was an error and the desktop couldn't be exported. Make sure you select the right place to export it.");
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
        public bool DeleteDesktop(ToolStripMenuItem renameBtn, int i, bool saveAfterDelete = true, bool changeIfCurrent = true)
        {
            // Ask if you're sure
            Savable = false;// We don't want it to be saved by the timer when we just deleted a desktop
            if (!CurrentConfig.desktops[i].Delete()) // Deletes its data
                return false;
            DesktopItems.Remove(renameBtn);
            DesktopsToolStripMenuItem.DropDownItems.Remove(renameBtn); // Deletes from the interface
            CurrentConfig.desktops.RemoveAt(i); // Deletes from the list

            if (CurrentConfig.desktops.Count == 0)
            {
                Desktop d = CreateDesktopData("Desktop 1");
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
        /// Renames the desktop
        /// </summary>
        /// <param name="renameBtn">Button in the contextmenustrp tool</param>
        /// <param name="i">Index of the desktop to be renamed</param>
        public void RenameDesktop(ToolStripMenuItem renameBtn, int i)
        {
            string newName;
            RenameDialog renameDialog = new RenameDialog();
            renameDialog.Text = "Rename " + renameBtn.Text;
            renameDialog.OnAccepting += (e) =>
            {
                foreach (Desktop d in CurrentConfig.desktops)
                {
                    if(e.Input == d.Name)
                    {
                        MessageBox.Show("There already exists a desktop with that name!");
                        e.Handled = true;
                        break;
                    }
                }
            };
            renameDialog.newNameTextBox.Text = CurrentConfig.desktops[i].Name;
            if (DialogResult.OK == renameDialog.ShowDialog(this, out newName))
            {
                CurrentConfig.desktops[i].Rename(newName);
                renameBtn.Text = newName;
                ConfigSaved = false;
                SaveConfig();
            }
        }
        
        /// <summary>
        /// Changes the current desktop to the clickedIdx desktop
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <param name="clickedIdx"></param>
        /// <param name="forceChange"></param>
        public void ChangeDesktop(object sender, EventArgs e, int clickedIdx, bool forceChange = false, bool saveWhenClosing = true)
        {
            if (!forceChange && clickedIdx == CurrentConfig.selectedDesktop) return;
            try
            {
                ((ToolStripMenuItem)DesktopItems[CurrentConfig.selectedDesktop]).Checked = false; // This is not selected anymore
            }
            catch(ArgumentOutOfRangeException)
            {
                // Does nothing, this happens when the last one is deleted and also selected as current
            }
            ToolStripMenuItem tsmi = ((ToolStripMenuItem)sender);
            if(tsmi !=null)
            {
                tsmi.Checked = true;
            }
            CloseCurrentDesktop(saveWhenClosing);
            CurrentDesktop = CurrentConfig.desktops[clickedIdx];
            CurrentConfig.selectedDesktop = clickedIdx;
            LoadData();
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
            if(!File.Exists(CurrentDesktop.DataFilePath))
            {
                // Ordered by newer to older
                if (File.Exists(dataFilePathv1_0_1_1))
                {
                    MoveDataAndBackupProcedure(dataFilePathv1_0_1_1, CurrentDesktop.DataFilePath);
                }
                else if(File.Exists(dataFilePathv1_0_1_0))
                {
                    MoveDataAndBackupProcedure(dataFilePathv1_0_1_0, CurrentDesktop.DataFilePath);
                }
                else if (File.Exists(dataFilePathv1_0_0_0))
                {
                    MoveDataAndBackupProcedure(dataFilePathv1_0_0_0, CurrentDesktop.DataFilePath);
                }
            }
			
			if(File.Exists(CurrentDesktop.DataFilePath))
			{
				if(!LoadDataAux()) // Some error
				{
					// Replace the backup with the real data, then load the data again
					if(!ReplaceDataForBackup() || !LoadDataAux())
					{
						// One of the above procedures went wrong
						if(DialogResult.Yes == MessageBox.Show("There was an error with the data. The application will create a blank note, the remaining data might get lost. Do you want to continue?", "Data error", MessageBoxButtons.YesNo))
						{
							Form f = Program.CreateNote();
							f.Show(this);
						}
						else
						{
							SaveWhenClose = false;
							Application.Exit();
						}
					}
				}
			}
			else
			{
				Form f = Program.CreateNote();
				f.Show(this);
			}
		}

        /// <summary>
        /// Saves the data of the current desktop, creates a backup if it already exists
        /// </summary>
		public void SaveData()
		{
            if (!Savable) return;
            // Get the data of all the notes visible for the current desktop
			NoteData[] currentData = new NoteData[Program.availableForms.Count];
			int i = 0;
			foreach (NoteForm note in Program.availableForms)
			{
				currentData[i++] = note.Data;
			}
			// Create backup
			if(File.Exists(CurrentDesktop.DataFilePath))
			{
				string backuppath = CurrentDesktop.DataFilePath + ".bak";
				if(File.Exists(backuppath)) File.SetAttributes(backuppath, File.GetAttributes(backuppath) & ~FileAttributes.Hidden);
				File.Copy(CurrentDesktop.DataFilePath, backuppath, true);
				File.SetAttributes(backuppath, File.GetAttributes(backuppath) | FileAttributes.Hidden);
			}
            else
            {
                CurrentDesktop.MakeDir();
            }
			using (FileStream file = File.Create(CurrentDesktop.DataFilePath))
			{
				bf.Serialize(file, currentData);
				DataSaved = true;
			}
		}

        /// <summary>
        /// Explorts all the desktops data into a zip file with extension .fnz
        /// </summary>
        public void ExportZippedData()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            DateTime time = DateTime.Now;
            sfd.FileName = "FastNotes Zipped Data " + time.ToString("yyyy.MMMM.dd.H.mm.ss") + ".fnz";
            sfd.Filter = "FastNotes zipped data file (*.fnz)|*.fnz|All files (*.*)|*.*";
            if (DialogResult.OK == sfd.ShowDialog())
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
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "FastNotes data file (*.fn)|*.fn|All files (*.*)|*.*";
            ofd.FilterIndex = 0;
            ofd.Multiselect = false;
            if (DialogResult.OK == ofd.ShowDialog())
            {
                if (DialogResult.Yes == MessageBox.Show("If you continue, the data of '"+desktop.Name+"' will be lost. Are you sure you want to continue?", "Replacing old data", MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
                {
                    try
                    {
                        File.Copy(ofd.FileName, desktop.DataFilePath, true);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("There was an error importing the file.", "Desktop not imported", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Program.WriteExceptionToLog(e);
                        return;
                    }
                    if(desktop == CurrentDesktop)
                    {
                        CloseCurrentDesktop(false);
                        LoadData();
                    }
                    MessageBox.Show(desktop.Name + " imported successfully");
                }
            }
        }

        /// <summary>
        /// Imports either .fn and .fnz data files, it doesn't matter if the file is zipped or data alone, it will import all of them.
        /// It can select many files merged between .fn and .fnz files.
        /// </summary>
        public void ImportZippedData()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "FastNotes data files (*.fnz, *.fn)|*.fnz; *.fn|All files (*.*)|*.*";
            ofd.FilterIndex = 0;
            ofd.Multiselect = true;
            if(DialogResult.OK == ofd.ShowDialog())
            {
                foreach (var fileName in ofd.FileNames)
                {
                    if(fileName.EndsWith(".fnz"))
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
                                    if(dts.Name == d.Name)
                                    {
                                        desktopFound = dts;
                                        break;
                                    }
                                }
                                bool replaceDesktop = true;
                                if(desktopFound != null)
                                {
                                    if(DialogResult.No == MessageBox.Show("There already exists a desktop with name " + d.Name + ". Do you want to replace it?", "Replace desktop", MessageBoxButtons.YesNo, MessageBoxIcon.Question)){

                                        replaceDesktop = false;
                                    }
                                }
                                if(replaceDesktop)
                                {
                                    if(desktopFound == null) AddDesktop(d);
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
                        string[] parts = fileName.Split(Path.DirectorySeparatorChar);
                        parts = parts[parts.Length - 1].Split(Path.AltDirectorySeparatorChar);
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
                            if (DialogResult.No == MessageBox.Show("There already exists a desktop with name " + d.Name + ". Do you want to replace it?", "Replace desktop", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                            {

                                replaceDesktop = false;
                            }
                        }
                        if (replaceDesktop)
                        {
                            if(desktopFound == null) AddDesktop(d);
                            d.MakeDir();
                            File.Copy(fileName, d.DataFilePath, true);
                            if(desktopFound == CurrentDesktop)
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
                MessageBox.Show("All desktops were imported successfully!");
            }
        }

        /// <summary>
        /// Closes the current desktop 
        /// </summary>
        /// <param name="saveBeforeClosing">Whether the desktop data will be saved before closing this</param>
        public void CloseCurrentDesktop(bool saveBeforeClosing = true)
        {
            if(saveBeforeClosing)
            {
                SaveData();
                SaveConfig();
            }
            foreach (NoteForm f in Program.availableForms)
            {
                f.ForgetDataWhenClosing = true;
                // Close the note
                f.Close();
            }
            Program.availableForms = new LinkedList<Form>();
        }

        /// <summary>
        /// Creates a new desktop, a dialog to get the name of the desktop will be thrown
        /// </summary>
        public void CreateDesktop()
        {
            RenameDialog renameDialog = new RenameDialog();
            string defaultName;
            int i = 1;
            bool foundRepeated;
            do
            {
                foundRepeated = false;
                defaultName = "Desktop " + i++;
                foreach (Desktop d in CurrentConfig.desktops)
                {
                    if (d.Name == defaultName) { foundRepeated = true; break; }
                }
            } while (foundRepeated);
            renameDialog.Text = "Create new desktop";
            renameDialog.newNameTextBox.Text = defaultName;
            string newName;
            if(renameDialog.ShowDialog(this, out newName) == DialogResult.OK)
            {
                CreateDesktopData(newName);
                ChangeDesktop(DesktopItems[CurrentConfig.desktops.Count - 1], null, CurrentConfig.desktops.Count - 1);
            }
        }

        /// <summary>
        /// Creates the needed data for a desktop with the name "newName" to work
        /// </summary>
        public Desktop CreateDesktopData(string newName)
        {
            Desktop d = new Desktop();
            d.Name = newName;
            d.MakeDir();
            AddDesktop(d);
            return d;
        }

        /// <summary>
        /// Adds the needed elements for a desktop to work
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public ToolStripMenuItem AddDesktop(Desktop d)
        {
            int idx = CurrentConfig.desktops.Count;
            CurrentConfig.desktops.Add(d);
            ToolStripMenuItem tmpDesktopBtn = CreateDesktopMenuItem(idx);
            DesktopsToolStripMenuItem.DropDownItems.Add(tmpDesktopBtn);
            return tmpDesktopBtn;
        }

        /// <summary>
        /// Deletes all the desktops and creates a new one with just a blank note
        /// </summary>
        public void DeleteAllDesktops()
        {
            Savable = false;
            int count = CurrentConfig.desktops.Count;
            for (int i = 0; i < count; i++)
            {

                // Delete silently
                DeleteDesktop(DesktopItems[0], 0, false, false);
            }
        }

        #endregion
    }
}
