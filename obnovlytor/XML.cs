using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Xml;

namespace obnovlytor
{
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
}
