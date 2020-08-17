using System.IO;

namespace obnovlytor
{
    class Log
    {
        internal static void Write()
        {
            if (!string.IsNullOrEmpty(Logs.Log))
            {
                using (FileStream fstream = new FileStream($"{IOs.RootDir}\\update.log", FileMode.OpenOrCreate))
                {
                    using (StreamWriter sw = new StreamWriter(fstream, System.Text.Encoding.Default))
                    {
                        sw.Write(Logs.Log);
                    }
                }
            }
        }
    }
    struct Logs
    {
        internal static string Log { get; set; }
    }
}
