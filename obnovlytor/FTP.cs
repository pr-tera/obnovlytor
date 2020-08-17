using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace obnovlytor
{
    class FTP
    {
        internal static bool Download()
        {
            bool check = false;
            if (!string.IsNullOrEmpty(_checkFTPServer()))
            {
                foreach (var file in FTPuri.FileList)
                {
                    string[] tokens = file.Split(new[] { ' ' }, 9, StringSplitOptions.RemoveEmptyEntries);
                    string filename = tokens[8];
                    string filepath = IOs.RootDir + filename;
                    if (IO.CheckFileExists(filepath) == true)
                    {
                        if (IO.DelFile(filepath) == true)
                        {
                            _download(filename);
                            Logs.Log += $"{DateTime.Now} Началась загрузка файла {filename}\n";
                            if (IO.CheckFileExists(filepath) == true)
                            {
                                Logs.Log += $"{DateTime.Now} Файл {filename} успешно загружен\n";
                                check = true;
                            }
                            else
                            {
                                Logs.Log += $"{DateTime.Now} Не удалось загрузить файл {filename}\n";
                                check = false;
                            }
                        }
                        else
                        {
                            Logs.Log += $"{DateTime.Now} Не удалось начать загрузку {filename}\n";
                            check = false;
                        }
                    }
                    else
                    {
                        _download(filename);
                        Logs.Log += $"{DateTime.Now} Началась загрузка файла {filename}\n";
                        if (IO.CheckFileExists(filepath) == true)
                        {
                            Logs.Log += $"{DateTime.Now} Файл {filename} успешно загружен\n";
                            check = true;
                        }
                        else
                        {
                            Logs.Log += $"{DateTime.Now} Не удалось загрузить файл {filename}\n";
                            check = false;
                        }
                    }
                }
            }
            else
            {
                Logs.Log += $"\n{DateTime.Now} FT1004\n";
                check = false;
            }
            return check;
        }
        protected static bool _download(string File)
        {
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(_uri(File, _checkFTPServer()));
                request.Method = WebRequestMethods.Ftp.DownloadFile;
                request.Credentials = new NetworkCredential(FTPuri.Login, FTPuri.Password);
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
                return true;
            }
            catch (Exception ex)
            {
                Logs.Log += $"{DateTime.Now} {ex}\n";
                return false;
            }
        }
        internal static bool GetListFolber()
        {
            if (string.IsNullOrEmpty(_checkFTPServer()))
            {
                return false;
            }
            else
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(_uri(null, _checkFTPServer()));
                request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                request.Credentials = new NetworkCredential(FTPuri.Login, FTPuri.Password);
                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                using (Stream listStream = response.GetResponseStream())
                using (StreamReader listReader = new StreamReader(listStream))
                {
                    while (!listReader.EndOfStream)
                    {
                        FTPuri.FileList.Add(listReader.ReadLine());
                    }
                }
                return true;
            }
        }
        protected static string _uri(string File, string Server)
        {
            if (string.IsNullOrEmpty(File))
            {
                string _uri = FTPuri.Head + Server + FTPuri.Port + FTPuri.Folder;
                return _uri;
            }
            else
            {
                string _uri = FTPuri.Head + Server + FTPuri.Port + FTPuri.Folder + File;
                return _uri;
            }
        }
        protected static bool _checkFTP(string Server)
        {
            bool _check = false;
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(_uri(null, Server));
            request.Method = WebRequestMethods.Ftp.ListDirectory;
            request.Credentials = new NetworkCredential(FTPuri.Login, FTPuri.Password);
            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            switch (response.StatusCode)
            {
                case FtpStatusCode.AccountNeeded:
                    _check = false;
                    break;
                case FtpStatusCode.OpeningData:
                    _check = true;
                    break;
                case FtpStatusCode.CommandOK:
                    _check = true;
                    break;
            }
            response.Close();
            return _check;
        }
        protected static string _checkFTPServer()
        {
            string _server = string.Empty;
            string[] server = { FTPuri.ServerDDNS, FTPuri.ServerRezerv, FTPuri.ServetRezerv2 };
            for (int i = 0; i < 2; i++)
            {
                if (_checkFTP(server[i]) == true)
                {
                    _server = server[i]; 
                    break;
                }
                else
                {
                    switch (i)
                    {
                        case 0:
                            Logs.Log += $"\n{DateTime.Now} FT1001";
                            break;
                        case 1:
                            Logs.Log += $"\n{DateTime.Now} FT1002";
                            break;
                        case 2:
                            Logs.Log += $"\n{DateTime.Now} FT1003";
                            break;
                    }
                }
            }
            return _server;
        }
    }
    struct FTPuri
    {
        internal static string Head { get; } = "ftp://";
        internal static string ServerDDNS { get; } = "1eskaftp.hldns.ru";
        internal static string ServerRezerv { get; } = "37.44.44.180";
        internal static string ServetRezerv2 { get; } = "194.87.94.189";
        internal static string Port { get; } = ":22526";
        internal static string Login { get; } = "prftp";
        internal static string Password { get; } = "NWaSTvpe7buLscifP1dJHclpbUNxf9Xn1FChQQQTCB";
        //internal static string Folder { get; } = "/cloud2/";
        internal static string Folder { get; } = "/cloud2/4.5/";
        internal static List<string> FileList = new List<string>();
        //internal static string File { get; set; }
    }
}
