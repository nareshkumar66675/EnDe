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

        public static void EncryptFile(string sInputFilename,string sOutputFilename,string sKey)
        {
            try
            {
                FileStream fsInput = new FileStream(sInputFilename, FileMode.Open, FileAccess.Read);
                FileStream fsEncrypted = new FileStream(sOutputFilename, FileMode.Create, FileAccess.Write);
                TripleDESCryptoServiceProvider DES = new TripleDESCryptoServiceProvider();
                PasswordDeriveBytes pwd = new PasswordDeriveBytes(sKey, Encoding.ASCII.GetBytes(IV), "SHA1", 2);
                //Rfc2898DeriveBytes k2 = new Rfc2898DeriveBytes(sKey.ToString(), Encoding.ASCII.GetBytes(IV));
                Console.WriteLine("Encrytping file:" + Path.GetFileNameWithoutExtension(sInputFilename));
                DES.Key = pwd.GetBytes(16);
                DES.IV = pwd.GetBytes(8); //Encoding.ASCII.GetBytes(IV);


                ICryptoTransform desencrypt = DES.CreateEncryptor();
                CryptoStream cryptostream = new CryptoStream(fsEncrypted, desencrypt, CryptoStreamMode.Write);

                byte[] bytearrayinput = new byte[fsInput.Length];
                fsInput.Read(bytearrayinput, 0, bytearrayinput.Length);
                cryptostream.Write(bytearrayinput, 0, bytearrayinput.Length);
                Console.WriteLine("Encrytping file:" + Path.GetFileNameWithoutExtension(sInputFilename)+"Completed");
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
                TripleDESCryptoServiceProvider DES = new TripleDESCryptoServiceProvider();
                //Rfc2898DeriveBytes k2 = new Rfc2898DeriveBytes(sKey.ToString(), Encoding.ASCII.GetBytes(IV));
                PasswordDeriveBytes pwd = new PasswordDeriveBytes(sKey, Encoding.ASCII.GetBytes(IV), "SHA1", 2);
                //A 64 bit key and IV is required for this provider.
                //Set secret key For DES algorithm.
                DES.Key = pwd.GetBytes(16);
                //Set initialization vector.
                DES.IV = pwd.GetBytes(8);// Encoding.ASCII.GetBytes(IV);
                Console.WriteLine("Decrytping file:" + Path.GetFileNameWithoutExtension(sInputFilename));
                var encryptedBytes = File.ReadAllBytes(sInputFilename);
                //Create a file stream to read the encrypted file back.
                MemoryStream fsread = new MemoryStream(File.ReadAllBytes(sInputFilename));
                //Create a DES decryptor from the DES instance.
                ICryptoTransform desdecrypt = DES.CreateDecryptor();
                //Create crypto stream set to read and do a 
                //DES decryption transform on incoming bytes.
                CryptoStream cryptostreamDecr = new CryptoStream(fsread, desdecrypt, CryptoStreamMode.Read);
                //Print the contents of the decrypted file.
                byte[] plainBytes = new byte[encryptedBytes.Length];

                int DecryptedCount = cryptostreamDecr.Read(plainBytes, 0, plainBytes.Length);
                File.WriteAllBytes(sOutputFilename, plainBytes);
                Console.WriteLine("Decrytping file:" + Path.GetFileNameWithoutExtension(sInputFilename)+"Completed");
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
