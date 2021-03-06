﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ServerLogger
{
    public static class Checker
    {
        #region Private Members

        static string srv = "http://fastnotes.tk/";

        #endregion

        #region Public Members

        public const string exeHide = ".hide";

        public class DownloaderEventHandler
        {
            public event Action OnBeforeDownload;
            public event DownloadingArgs OnDownloading;
            public event Action OnAfterDownload;
            public delegate void DownloadingArgs(long prevBytesDownloaded, long bytesDownloaded, long totalBytes, long timeElapsed);
            public void ExecOnBeforeDownload()
            {
                OnBeforeDownload?.Invoke();
            }
            public void ExecOnDownloading(long prevBytesDownloaded, long bytesDownloaded, long totalBytes, long timeElapsed)
            {
                OnDownloading?.Invoke(prevBytesDownloaded, bytesDownloaded, totalBytes, timeElapsed);
            }
            public void ExecOnAfterDownload()
            {
                OnAfterDownload?.Invoke();
            }
        }

        public class ProgramInfo
        {
            public List<FileInfo> Files;
            public ProgramInfo()
            {

            }
            public static ProgramInfo Get(string rootDirectory)
            {
                ProgramInfo pi = new ProgramInfo();
                List<FileInfo> fil = new List<FileInfo>();
                foreach (var file in Directory.GetFiles(rootDirectory))
                {
                    FileInfo fi = new FileInfo();
                    System.IO.FileInfo sifi = new System.IO.FileInfo(file);
                    fi.Size = sifi.Length;
                    fi.Name = sifi.FullName.Replace(rootDirectory, "").TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                    fi.CheckSum = 0;
                    fi.ShiftedCheckSum = 0;
                    using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read))
                    {
                        for (long i = fs.Length - 1; i >= 0; i--)
                        {
                            long byteRead = fs.ReadByte();
                            fi.CheckSum += byteRead << (8 * (int)((i + 7) % 8));
                            fi.ShiftedCheckSum += byteRead << (8 * (int)((i + 3) % 8));
                        }
                    }
                    fil.Add(fi);
                }
                pi.Files = fil;
                return pi;
            }
        }

        public class FileInfo
        {
            public string Name;
            public long CheckSum;
            public long ShiftedCheckSum;
            public long Size;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// When an update is being performed this will return a list of files to download first, 
        /// only when the updater needs to be downloaded
        /// </summary>
        /// <returns></returns>
        public static string[] GetUpdaterDependencies()
        {
            HttpWebRequest rquest = ((HttpWebRequest)WebRequest.Create($"{srv}updaterdependencies/files"));
            string[] dependencies = null;
            using (WebResponse wr = rquest.GetResponse())
            {
                using (Stream bs = wr.GetResponseStream())
                {
                    using (StreamReader sr = new StreamReader(bs))
                    {
                        string read = sr.ReadToEnd();
                        if(read.Length != 0)
                            dependencies = read.Split('\n');
                    }
                }
            }
            return dependencies;
        }

        /// <summary>
        /// Returns true if the version given on parameters is the newest
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        public static bool IsNewestVersion(string version)
        {
            string str;
            return IsNewestVersion(version, out str);
        }

        /// <summary>
        /// Returns true if the version given on parameters is the newest, also gives the newestversion found
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        public static bool IsNewestVersion(string version, out string newestVersion)
        {
            HttpWebRequest rquest = ((HttpWebRequest)WebRequest.Create($"{srv}version/newest"));
            newestVersion = null;
            using (WebResponse wr = rquest.GetResponse())
            {
                using (Stream bs = wr.GetResponseStream())
                {
                    using (StreamReader sr = new StreamReader(bs))
                    {
                        newestVersion = sr.ReadLine();
                    }
                }
            }
            if(!Regex.Match(version, "^[0-9]+.[0-9]+.[0-9]+.[0-9]+$").Success)
            { // Can be an error with the server
                newestVersion = version;
                return true;
            }
            if (newestVersion == version)
            {
                return true; // Newest
            }
            return false;
        }

        /// <summary>
        /// Gets a file that is on the server and parses it to a class, that class (ProgramInfo) contains the
        /// information of each file that is in the server's folder, this information it's supposed to be used to
        /// compare it to the information of the current local folder, that way it can be known what files to download
        /// </summary>
        /// <param name="statusDescription"></param>
        /// <returns></returns>
        public static ProgramInfo GetProgramInfo(out string statusDescription)
        {
            HttpWebRequest request = ((HttpWebRequest)WebRequest.Create($"{srv}version/info"));
            ProgramInfo pi;
            using (var response = (HttpWebResponse)request.GetResponse())
            {
                XmlSerializer xmls = new XmlSerializer(typeof(ProgramInfo));
                pi = (ProgramInfo)xmls.Deserialize(response.GetResponseStream());
                statusDescription = response.StatusDescription;
            }
            return pi;
        }

        /// <summary>
        /// Gets a file that is on the server and parses it to a class, that class (ProgramInfo) contains the
        /// information of each file that is in the server's folder, this information it's supposed to be used to
        /// compare it to the information of the current local folder, that way it can be known what files to download
        /// </summary>
        public static ProgramInfo GetProgramInfo()
        {
            string s;
            return GetProgramInfo(out s);
        }

        /// <summary>
        /// Gets the size of a file in the server
        /// </summary>
        /// <param name="filepath">Path of the file in the server (it will be combined with the app folder)</param>
        /// <returns></returns>
        public static long GetFileSizeOnServer(string filepath)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"{srv}app/?filesize={filepath}");
            long length = 0;
            using (var response = (HttpWebResponse)request.GetResponse())
            {
                using (var s = new StreamReader(response.GetResponseStream()))
                {
                    length = long.Parse(s.ReadToEnd());
                }
            }
            return length;
        }

        /// <summary>
        /// Downloads a file from the server.
        /// </summary>
        /// <param name="filepath">Path of the file in the server (it will be combined with the app folder)</param>
        /// <param name="targetPath">Path to local storage</param>
        /// <param name="deh">Object that will contain information of the current download, it will be called msPerInfo milliseconds</param>
        /// <param name="msPerInfo">Milliseconds between calls to the 'deh' object</param>
        /// <param name="buffer">Amount of bytes transferred per iteration</param>
        /// <returns></returns>
        public static string DownloadAppFile(string filepath, string targetPath, DownloaderEventHandler deh = null, int msPerInfo = 1000, int buffer = 4096)
        {
            if (targetPath.EndsWith(".exe")) filepath += exeHide;
            long length = GetFileSizeOnServer(filepath);
            HttpWebRequest request = ((HttpWebRequest)WebRequest.Create($"{srv}app/{filepath}"));
            if (deh != null)
            {
                deh.ExecOnBeforeDownload();
            }

            CancellationTokenSource tokenSource = new CancellationTokenSource();
            CancellationToken token = tokenSource.Token;
            Task downloaderInfo = null;
            using (HttpWebResponse fwr = (HttpWebResponse)request.GetResponse())
            {
                using (var s = fwr.GetResponseStream())
                {
                    using (var br = new BinaryReader(s))
                    {
                        using (var bw = new BinaryWriter(new FileStream(targetPath, FileMode.Create, FileAccess.Write)))
                        {
                            long idx = 0;
                            long previdx = 0;
                            if (deh != null)
                            {
                                downloaderInfo = Task.Run(() =>
                                {
                                    long msPassed = 0;
                                    while (idx < length)
                                    {
                                        deh.ExecOnDownloading(previdx, idx, length, msPassed);
                                        previdx = idx;
                                        Thread.Sleep(msPerInfo);
                                        msPassed += msPerInfo;
                                        if (token.IsCancellationRequested) return;
                                    }
                                }, token);
                            }
                            for (idx = 0; idx < length;)
                            {
                                byte[] readBytes = br.ReadBytes(buffer);
                                bw.Write(readBytes);
                                idx += readBytes.Length;
                            }
                            if (downloaderInfo != null)
                            {
                                try
                                {
                                    tokenSource.Cancel();
                                    downloaderInfo.Wait();
                                }
                                catch
                                {

                                }
                            }
                            if (deh != null) deh.ExecOnAfterDownload();
                        }
                    }
                }
                return fwr.StatusDescription;
            }
        }
       
        #endregion
    }
}
