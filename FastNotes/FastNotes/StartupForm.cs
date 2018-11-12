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
        public static bool savable = true;
		public bool saveWhenClose = true;
		public static StartupForm Instance;
		public static bool dataSaved = true;
		public static bool configSaved = true;
		public PrivateFontCollection customFonts = new PrivateFontCollection();
		public FontFamily novaSquare;
		static BinaryFormatter bf = new BinaryFormatter();

		static string extPathv1_0_1_0 = @"FastNotes\FastNotes\1.0.1.0";
		static string extPathv1_0_1_1 = @"Jeweells\FastNotes\1.0.1.1";

        // Config paths (ordered from older to newer)
        public static string configPathv1_0_0_0 = Environment.CurrentDirectory + "//config.fn";
		public static string configPathv1_0_1_0 = Application.UserAppDataPath.Substring(0, Application.UserAppDataPath.Length - extPathv1_0_1_1.Length)
			+ extPathv1_0_1_0 + "//config.fn"; // Previous path (trying to move data to the new path)
        public static string configPathv1_0_1_1 = Application.UserAppDataPath.Substring(0, Application.UserAppDataPath.Length - extPathv1_0_1_1.Length)
            + extPathv1_0_1_1 + "//config.fn"; // Previous path (trying to move data to the new path)

        public static string configPath = Application.UserAppDataPath + "//config.fn"; // This is current


        // Data paths (ordered from older to newer)
		static string dataFilePathv1_0_0_0 = Environment.CurrentDirectory + "//data.fn";
		static string dataFilePathv1_0_1_0 = Application.UserAppDataPath.Substring(0, Application.UserAppDataPath.Length - extPathv1_0_1_1.Length)
			+ extPathv1_0_1_0 + "//data.fn"; // Previous path (Trying to move data to the new path)
        static string dataFilePathv1_0_1_1 = Application.UserAppDataPath.Substring(0, Application.UserAppDataPath.Length - extPathv1_0_1_1.Length)
            + extPathv1_0_1_1 + "//data.fn"; // Previous path (Trying to move data to the new path)

        static string dataFilePath = Application.UserAppDataPath + "//data.fn"; // This is current


		public Config currentConfig;

        public Desktop currentDesktop { protected set; get; }

		public StartupForm()
		{
			Instance = this;
            InitializeComponent();
            LoadConfig();
            System.Threading.Tasks.Task.Run(() =>
            {
                string newestVersion;
                if (!ServerLogger.Checker.IsNewestVersion(currentConfig.lastVersionChecked, out newestVersion))
                {
                    AskToInstallNewVersion(currentConfig.lastVersionChecked = newestVersion);
                }
            });
            
            byte[] novaSquareData = Properties.Resources.NovaSquare;
            IntPtr data = Marshal.AllocCoTaskMem(novaSquareData.Length);
            Marshal.Copy(novaSquareData, 0, data, novaSquareData.Length);
            customFonts.AddMemoryFont(data, novaSquareData.Length);
            novaSquare = customFonts.Families[0];

            runAtStartupContextMenuStrip.Checked = currentConfig.runAtStartup;
            LoadData(); // Initializes if the data doesn't exist
        }

        List<ToolStripMenuItem> desktopItems;

        public bool SaveConfig()
		{
            if (!savable) return true;
			XmlSerializer xs = new XmlSerializer(typeof(Config));
			try
			{
				using (FileStream f = File.Create(configPath))
				{
					xs.Serialize(f, currentConfig);
				}
			}
			catch(Exception e)
			{
				Program.WriteExceptionToLog(e);
				return false;
			}
			configSaved = true;
			return true;
		}

		bool LoadConfigAux()
		{
			try
			{
				XmlSerializer xs = new XmlSerializer(typeof(Config));
				using (StreamReader sr = new StreamReader(configPath))
				{
					currentConfig = (Config)xs.Deserialize(sr);
				}
			}
			catch(Exception e)
			{
				Program.WriteExceptionToLog(e);
				return false;
			}
			return true;
		}

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

			if (File.Exists(configPath))
			{
				if(!LoadConfigAux())
				{
                    // Error while loading configuration
                    MessageBox.Show("There was an error while loading the configuration");
				}
                if(currentConfig == null)
                {
                    currentConfig = new Config();
                }
                if (currentConfig.desktops == null || currentConfig.desktops.Count == 0)
                {
                    currentConfig.desktops = new List<Desktop>();
                    currentConfig.desktops.Add(new Desktop());

                    currentDesktop = currentConfig.desktops[0];
                    currentConfig.selectedDesktop = 0;
                    currentDesktop.Name = "Desktop 1";
                    currentDesktop.MakeDir();

                }
                else
                {
                    currentDesktop = currentConfig.desktops[currentConfig.selectedDesktop];
                }
			}
			else
			{
				XmlSerializer xs = new XmlSerializer(typeof(Config));
				currentConfig = new Config();

                currentConfig.desktops = new List<Desktop>();
                currentConfig.desktops.Add(new Desktop());
                currentDesktop = currentConfig.desktops[0];
                currentConfig.selectedDesktop = 0;
                currentDesktop.Name = "Desktop 1";
                currentDesktop.MakeDir();
                try
				{
					using (FileStream f = File.Create(configPath))
					{
						xs.Serialize(f, currentConfig);
					}
				}
				catch(UnauthorizedAccessException)
				{
					MessageBox.Show("The configuration file can't be accessed. The program can continue but any configuration might get lost.");
				}
				Program.RunAtStartup(currentConfig.runAtStartup);
			}
            // Make the desktops appear on the contextmenustrip tool
            desktopItems = new List<ToolStripMenuItem>(currentConfig.desktops.Count);

            for (int i = 0; i < currentConfig.desktops.Count; i++)
            {
                desktopsToolStripMenuItem.DropDownItems.Add(CreateDesktopMenuItem(i));
            }
        }


        ToolStripMenuItem CreateDesktopMenuItem(int index)
        {
            ToolStripMenuItem tmpDesktopBtn = new ToolStripMenuItem();
            Desktop thisDesktop = currentConfig.desktops[index];
            desktopItems.Add(tmpDesktopBtn);
            tmpDesktopBtn.Name = thisDesktop.Name + "StripItem";
            tmpDesktopBtn.Text = thisDesktop.Name;
            tmpDesktopBtn.Checked = index == currentConfig.selectedDesktop;

            ToolStripMenuItem deleteBtn = new ToolStripMenuItem();
            deleteBtn.Name = "Delete" + index + "Desktop";
            deleteBtn.Text = "Delete";
            deleteBtn.Click += (x, y) =>
            {
                if (DeleteDesktop(tmpDesktopBtn, desktopItems.FindIndex((elm) => elm == tmpDesktopBtn)))
                {
                    MessageBox.Show(tmpDesktopBtn.Text + " was deleted successfully!");
                }
            };
            ToolStripMenuItem renameBtn = new ToolStripMenuItem();
            renameBtn.Name = "Rename" + index + "Desktop";
            renameBtn.Text = "Rename";
            renameBtn.Click += (x, y) => RenameDesktop(tmpDesktopBtn, desktopItems.FindIndex((elm) => elm == tmpDesktopBtn));

            ToolStripMenuItem loadBtn = new ToolStripMenuItem();
            loadBtn.Name = "Load" + index + "Desktop";
            loadBtn.Text = "Load";
            // Sender needs to be the desktop button not the load button, since the sender will be checked
            loadBtn.Click += (x, y) => ChangeDesktop(tmpDesktopBtn, y, desktopItems.FindIndex((elm) => elm == tmpDesktopBtn));
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
            tmpDesktopBtn.Click += (x, y) => ChangeDesktop(x, y, desktopItems.FindIndex((elm) => elm == x));
            return tmpDesktopBtn;
        }

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
                    dataSaved = false;
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
        bool DeleteDesktop(ToolStripMenuItem renameBtn, int i, bool saveAfterDelete = true, bool changeIfCurrent = true)
        {
            // Ask if you're sure

            if (!currentConfig.desktops[i].Delete()) // Deletes its data
                return false;
            desktopItems.Remove(renameBtn);
            desktopsToolStripMenuItem.DropDownItems.Remove(renameBtn); // Deletes from the interface
            currentConfig.desktops.RemoveAt(i); // Deletes from the list
            if (currentConfig.selectedDesktop > i) currentConfig.selectedDesktop--; // Shifts the current desktop
            else if (currentConfig.selectedDesktop == i)
            {
                // Change to a default desktop (0 in this case)
                if(changeIfCurrent) ChangeDesktop(null, null, 0);
            }
            if(saveAfterDelete)
            {
                configSaved = false; // The configuration has changed
                SaveConfig(); // Save the configuration
            }
            return true;  
        }

        /// <summary>
        /// Renames the desktop
        /// </summary>
        /// <param name="renameBtn">Button in the contextmenustrp tool</param>
        /// <param name="i">Index of the desktop to be renamed</param>
        void RenameDesktop(ToolStripMenuItem renameBtn, int i)
        {
            string newName;
            RenameDialog renameDialog = new RenameDialog();
            renameDialog.Text = "Rename " + renameBtn.Text;
            renameDialog.OnAccepting += (e) =>
            {
                foreach (Desktop d in currentConfig.desktops)
                {
                    if(e.Input == d.Name)
                    {
                        MessageBox.Show("There already exists a desktop with that name!");
                        e.Handled = true;
                        break;
                    }
                }
            };
            renameDialog.newNameTextBox.Text = currentConfig.desktops[i].Name;
            if (DialogResult.OK == renameDialog.ShowDialog(this, out newName))
            {
                currentConfig.desktops[i].Rename(newName);
                renameBtn.Text = newName;
                configSaved = false;
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
        void ChangeDesktop(object sender, EventArgs e, int clickedIdx, bool forceChange = false)
        {
            if (!forceChange && clickedIdx == currentConfig.selectedDesktop) return;
            ((ToolStripMenuItem)desktopItems[currentConfig.selectedDesktop]).Checked = false; // This is not selected anymore
            ToolStripMenuItem tsmi = ((ToolStripMenuItem)sender);
            if(tsmi !=null)
            {
                tsmi.Checked = true;
            }
            CloseCurrentDesktop(true);
            currentDesktop = currentConfig.desktops[clickedIdx];
            currentConfig.selectedDesktop = clickedIdx;
            LoadData();
            dataSaved = false;
            configSaved = false;

            SaveConfig();
            SaveData();
        }

		/// <summary>
		/// Returns false if something went wrong
		/// </summary>
		/// <returns></returns>
		bool LoadDataAux()
		{
			try
			{
				using (FileStream file = new FileStream(currentDesktop.DataFilePath, FileMode.Open))
				{
					foreach (NoteData note in (NoteData[])bf.Deserialize(file))
					{
						NoteForm f = (NoteForm)Program.CreateNote();
                        f.Load += (x,y) => f.Data = note;
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

		bool ReplaceDataForBackup()
		{
			try
			{
				File.Copy(dataFilePath + ".bak", dataFilePath, true);
			}
			catch(UnauthorizedAccessException)
			{
				MessageBox.Show("The data file is in use. The application will close.");
				saveWhenClose = false;
				Application.Exit();
			}
			catch
			{
				return false;
			}
			return true;
		}

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

		public void LoadData()
		{
            if(!File.Exists(currentDesktop.DataFilePath))
            {
                // Ordered by newer to older
                if (File.Exists(dataFilePathv1_0_1_1))
                {
                    MoveDataAndBackupProcedure(dataFilePathv1_0_1_1, currentDesktop.DataFilePath);
                }
                else if(File.Exists(dataFilePathv1_0_1_0))
                {
                    MoveDataAndBackupProcedure(dataFilePathv1_0_1_0, currentDesktop.DataFilePath);
                }
                else if (File.Exists(dataFilePathv1_0_0_0))
                {
                    MoveDataAndBackupProcedure(dataFilePathv1_0_0_0, currentDesktop.DataFilePath);
                }
            }
			
			if(File.Exists(currentDesktop.DataFilePath))
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
							saveWhenClose = false;
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

		private void StartupForm_Activated(object sender, EventArgs e)
		{
		}
		private void StartupForm_Deactivate(object sender, EventArgs e)
		{
		}

		public void SaveData()
		{
            if (!savable) return;
			NoteData[] currentData = new NoteData[Program.availableForms.Count];
			int i = 0;
			foreach (NoteForm note in Program.availableForms)
			{
				currentData[i++] = note.Data;
			}
			// Create backup
			if(File.Exists(currentDesktop.DataFilePath))
			{
				string backuppath = currentDesktop.DataFilePath + ".bak";
				if(File.Exists(backuppath)) File.SetAttributes(backuppath, File.GetAttributes(backuppath) & ~FileAttributes.Hidden);
				File.Copy(currentDesktop.DataFilePath, backuppath, true);
				File.SetAttributes(backuppath, File.GetAttributes(backuppath) | FileAttributes.Hidden);
			}
            else
            {
                currentDesktop.MakeDir();
            }
			using (FileStream file = File.Create(currentDesktop.DataFilePath))
			{
				bf.Serialize(file, currentData);
				dataSaved = true;
			}
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

		private void StartupForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (saveWhenClose)
			{
				SaveData();
				SaveConfig();
			}
		}

		private void timer1_Tick(object sender, EventArgs e)
		{
            if(savable)
            {
                if (!dataSaved) SaveData();
                if (!configSaved) SaveConfig();
            }
		}

		private void contextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
		{
			
		}


		private void runAtStartupContextMenuStrip_Click(object sender, EventArgs e)
		{
			ToolStripMenuItem tsmi = ((ToolStripMenuItem)sender);
			Program.RunAtStartup(tsmi.Checked = !tsmi.Checked);
			configSaved = false;
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{

			Application.Exit();
		}

        private void ExportZipDataClick(object sender, EventArgs e)
        {
            ExportZippedData();
        }


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
                    foreach (var d in currentConfig.desktops)
                    {
                        zip.CreateEntryFromFile(d.DataFilePath, d.GetEscapedName() + "/data.fn");
                    }
                }
            }
        }

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
                    if(desktop == currentDesktop)
                    {
                        CloseCurrentDesktop(false);
                        LoadData();
                    }
                    MessageBox.Show(desktop.Name + " imported successfully");
                }
            }
        }


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
                                foreach (var dts in currentConfig.desktops)
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
                                    if (desktopFound == currentDesktop)
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
                        foreach (var dts in currentConfig.desktops)
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
                            if(desktopFound == currentDesktop)
                            {
                                CloseCurrentDesktop(false);
                                LoadData();
                            }
                        }
                    }

                }
                configSaved = false;
                dataSaved = false;
                SaveData();
                SaveConfig();
                MessageBox.Show("All desktops were imported successfully!");


                //if (DialogResult.Yes == MessageBox.Show("If you continue, current data will be lost. Are you sure you want to continue?", "Replacing old data", MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
                //{
                //    try
                //    {
                //        using (ZipArchive zip = ZipFile.Open(ofd.FileName, ZipArchiveMode.Read))
                //        {
                //            ZipArchiveEntry configEntr = zip.GetEntry("config.fn");
                //            ZipArchiveEntry dataEntr = zip.GetEntry("data.fn");
                //            if (dataEntr == null || configEntr == null)
                //                throw new Exception("File error");
                //            // Make sure it makes a backup
                //            SaveData();
                //            SaveConfig();
                //            // Disable any way of saving while this is extracting data
                //            savable = false;
                //            // Extract the data
                //            dataEntr.ExtractToFile(dataFilePath, true);
                //            configEntr.ExtractToFile(configPath, true);
                //        }
                //    }
                //    catch(Exception e)
                //    {
                //        MessageBox.Show("There was an error reading the file. The data won't be imported.", "Incorrect format", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //        Program.WriteExceptionToLog(e);
                //        return;
                //    }
                //    // This happens only if the data was correctly read
                //    CloseCurrentDesktop(false);
                //    LoadConfig();
                //    LoadData();
                //    savable = true;
                //}
            }
        }

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


        private void ImportZipDataClick(object sender, EventArgs e)
        {
            ImportZippedData();
        }


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
                foreach (Desktop d in currentConfig.desktops)
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
                ChangeDesktop(desktopItems[currentConfig.desktops.Count - 1], null, currentConfig.desktops.Count - 1);
            }
        }
        Desktop CreateDesktopData(string newName)
        {
            Desktop d = new Desktop();
            d.Name = newName;
            d.MakeDir();
            AddDesktop(d);
            return d;
        }
        ToolStripMenuItem AddDesktop(Desktop d)
        {
            currentConfig.desktops.Add(d);
            ToolStripMenuItem tmpDesktopBtn = CreateDesktopMenuItem(currentConfig.desktops.Count - 1);
            desktopsToolStripMenuItem.DropDownItems.Add(tmpDesktopBtn);
            return tmpDesktopBtn;
        }


        private void newDesktopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateDesktop();
        }

        public void DeleteAllDesktops()
        {
            savable = false;
            while(currentConfig.desktops.Count != 0)
            {
                // Delete silently
                DeleteDesktop(desktopItems[0], 0, false, false);
            }
            currentDesktop = new Desktop();
            currentDesktop.Name = "Desktop 1";
            currentConfig.selectedDesktop = 0;
            currentConfig.desktops.Add(currentDesktop);
            CloseCurrentDesktop(false); // Close current shown
            LoadData(); // Loads the data of the new note
            savable = true; // Makes everything savable again
            SaveConfig(); // Saves the new config
            LoadConfig(); // Loads the same configuration (The buttons needs to appear)
            SaveData(); // Saves the current clean data
        }

        private void deleteAllDesktopsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteAllDesktops();
        }

        private void importDesktopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ImportZippedData();
        }

        private void exportAllDesktopsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExportZippedData();
        }

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
                catch
                {
                    MessageBox.Show("It wasn't possible to connect with the server right now. Please, try again later.");
                }
            });
            MessageBox.Show("Checking if there's a new version available...");
        }

        private void AskToInstallNewVersion(string newVersion)
        {
            if (DialogResult.Yes == MessageBox.Show($"There's a new version available ({newVersion}). Do you want to install it?", "New version found", MessageBoxButtons.YesNo))
            {
                InstallNewVersion();

            }
        }
        private void InstallNewVersion()
        {
            dataSaved = false;
            configSaved = false;
            SaveConfig();
            SaveData();
            System.Diagnostics.Process.Start(Path.Combine(Environment.CurrentDirectory, "FastNotesUpdater.exe"));
            Application.Exit();
        }
    }
}
