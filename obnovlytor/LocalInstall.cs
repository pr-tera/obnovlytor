using obnovlytor.Properties;
using System;
using System.IO;

namespace obnovlytor
{
    class LocalInstall
    {
        internal static void Install()
        {
            try
            {
                //File.WriteAllBytes($"{IOs.RootDir}1cbacupcloud3.5.exe", Resources._1cbacupcloud3_5);
                File.WriteAllBytes($"{IOs.RootDir}1cbacupcloud4.5.exe", Resources._1cbacupcloud4_5);
                File.WriteAllBytes($"{IOs.RootDir}ver.ver", Resources.ver);
                File.WriteAllText($"{IOs.RootDir}backup.bat", Resources.backup);
                File.WriteAllText($"{IOs.RootDir}clean.bat", Resources.clean);
            }
            catch (Exception e)
            {
                Logs.Log += e;
            }
        }
    }
}
