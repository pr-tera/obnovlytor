using System.IO;

namespace obnovlytor
{
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
}
