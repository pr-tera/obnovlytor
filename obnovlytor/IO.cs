using System;
//using System.Dynamic;
using System.IO;

namespace obnovlytor
{
    class IO
    {
        internal static string CreatFolder(string dir)
        {
            if (CheckDirExists(dir) == true)
            {
                if (DelDir(dir) == true)
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo(dir);
                    directoryInfo.Create();
                    Logs.Log += $"{DateTime.Now} Директория {directoryInfo.Name} создана.\n";
                    return dir;
                }
                else
                {
                    Logs.Log += "Ошибка при удалении файла\n";
                    return string.Empty;
                }
            }
            else
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(dir);
                directoryInfo.Create();
                Logs.Log += $"{DateTime.Now} Директория {directoryInfo.Name} создана.\n";
                return dir;
            }
        }
        internal static bool CheckDirExists(string dir)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(dir);
            if (directoryInfo.Exists)
            {
                Logs.Log += $"{DateTime.Now} Директория {directoryInfo.Name} существует.\n";
                return true;
            }
            else
            {
                Logs.Log += $"{DateTime.Now} Директория {directoryInfo.Name} не существует.\n";
                return false;
            }
        }
        internal static bool DelDir(string dir)
        {
            try
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(dir);
                directoryInfo.Delete(true);
                Logs.Log += $"{DateTime.Now} Директория {directoryInfo.Name} удалена!\n";
                return true;
            }
            catch (Exception ex)
            {
                Logs.Log += $"{DateTime.Now} {ex}\n";
                return false;
            }
        }
        internal static bool CheckFileExists(string File)
        {
            FileInfo fileInfo = new FileInfo(File);
            try
            {
                if (fileInfo.Exists)
                {
                    Logs.Log += $"{DateTime.Now} Файл {fileInfo.Name} существует!\n";
                    return true;
                }
                else
                {
                    Logs.Log += $"{DateTime.Now} Файл {fileInfo.Name} не существует!\n";
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
        internal static bool DelFile(string File)
        {
            FileInfo fileInfo = new FileInfo(File);
            if (fileInfo.Exists)
            {
                try
                {
                    fileInfo.Delete();
                    Logs.Log += $"{DateTime.Now} Файл {fileInfo.Name} удален!\n";
                    return true;
                }
                catch (Exception ex)
                {
                    Logs.Log += $"{DateTime.Now} {ex}\n";
                    return false;
                }
            }
            else
            {
                Logs.Log += $"{DateTime.Now} Файл {fileInfo.Name} не существует!\n";
                return false;
            }
        }
    }
    struct IOs
    {
        internal static string Parametrs { get; } = "Parametrs.xml";
        internal static string RootDir { get; } = Directory.GetCurrentDirectory() + @"\";
        internal static string backupagentTemp { get; } = "backupagentTemp";
        internal static string ImagePathAgent { get; set; }
        internal static string BackupPath { get; } = @"\backup.path";
    }
}
