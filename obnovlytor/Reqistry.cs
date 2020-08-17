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
                IOs.ImagePathAgent = key.GetValue("ImagePath").ToString().Replace("agentNET.exe", "").Replace('"', ' ');
            }
            catch (Exception ex)
            {
                Logs.Log += ex;
            }
        }
        internal static string AgentServiceName()
        {
            string name = string.Empty;
            try
            {
                RegistryKey key = Registry.LocalMachine;
                key = key.OpenSubKey(@"SYSTEM\CurrentControlSet\Services\1CBackupAgent");
                name = key.GetValue("DisplayName").ToString();
            }
            catch (Exception ex)
            {
                Logs.Log += ex;
            }
            return name;
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
                Logs.Log += ex;
                return false;
            }
        }
    }
}
