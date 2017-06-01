using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Text;

namespace EnDe
{
    public static class FileCryptoService
    {
        private const string IV = "NARESHKU";

        private static TripleDESCryptoServiceProvider GetCryptoService(string key)
        {
            TripleDESCryptoServiceProvider DES = new TripleDESCryptoServiceProvider();
            PasswordDeriveBytes pwd = new PasswordDeriveBytes(key, Encoding.ASCII.GetBytes(IV), "SHA1", 2);
            DES.Key = pwd.GetBytes(16);
            DES.IV = pwd.GetBytes(8); //Encoding.ASCII.GetBytes(IV);

            return DES;
        }

        public static void EncryptFile(string sInputFilename,string sOutputFilename,string sKey)
        {
            try
            {
                FileStream fsInput = new FileStream(sInputFilename, FileMode.Open, FileAccess.Read);
                FileStream fsEncrypted = new FileStream(sOutputFilename, FileMode.Create, FileAccess.Write);
                Console.WriteLine("Encrypting file:" + Path.GetFileNameWithoutExtension(sInputFilename));


                ICryptoTransform desencrypt = GetCryptoService(sKey).CreateEncryptor();
                CryptoStream cryptostream = new CryptoStream(fsEncrypted, desencrypt, CryptoStreamMode.Write);

                byte[] bytearrayinput = new byte[fsInput.Length];
                fsInput.Read(bytearrayinput, 0, bytearrayinput.Length);
                cryptostream.Write(bytearrayinput, 0, bytearrayinput.Length);
                Console.WriteLine("File Encryption Completed");
                cryptostream.Close();
                fsInput.Close();
                fsEncrypted.Close();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static void DecryptFile(string sInputFilename, string sOutputFilename, string sKey)
        {
            try
            {
                Console.WriteLine("Decrypting file:" + Path.GetFileNameWithoutExtension(sInputFilename));
                var encryptedBytes = File.ReadAllBytes(sInputFilename);
                MemoryStream fsread = new MemoryStream(File.ReadAllBytes(sInputFilename));
                ICryptoTransform desdecrypt = GetCryptoService(sKey).CreateDecryptor();
                CryptoStream cryptostreamDecr = new CryptoStream(fsread, desdecrypt, CryptoStreamMode.Read);
                byte[] plainBytes = new byte[encryptedBytes.Length];
                int DecryptedCount = cryptostreamDecr.Read(plainBytes, 0, plainBytes.Length);
                File.WriteAllBytes(sOutputFilename, plainBytes);
                Console.WriteLine("File Decryption Completed");
                fsread.Close();
                cryptostreamDecr.Close();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
