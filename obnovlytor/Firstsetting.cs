using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Xml;

namespace obnovlytor
{
    class Firstsetting : XML
    {
        public static bool Start()
        {
            if (Install.GetAgent() == true)
            {
                if (GenParametrsXML() == true)
                {
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
    }
    class XML
    {
        internal static bool GenParametrsXML()
        {
            XmlDocument XmlParam = new XmlDocument();
            XmlDeclaration XmlDec = XmlParam.CreateXmlDeclaration("1.0", null, null);
            XmlParam.AppendChild(XmlDec);
            //Корень <Options>
            XmlElement xOptions = XmlParam.CreateElement("Options");
            XmlParam.AppendChild(xOptions);
            //PublicKey
            Data.PublicKey = DateTime.Now + Crypto.PrivatKey + DateTime.Now + Data.Path;
            XmlElement xPublicKey = XmlParam.CreateElement("PublicKey");
            xPublicKey.InnerText = Crypto.Encrypt(Data.PublicKey, Crypto.PrivatKey);
            xOptions.AppendChild(xPublicKey);
            /*
             */
            ConsoleKeyInfo key;
            var rule = @"[aA-zZ0-9!@#№$%&?*]";
            var buffer = "";
            //Console.CancelKeyPress += new ConsoleCancelEventHandler(GetBuff);
            /*
             */
            //login
            Console.Write("Логин:");
            while (true)
            {
                key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Enter)
                {
                    if (!string.IsNullOrEmpty(buffer))
                    {
                        Data.Login = buffer.ToString();
                    }
                    buffer = "";
                    break;
                }
                if (key.Key == ConsoleKey.Backspace)
                {
                    if (buffer.Length != 0)
                    {
                        buffer = buffer.Remove(buffer.Length - 1);
                        Console.Clear();
                        Console.Write("Логин:" + buffer);
                    }
                }
                if (key.Key == ConsoleKey.V && key.Modifiers == ConsoleModifiers.Control)
                {
                    Data.Login = Clipboard.GetText();
                    Console.Write(Clipboard.GetText());
                }
                if (System.Text.RegularExpressions.Regex.IsMatch(key.KeyChar.ToString(), rule))
                {
                    buffer += key.KeyChar;
                    Console.Write(key.KeyChar);
                }
            }
            XmlElement xLogin = XmlParam.CreateElement("Login");
            xLogin.InnerText = Crypto.Encrypt(Data.Login, Data.PublicKey);
            xOptions.AppendChild(xLogin);
            //Password
            Console.Write("\nПароль:");
            while (true)
            {
                key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Enter)
                {
                    if (!string.IsNullOrEmpty(buffer))
                    {
                        Data.Password = buffer.ToString();
                    }
                    buffer = "";
                    break;
                }
                if (key.Key == ConsoleKey.Backspace)
                {
                    if (buffer.Length != 0)
                    {
                        buffer = buffer.Remove(buffer.Length - 1);
                        Console.Clear();
                        Console.Write("Пароль:" + buffer);
                    }
                }
                if (key.Key == ConsoleKey.V && key.Modifiers == ConsoleModifiers.Control)
                {
                    Data.Password = Clipboard.GetText();
                    Console.Write(Clipboard.GetText());
                }
                if (System.Text.RegularExpressions.Regex.IsMatch(key.KeyChar.ToString(), rule))
                {
                    buffer += key.KeyChar;
                    Console.Write(key.KeyChar);
                }
            }
            //Data.Password = Console.ReadLine();
            XmlElement xPassword = XmlParam.CreateElement("Pwd");
            xPassword.InnerText = Crypto.Encrypt(Data.Password, Data.PublicKey);
            xOptions.AppendChild(xPassword);
            //Console.Write("Количество дней хранения:");
            //Data.Days = Console.ReadLine();
            Data.Days = "3";
            XmlElement xDays = XmlParam.CreateElement("Days");
            xDays.InnerText = Data.Days;
            xOptions.AppendChild(xDays);
            SetPath();
            if (!string.IsNullOrEmpty(Data.Login) && !string.IsNullOrEmpty(Data.Password) && !string.IsNullOrEmpty(Data.PublicKey))
            {
                /*вызвать функцию для активации
                 * активироватьАгента()
                 * {бла-бла}
                 */
                try
                {
                    XmlParam.Save(Data.Path + @"/Parametrs.xml");
                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    Data.Log += $"{DateTime.Now} {e} \n";
                    return false;
                }
            }
            else
            {
                Console.WriteLine("Введены не все данные\n");
                GenParametrsXML();
                return false;
            }
        }
        private static void GetBuff(object sender, ConsoleCancelEventArgs args)
        {
            Data.Buffer = Clipboard.GetText();
        }
        static void SetPath()
        {
            Reqistry.GetKey();
            if (File.Exists(Version.ImagePathAgent + @"\backup.path"))
            {
                File.Delete(Version.ImagePathAgent + @"\backup.path");
            }
            using (FileStream fstream = new FileStream(Version.ImagePathAgent + @"\backup.path", FileMode.OpenOrCreate))
            {
                using (StreamWriter sw = new StreamWriter(fstream, Encoding.Default))
                {
                    sw.Write(Request.LocalPath);
                }
            }
        }
    }
    class Install
    {
        internal static bool GetAgent()
        {
            if (DownloadAgetn() == true)
            {
                if (InstallAgent() == true)
                {
                    Directory.Delete(Data.Path + @"\backupagentTemp", true);
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
            string backupagentTemp = Data.Path + @"\backupagentTemp";
            if (!Directory.Exists(backupagentTemp))
            {
                try
                {
                    Directory.CreateDirectory(backupagentTemp);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    Data.Log += $"{DateTime.Now} {e} \n";
                }
            }
            //else
            //{
            //    Directory.Delete(backupagentTemp, true);
            //    Temp();
            //}
            return backupagentTemp;
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
                Data.Log += $"{DateTime.Now} {e} \n";
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
                Data.Log += $"{DateTime.Now} {e} \n";
                return false;
            }
        }
    }
    class Crypto : Encryptc
    {
        internal static string PrivatKey { get; } = "dHJGddhsbf&3gASt38c!!!xbKASdhgd*82JBSFhjks8ASbdjaod8**wbbka(*@jk";
        internal static string Encrypt(string str, string keyCrypt)
        {
            return Convert.ToBase64String(encrypt(Encoding.UTF8.GetBytes(str), keyCrypt));
        }
        internal static string Decrypt(string str, string keyCrypt)
        {
            string Result;
            try
            {
                CryptoStream Cs = InternalDecrypt(Convert.FromBase64String(str), keyCrypt);
                StreamReader Sr = new StreamReader(Cs);
                Result = Sr.ReadToEnd();
                Cs.Close();
                Cs.Dispose();
                Sr.Close();
                Sr.Dispose();
            }
            catch (CryptographicException)
            {
                return null;
            }
            return Result;
        }
    }
    class Encryptc : Decrypt
    {
        protected static byte[] encrypt(byte[] key, string value)
        {
            SymmetricAlgorithm Sa = Rijndael.Create();
            ICryptoTransform Ct = Sa.CreateEncryptor((new PasswordDeriveBytes(value, null)).GetBytes(16), new byte[16]);
            MemoryStream Ms = new MemoryStream();
            CryptoStream Cs = new CryptoStream(Ms, Ct, CryptoStreamMode.Write);
            Cs.Write(key, 0, key.Length);
            Cs.FlushFinalBlock();
            byte[] Result = Ms.ToArray();
            Ms.Close();
            Ms.Dispose();
            Cs.Close();
            Cs.Dispose();
            Ct.Dispose();
            return Result;
        }
    }
    class Decrypt
    {
        protected static CryptoStream InternalDecrypt(byte[] key, string value)
        {
            SymmetricAlgorithm sa = Rijndael.Create();
            ICryptoTransform ct = sa.CreateDecryptor((new PasswordDeriveBytes(value, null)).GetBytes(16), new byte[16]);
            MemoryStream ms = new MemoryStream(key);
            return new CryptoStream(ms, ct, CryptoStreamMode.Read);
        }
    }
    struct Data
    {
        internal static string Path { get; set; } = Directory.GetCurrentDirectory();
        internal static string PathPort { get; } = @"C:\1C\1cbackup-agent\metadata\port.properties";
        internal static string PublicKey { get; set; }
        internal static string Login { get; set; }
        internal static string Password { get; set; }
        internal static string Log { get; set; }
        internal static string Days { get; set; }
        internal static string Buffer { get; set; }
    }
}
