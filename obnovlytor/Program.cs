using System;
using System.Diagnostics;
using System.IO;
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
                if (FTP.GetListFolber() == true)
                {
                    if (Version.GetVersion() == true)
                    {
                        if (FTP.GetListFolber() == true)
                        {
                            if (FTP.Download() == true)
                            {
                                Logs.Log += $"\n{DateTime.Now} Обновление прошло успешно.\n";
                                Start();
                                Log.Write();
                            }
                        }
                        else
                        {
                            Logs.Log += $"\n{DateTime.Now} При обновлении не удалось получить список файлов с ftp.\n";
                            Start();
                            Log.Write();
                        }
                    }
                    else
                    {
                        Logs.Log += $"\n{DateTime.Now} При получении версии произошла ошибка.\n";
                        Start();
                        Log.Write();
                    }
                }
                else
                {
                    Logs.Log += $"\n{DateTime.Now} Не удалось проверить наличие обновлений.\n";
                    Start();
                    Log.Write();
                }
            }
            else
            {
                if (FTP.GetListFolber() == true)
                {
                    if (FTP.Download() == true)
                    {
                        if (IO.CheckFileExists(IOs.RootDir + IOs.Parametrs) == false)
                        {
                            XML.GenParametrsXML();
                            if (Reqistry.CheckAgent() == false)
                            {
                                if (Install.GetAgent() == true)
                                {
                                    if (Reqistry.CheckAgent() == true)
                                    {
                                        if (Service.Status() == true)
                                        {
                                            if (SetPath() == true)
                                            {
                                                Console.WriteLine("Установка прошла успешно!");
                                                Log.Write();
                                                Console.ReadLine();
                                            }
                                            else
                                            {
                                                Console.WriteLine("В ходе установки возникли проблемы, см. лог.");
                                                Log.Write();
                                                Console.ReadLine();
                                            }
                                        }
                                        else
                                        {
                                            Console.WriteLine("Служба агента не запущена!");
                                            Log.Write();
                                            Console.ReadLine();
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("Агент не установлен!");
                                        Log.Write();
                                        Console.ReadLine();
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Агент не установлен!");
                                    Log.Write();
                                    Console.ReadLine();
                                }
                            }
                            else
                            {
                                if (IO.CheckFileExists(IOs.RootDir + IOs.Parametrs) == true)
                                {
                                    Console.WriteLine("Обновление скриптов и активация агента успешно завершены!");
                                    Log.Write();
                                    Console.ReadLine();
                                }
                                else
                                {
                                    Console.WriteLine("При активации агента произошла ошибка!");
                                    Log.Write();
                                    Console.ReadLine();
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("Обновление прошло успешно!");
                            Log.Write();
                            Console.ReadLine();
                        }
                    }
                    else
                    {
                        if (IO.CheckFileExists(IOs.RootDir + IOs.Parametrs) == false)
                        {
                            XML.GenParametrsXML();
                            LocalInstall.Install();
                            if (Reqistry.CheckAgent() == false)
                            {
                                if (Install.GetAgent() == true)
                                {
                                    if (Reqistry.CheckAgent() == true)
                                    {
                                        if (Service.Status() == true)
                                        {
                                            if (SetPath() == true)
                                            {
                                                Console.WriteLine("Установка прошла успешно!");
                                                Log.Write();
                                                Console.ReadLine();
                                            }
                                            else
                                            {
                                                Console.WriteLine("В ходе установки возникли проблемы, см. лог.");
                                                Log.Write();
                                                Console.ReadLine();
                                            }
                                        }
                                        else
                                        {
                                            Console.WriteLine("Служба агента не запущена!");
                                            Log.Write();
                                            Console.ReadLine();
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("Агент не установлен!");
                                        Log.Write();
                                        Console.ReadLine();
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Агент не установлен!");
                                    Log.Write();
                                    Console.ReadLine();
                                }
                            }
                            else
                            {
                                if (IO.CheckFileExists(IOs.RootDir + IOs.Parametrs) == true)
                                {
                                    Console.WriteLine("Обновление скриптов и активация агента успешно завершены!");
                                    Log.Write();
                                    Console.ReadLine();
                                }
                                else
                                {
                                    Console.WriteLine("При активации агента произошла ошибка!");
                                    Log.Write();
                                    Console.ReadLine();
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (IO.CheckFileExists(IOs.RootDir + IOs.Parametrs) == false)
                    {
                        XML.GenParametrsXML();
                        LocalInstall.Install();
                        if (Reqistry.CheckAgent() == false)
                        {
                            if (Install.GetAgent() == true)
                            {
                                if (Reqistry.CheckAgent() == true)
                                {
                                    if (Service.Status() == true)
                                    {
                                        if (SetPath() == true)
                                        {
                                            Console.WriteLine("Установка прошла успешно!");
                                            Log.Write();
                                            Console.ReadLine();
                                        }
                                        else
                                        {
                                            Console.WriteLine("В ходе установки возникли проблемы, см. лог.");
                                            Log.Write();
                                            Console.ReadLine();
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("Служба агента не запущена!");
                                        Log.Write();
                                        Console.ReadLine();
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Агент не установлен!");
                                    Log.Write();
                                    Console.ReadLine();
                                }
                            }
                            else
                            {
                                Console.WriteLine("Агент не установлен!");
                                Log.Write();
                                Console.ReadLine();
                            }
                        }
                        else
                        {
                            if (IO.CheckFileExists(IOs.RootDir + IOs.Parametrs) == true)
                            {
                                Console.WriteLine("Обновление скриптов и активация агента успешно завершены!");
                                Log.Write();
                                Console.ReadLine();
                            }
                            else
                            {
                                Console.WriteLine("При активации агента произошла ошибка!");
                                Log.Write();
                                Console.ReadLine();
                            }
                        }
                    }
                }
            }
        }
        internal static void Start()
        {
            try
            {
                var process = new Process();
                //process.StartInfo.FileName = $"{IOs.RootDir}\\1cbacupcloud3.5.exe";
                process.StartInfo.FileName = $"{IOs.RootDir}\\1cbacupcloud4.5.exe";
                process.StartInfo.Arguments = "Upload";
                process.Start();
                process.WaitForExit();
            }
            catch (Exception ex)
            {
                Logs.Log += $"\n{DateTime.Now} {ex}.\n";
            }
        }
        static bool SetPath()
        {
            bool _check;
            Reqistry.GetKey();
            if (IO.CheckFileExists(IOs.ImagePathAgent + IOs.BackupPath) == true)
            {
                if (IO.DelFile(IOs.ImagePathAgent + IOs.BackupPath) == true)
                {
                    try
                    {
                        using (FileStream fstream = new FileStream(IOs.ImagePathAgent + @"\backup.path", FileMode.OpenOrCreate))
                        {
                            using (StreamWriter sw = new StreamWriter(fstream, Encoding.Default))
                            {
                                sw.Write(IOs.RootDir);
                            }
                        }
                    }
                    catch
                    {

                    }
                }
                else
                {
                    Logs.Log += $"\n{DateTime.Now} Не удалось записать путь до скрипта резервного копирования.\n";
                }
            }
            else
            {
                try
                {
                    using (FileStream fstream = new FileStream(IOs.ImagePathAgent + @"\backup.path", FileMode.OpenOrCreate))
                    {
                        using (StreamWriter sw = new StreamWriter(fstream, Encoding.Default))
                        {
                            sw.Write(IOs.RootDir);
                        }
                    }
                }
                catch
                {

                }
            }
            if (IO.CheckFileExists(IOs.ImagePathAgent + IOs.BackupPath) == true)
            {
                _check = true;
            }
            else
            {
                _check = false;
            }
            return _check;
        }
    }
}