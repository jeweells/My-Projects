using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FastNotes
{
    public class Desktop
    {
        public string Name { get; set; }
        public string GetEscapedName()
        {
            return GetEscapedName(Name);
        }
        public static string GetEscapedName(string name)
        {
            string tmp = "";
            for (int i = 0; i < name.Length; i++)
            {
                int utf32 = Char.ConvertToUtf32(name, i);
                if (utf32 == 92 ||
                    utf32 == 47 ||
                    utf32 == 58 ||
                    utf32 == 42 ||
                    utf32 == 63 ||
                    utf32 == 34 ||
                    utf32 == 60 ||
                    utf32 == 62 ||
                    utf32 == 124 ||
                    utf32 == 85) // Avoid errors on creating a folder (prohibben characters)
                                 // 85 is just to escape the escape character which will be U
                {
                    /* That secuence is
                    \...92
                    /...47
                    :...58
                    *...42
                    ?...63
                    "...34
                    <...60
                    >...62
                    |...124
                    U...85
                    */
                    tmp += "U" + utf32 + "U";
                }
                else { tmp += name[i]; }

            }
            return tmp;
        }
        public string DataFilePath {
            get {
                return FolderPath + "//data.fn";
            }
        }


        public string FolderPath
        {
            get
            {
                return Application.UserAppDataPath + "//" + GetEscapedName(Name);
            }
        }

        public bool MakeDir()
        {
            try
            {
                Directory.CreateDirectory(FolderPath);
            }
            catch(Exception e)
            {
                MessageBox.Show("There was an error while creating the desktop data directory");
                Program.WriteExceptionToLog(e);
                return false;
            }
            return true;
        }
        public bool Rename(string newName)
        {
            try
            {
                Directory.Move(FolderPath, Application.UserAppDataPath + "//" + GetEscapedName(newName));
                Name = newName;
            }
            catch (Exception e)
            {
                MessageBox.Show("There was an error while renaming the desktop data directory");
                Program.WriteExceptionToLog(e);
                return false;
            }
            return true;

        }
        public bool Delete()
        {
            try
            {
                Directory.Delete(FolderPath,true);
            }
            catch (Exception e)
            {
                MessageBox.Show("There was an error while deleting the desktop data");
                Program.WriteExceptionToLog(e);
                return false;
            }
            return true;
        }

        public static string GetNameFromEscaped(string v)
        {
            // We assume the parameter v was escaped propperly
            string tmp = "";
            for (int i = 0; i < v.Length; i++)
            {
                if (v[i] == 'U')
                {
                    string normal = "U";
                    int utf = 0;
                    while (++i < v.Length && char.IsNumber(v[i]))
                    {
                        normal += v[i];
                        utf = utf * 10 + Int32.Parse(v[i] + "");
                    }
                    if (i >= v.Length || v[i] != 'U')
                    {
                        tmp += normal;
                    }
                    else
                    {
                        tmp += char.ConvertFromUtf32(utf);
                        continue;
                    }
                }
                tmp += v[i];
            }

            return tmp;
        }
    }
}
