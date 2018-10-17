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

namespace FastNotes
{
	public partial class StartupForm : Form
	{
		public static StartupForm Instance;
		public static bool saved = true;
		public PrivateFontCollection customFonts = new PrivateFontCollection();
		public FontFamily novaSquare;
		static BinaryFormatter bf = new BinaryFormatter();

		static string extPathv1_0_1_0 = @"FastNotes\FastNotes\1.0.1.0";
		static string extPathv1_0_1_1 = @"Jeweells\FastNotes\1.0.1.1";
		public static string configPathv1_0_0_0 = Environment.CurrentDirectory + "//config.fn";
		public static string configPathv1_0_1_0 = Application.UserAppDataPath.Substring(0, Application.UserAppDataPath.Length - extPathv1_0_1_1.Length) 
			+ extPathv1_0_1_0 + "//config.fn"; // Previous path (trying to move data to the new path)
		public static string configPath = Application.UserAppDataPath + "//config.fn";

		static string dataFilePathv1_0_0_0 = Environment.CurrentDirectory + "//data.fn";
		static string dataFilePathv1_0_1_0 = Application.UserAppDataPath.Substring(0, Application.UserAppDataPath.Length - extPathv1_0_1_1.Length)
			+ extPathv1_0_1_0 +  "//data.fn"; // Previous path (Trying to move data to the new path)

		static string dataFilePath = Application.UserAppDataPath + "//data.fn";


		public Config currentConfig;
		
		public StartupForm()
		{
			Instance = this;
			XmlSerializer xs = new XmlSerializer(typeof(Config));
			if(!File.Exists(configPath) && File.Exists(configPathv1_0_1_0))
			{
				File.Copy(configPathv1_0_1_0, configPath);
			}
			else if (!File.Exists(configPath) && File.Exists(configPathv1_0_0_0)) // Translate data
			{
				File.Copy(configPathv1_0_0_0, configPath); // Copies the data if exists only
			}

			if (File.Exists(configPath))
			{
				using (StreamReader sr = new StreamReader(configPath))
				{
					currentConfig = (Config) xs.Deserialize(sr);
				}
			}
			else
			{
				currentConfig = new Config();
				using (FileStream f = File.Create(configPath))
				{
					xs.Serialize(f, currentConfig);
				}
				Program.RunAtStartup(currentConfig.runAtStartup);
			}
			byte[] novaSquareData = Properties.Resources.NovaSquare;
			IntPtr data = Marshal.AllocCoTaskMem(novaSquareData.Length);
			Marshal.Copy(novaSquareData, 0, data, novaSquareData.Length);
			customFonts.AddMemoryFont(data, novaSquareData.Length);
			novaSquare = customFonts.Families[0];
			InitializeComponent();

			runAtStartupContextMenuStrip.Checked = currentConfig.runAtStartup;
			LoadData(); // Initializes if the data doesn't exist
		}
		public void LoadData()
		{
			if (!File.Exists(dataFilePath) && File.Exists(dataFilePathv1_0_1_0))
			{
				File.Copy(dataFilePathv1_0_1_0, dataFilePath);
			}
			else if(!File.Exists(dataFilePath) && File.Exists(dataFilePathv1_0_0_0))
			{
				File.Copy(dataFilePathv1_0_0_0, dataFilePath);
			}
			if(File.Exists(dataFilePath))
			{
				try
				{
					using (FileStream file = new FileStream(dataFilePath, FileMode.Open))
					{
						foreach (NoteData note in (NoteData[])bf.Deserialize(file))
						{
							NoteForm f = (NoteForm)Program.CreateNote();
							f.Show(this);
							f.Data = note;
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
					MessageBox.Show("Something went wrong");
					Form f = Program.CreateNote();
					f.Show(this);
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

		public static void SaveData()
		{
			NoteData[] currentData = new NoteData[Program.availableForms.Count];
			int i = 0;
			foreach (NoteForm note in Program.availableForms)
			{
				currentData[i++] = note.Data;
			}
			using (FileStream file = File.Create(dataFilePath))
			{
				bf.Serialize(file, currentData);
				saved = true;
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
			SaveData();
		}

		private void timer1_Tick(object sender, EventArgs e)
		{
			if(!saved) SaveData();
		}

		private void contextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
		{
			
		}


		private void runAtStartupContextMenuStrip_Click(object sender, EventArgs e)
		{
			ToolStripMenuItem tsmi = ((ToolStripMenuItem)sender);
			Program.RunAtStartup(tsmi.Checked = !tsmi.Checked);
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{

			Application.Exit();
		}
	}
}
