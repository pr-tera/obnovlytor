using System;
using System.IO;
using System.Text;

namespace obnovlytor
{
    class Version
    {
        internal static bool GetVersion()
        {
            bool check = false;
            if (FTP.GetListFolber() == true)
            {
                if (IO.CheckFileExists(IOs.RootDir + Versions.FilePath) == true)
                {
                    using (StreamReader sr = new StreamReader(Versions.FilePath, Encoding.Default))
                    {
                        Versions.VersionLocal = Convert.ToInt32(sr.ReadLine());
                    }
                    if (Versions.VersionLocal == 000)
                    {
                        Logs.Log += $"\n{DateTime.Now} Не удалось прочитать файл с версией!.\n";
                        check = true;
                    }
                    else
                    {
                        FTPuri.FileList.Clear();
                        FTPuri.FileList.Add($"-rw-r--r--    1 1001     1001       352768 Aug 17 06:07 {Versions.FilePath}");
                        if (IO.DelFile(IOs.RootDir + Versions.FilePath) == true)
                        {
                            if (FTP.Download() == true)
                            {
                                using (StreamReader sr = new StreamReader(Versions.FilePath, Encoding.Default))
                                {
                                    Versions.VersionCurent = Convert.ToInt32(sr.ReadLine());
                                }
                                if (Versions.VersionCurent == 000)
                                {
                                    Logs.Log += $"\n{DateTime.Now} Не удалось прочитать файл с версией!.\n";
                                    check = true;
                                }
                                else
                                {
                                    if (Versions.VersionCurent > Versions.VersionLocal)
                                    {
                                        check = true;
                                    }
                                    else if (Versions.VersionCurent == Versions.VersionLocal)
                                    {
                                        check = false;
                                    }
                                }
                            }
                            else
                            {
                                Logs.Log += $"\n{DateTime.Now} Не удалось получить новый файл с версией!.\n";
                                check = true;
                            }
                        }
                        else
                        {
                            Logs.Log += $"\n{DateTime.Now} Не удалось получить новый файл с версией!.\n";
                            check = true;
                        }
                    }
                }
                else
                {
                    Logs.Log += $"\n{DateTime.Now} Файл с версией не существует!.\n";
                    check = true;
                }
            }
            else
            {
                check = false;
            }
            return check;
        }
    }
    struct Versions
    {
        internal static string FilePath { get; } = "ver.ver";
        internal static int VersionLocal { get; set; } = 000;
        internal static int VersionCurent { get; set; } = 000;
    }
}
