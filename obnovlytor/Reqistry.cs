using Microsoft.Win32;
using System;

namespace obnovlytor
{
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
}
