using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Text;

namespace EnDe
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //Decrypt Current Directory Files
                if (args == null || args.Length == 0)
                {
                    var allFiles = GetCurrentDirectoryEncryptedFiles();
                    foreach (var file in allFiles)
                    {
                        Console.WriteLine("Decrypting file:" + Path.GetFileName(file));
                        var key = GetKeyFromUser();
                        var decryptedFile = Path.ChangeExtension(file, Path.GetExtension(file).Replace("ende", ""));
                        try
                        {
                            FileCryptoService.DecryptFile(file, decryptedFile, key);
                        }
                        catch (CryptographicException)
                        {

                            Console.WriteLine("Incorrect Password. Please Try Again.");
                        }
                    }
                }
                //Encryption 
                else if (args.Length > 0 && (args[0].ToUpper() == "E" || args[0].ToUpper() == "ENCRYPT"))
                {
                    var filePath = GetFilePath(args);
                    var key = GetKeyFromUser();
                    var encryptedFile = Path.ChangeExtension(filePath, Path.GetExtension(filePath) + "ende");
                    FileCryptoService.EncryptFile(filePath, encryptedFile, key);
                }
                //Decryption
                else if (args.Length > 0 && (args[0].ToUpper() == "D" || args[0].ToUpper() == "DECRYPT"))
                {
                    string filePath = GetFilePath(args);
                    var key = GetKeyFromUser();
                    var decryptedFile = Path.ChangeExtension(filePath, Path.GetExtension(filePath).Replace("ende", ""));
                    try
                    {
                        FileCryptoService.DecryptFile(filePath, decryptedFile, key);
                    }
                    catch (CryptographicException)
                    {
                        Console.WriteLine("Incorrect Password. Please Try Again.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception Occured:" + ex.Message);
            }
            Console.ReadLine();
        }
        private static string GetFilePath(string[] args)
        {
            string filePath = string.Empty;
            if (args.Length == 2)
            {
                filePath = args[1];
                if (!File.Exists(filePath))
                    filePath = GetFilePathFromUser();
            }
            else
                filePath = GetFilePathFromUser();
            return filePath;

        }
        private static List<string> GetCurrentDirectoryEncryptedFiles()
        {
            List<string> filesList = new List<string>();

            filesList =Directory.GetFiles(Directory.GetCurrentDirectory(),"*.*ende").ToList();

            return filesList;
        }

        private static string GetFilePathFromUser()
        {
            string path = string.Empty;
            while(true)
            {
                Console.WriteLine("Enter File Path:");
                path = Console.ReadLine();
                if (File.Exists(path))
                    break;
                else
                    Console.WriteLine("File Not Found.");
            }
            return path;
        }

        private static string GetKeyFromUser()
        {
            StringBuilder passPhrase = new StringBuilder();
            ConsoleKeyInfo key;
            Console.WriteLine("Enter Password:");
            do
            {
                key = Console.ReadKey(true);

                if (key.Key != ConsoleKey.Enter)
                {
                    // Append the character to the password.
                    passPhrase.Append(key.KeyChar);
                    Console.Write("*");
                }
                // Exit if Enter key is pressed.
            } while (key.Key != ConsoleKey.Enter);
            Console.WriteLine();
            return passPhrase.ToString(); ;
        }
    }
}
