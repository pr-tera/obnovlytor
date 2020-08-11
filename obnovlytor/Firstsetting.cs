﻿using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

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
