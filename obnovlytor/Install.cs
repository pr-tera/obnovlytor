using System;
using System.Diagnostics;
using System.Net;

namespace obnovlytor
{
    class Install
    {
        internal static bool GetAgent()
        {
            if (DownloadAgetn() == true)
            {
                if (InstallAgent() == true)
                {
                    IO.DelDir(IOs.RootDir + IOs.backupagentTemp);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        private static string Temp()
        {
            if (IO.CheckDirExists(IOs.RootDir + IOs.backupagentTemp) == false)
            {
                try
                {
                    IO.CreatFolder(IOs.RootDir + IOs.backupagentTemp);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    Logs.Log += $"{DateTime.Now} {e} \n";
                }
            }
            return IOs.RootDir + IOs.backupagentTemp;
        }
        private static bool DownloadAgetn()
        {
            string uri = "http://agentupdprod.hb.bizmrg.com/installAgent.exe";
            try
            {
                using (var client = new WebClient())
                {
                    client.DownloadFile(uri, $"{Temp()}\\installagent.exe");
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine($"При загрузке произошла ошибка:\n{e}");
                Logs.Log += $"{DateTime.Now} {e} \n";
                return false;
            }
        }
        private static bool InstallAgent()
        {
            try
            {
                var process = new Process();
                process.StartInfo.FileName = $"{Temp()}\\installagent.exe";
                process.Start();
                process.WaitForExit();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Logs.Log += $"{DateTime.Now} {e} \n";
                return false;
            }
        }
    }
}
