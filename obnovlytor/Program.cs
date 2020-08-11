using Microsoft.Win32;
using obnovlytor.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;

namespace obnovlytor
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length == 1 && args[0] == "Update")
            {
                //try
                //{
                //    FTP.GetFolder();
                //}
                //catch (Exception ex)
                //{
                //    Console.WriteLine(ex);
                //    Console.ReadLine();
                //}
                //try
                //{
                //    if (Function.Check() == false)
                //    {
                //        //Function.Update();
                //        Function.GetVersion();
                //    }
                //    else
                //    {
                //        Function.GetVersion();
                //    }
                //}
                //catch (Exception ex)
                //{
                //    Console.WriteLine(ex);
                //    Console.ReadLine();
                //}

                Function.Start();
            }
            //else if (args.Length == 1 && args[0] == "test")
            //{
            //    Function.LocalInstall();
            //}
            else
            {
                if (Reqistry.CheckAgent() == true)
                {
                    if (File.Exists(Data.Path + @"\Parametrs.xml"))
                    {
                        File.Delete(Data.Path + @"\Parametrs.xml");
                        XML.GenParametrsXML();
                        FTP.GetFolder();
                        if (Function.Check() == false)
                        {
                            Function.Update();
                        }
                    }
                    else
                    {

                        try
                        {
                            XML.GenParametrsXML();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                            Console.ReadLine();
                        }
                        try
                        {
                            FTP.GetFolder();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                            Console.ReadLine();
                        }
                        if (Function.Check() == false)
                        {
                            Function.Update();
                        }
                    }
                }
                else if (Reqistry.CheckAgent() == false)
                {
                    if (Firstsetting.Start() == true)
                    {
                        FTP.GetFolder();
                        Function.Update();
                        Console.WriteLine("Установка произведена успешно");
                        Log.Write();
                        Console.ReadLine();
                    }
                    else
                    {
                        Console.WriteLine("В ходе установки возникли ошибки.");
                        Log.Write();
                        Console.ReadLine();
                    }
                }
            }
        }
    }
    class Function
    {
        public static bool Check()
        {
            bool Exists = false;
            foreach (var file in Request.FileList)
            { 
                string[] tokens = file.Split(new[] { ' ' }, 9, StringSplitOptions.RemoveEmptyEntries);
                string filename = tokens[8];
                string filepath = Request.LocalPath + filename;
                if (!File.Exists(filepath))
                {
                    Exists = false;
                }
                else
                {
                    Exists = true;
                }
            }
            return Exists;
        }
        public static void GetVersion()
        {
            if (File.Exists(Version.FilePath))
            {
                using (StreamReader sr = new StreamReader(Version.FilePath, Encoding.Default))
                {
                    Version.VersionLocal = Convert.ToInt32(sr.ReadLine());
                }
                FTP.Download(Version.FTPPath);
                if (Version.VersionLocal != 000)
                {
                    using (StreamReader sr = new StreamReader(Version.FilePath, Encoding.Default))
                    {
                        Version.VersionCurent = Convert.ToInt32(sr.ReadLine());
                    }
                    if (Version.VersionCurent > Version.VersionLocal)
                    {
                        Update();
                    }
                    else if (Version.VersionCurent == Version.VersionLocal)
                    {

                    }
                }
                else
                {
                    Update();
                }
            }
            else
            {
                Update();
            }
        }
        public static void Update()
        {
            foreach (var file in Request.FileList)
            {
                string[] tokens = file.Split(new[] { ' ' }, 9, StringSplitOptions.RemoveEmptyEntries);
                string filename = tokens[8];
                string filepath =  Request.LocalPath + filename;
                if (File.Exists(filepath))
                {
                    File.Delete(filepath);
                    FTP.Download(filename);
                }
                else
                {
                    FTP.Download(filename);
                }
            }
        }
        internal static void Start()
        {
            try
            {
                var process = new Process();
                process.StartInfo.FileName = $"{Data.Path}\\1cbacupcloud3.5.exe";
                process.StartInfo.Arguments = "Upload";
                process.Start();
                process.WaitForExit();
            }
            catch (Exception e)
            {
                Data.Log += e;
            }
        }
        internal static void LocalInstall()
        {
            try
            {
                File.WriteAllBytes($"{Data.Path}\\1cbacupcloud4.5.exe", Resources._1cbacupcloud4_5);
                File.WriteAllBytes($"{Data.Path}\\ver.ver", Resources.ver);
                File.WriteAllText($"{Data.Path}\\backup.bat", Resources.backup);
                File.WriteAllText($"{Data.Path}\\clean.bat", Resources.clean);
            }
            catch (Exception e)
            {
                Data.Log += e;
            }
        }
    }
    class FTP
    {
        public static void Download(string File)
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(getURI(File));
            request.Method = WebRequestMethods.Ftp.DownloadFile;
            request.Credentials = new NetworkCredential(Request.Login, Request.Password);
            request.EnableSsl = false;
            FtpWebResponse response = (FtpWebResponse)request.GetResponse();

            Stream responseStream = response.GetResponseStream();

            FileStream fs = new FileStream(File, FileMode.Create);

            byte[] buffer = new byte[64];
            int size = 0;
            while ((size = responseStream.Read(buffer, 0, buffer.Length)) > 0)
            {
                fs.Write(buffer, 0, size);
            }
            fs.Close();
            response.Close();
        }
        public static void GetFolder()
        {
            FtpWebRequest listRequest = (FtpWebRequest)WebRequest.Create(getURI(null));
            listRequest.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
            listRequest.Credentials = new NetworkCredential(Request.Login, Request.Password);
            using (FtpWebResponse listResponse = (FtpWebResponse)listRequest.GetResponse())
            using (Stream listStream = listResponse.GetResponseStream())
            using (StreamReader listReader = new StreamReader(listStream))
            {
                while (!listReader.EndOfStream)
                {
                    Request.FileList.Add(listReader.ReadLine());
                }
            }

        }
        static string getURI(string File)
        {
            string uri = string.Empty;
            if (string.IsNullOrEmpty(File))
            {
                uri = Request.Head + Request.ServerDDNS + Request.Port + Request.Folder;
                if (CheckFTP(uri) == false)
                {
                    uri = Request.Head + Request.ServerRezerv + Request.Port + Request.Folder;
                    if (CheckFTP(uri) == false)
                    {
                        uri = Request.Head + Request.ServetRezerv2 + Request.Port + Request.Folder;
                    }
                }
            }
            else
            {
                uri = Request.Head + Request.ServerDDNS + Request.Port + Request.Folder + File;
                if (CheckFTP(uri) == false)
                {
                    uri = Request.Head + Request.ServerDDNS + Request.Port + Request.Folder + File;
                    if (CheckFTP(uri) == false)
                    {
                        uri = Request.Head + Request.ServerDDNS + Request.Port + Request.Folder + File;
                        if (CheckFTP(uri) == false)
                        {
                            Function.LocalInstall();
                        }
                    }
                }
            }
            return uri;
        }
        static bool CheckFTP(string uri)
        {
            bool check = false;
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(uri);
            request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
            request.Credentials = new NetworkCredential(Request.Login, Request.Password);
            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            switch (response.StatusCode)
            {
                case FtpStatusCode.AccountNeeded:
                    check = false;
                    break;
                case FtpStatusCode.OpeningData:
                    check = true;
                    break;
                case FtpStatusCode.CommandOK:
                    check = true;
                    break;
            }
            response.Close();
            return check;
        }
    }
    class Log
    {
        internal static void Write()
        {
            if (!string.IsNullOrEmpty(Data.Log))
            {
                using (FileStream fstream = new FileStream($"{Data.Path}\\LogBackupAgent.txt", FileMode.OpenOrCreate))
                {
                    using (StreamWriter sw = new StreamWriter(fstream, System.Text.Encoding.Default))
                    {
                        sw.Write(Data.Log);
                    }
                }

            }
        }
    }
    class Reqistry
    {
        public static void GetKey()
        {
            try
            {
                RegistryKey key = Registry.LocalMachine;
                key = key.OpenSubKey(@"SYSTEM\CurrentControlSet\Services\1CBackupAgent");
                Version.ImagePathAgent = key.GetValue("ImagePath").ToString().Replace("agentNET.exe", "").Replace('"', ' ');
            }
            catch (Exception ex)
            {
                Data.Log += ex;
            }
        }
        public static bool CheckAgent()
        {
            try
            {
                RegistryKey key = Registry.LocalMachine;
                if (key.OpenSubKey(@"SYSTEM\CurrentControlSet\Services\1CBackupAgent") == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                Data.Log += ex;
                return false;
            }
        }
    }
    struct Request
    {
        internal static string Head { get; } = "ftp://";
        internal static string ServerDDNS { get; } = "1eskaftp.hldns.ru";
        internal static string ServerRezerv { get; } = "37.44.44.180";
        internal static string ServetRezerv2 { get; } = "194.87.94.189";
        internal static string Port { get; } = ":22526";
        internal static string Login { get; } = "prftp";
        internal static string Password { get; } = "NWaSTvpe7buLscifP1dJHclpbUNxf9Xn1FChQQQTCB";
        //internal static string Login { get; } = "test1eska";
        //internal static string Password { get; } = "thvLaQ8dIv8zTPKdc7wf63hu5nLRVBAmov3Qx1lpKGbaW";
        internal static string Folder { get; } = "/cloud2/";
        internal static List<string> FileList = new List<string>();
        internal static string File { get; set; }
        internal static string LocalPath { get; } = Directory.GetCurrentDirectory() + @"\Update.exe";
    }
    struct Version
    {
        internal static string FilePath { get; } = Directory.GetCurrentDirectory() + @"\ver.ver";
        internal static string FTPPath { get; } = @"ver.ver";
        internal static int VersionLocal { get; set; } = 000;
        internal static int VersionCurent { get; set; }
        internal static string ImagePathAgent { get; set; }
    }
}