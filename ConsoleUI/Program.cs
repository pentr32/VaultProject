using ConsoleUI.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using VaultLibrary;
using VaultLibrary.Model;

namespace ConsoleUI
{
    class Program
    {
        public static List<StoredUserPassword> AllStoredUserPasswords { get; set; }                     // Liste med alle encrypted password med deres IV og WebSide. 
        public static List<StoredEncryptedFile> AllStoredEncryptedFiles { get; set; }                   // Liste med alle FilePaths og deres IV.

        static void Main(string[] args)
        {

            #region Config
            AllStoredUserPasswords = new List<StoredUserPassword>();                                    
            AllStoredEncryptedFiles = new List<StoredEncryptedFile>();                                  
            Cryptography newCrypto = new Cryptography();                                                

            var keyPassword = newCrypto.GenerateRandomNumber(32);                                       // Generer et ny key med random numbers til passwords. 
            var keyFile = newCrypto.GenerateRandomNumber(32);                                           // Generer et ny key med random numbers til filer.

            string currentUserPath = System.Environment.GetEnvironmentVariable("USERPROFILE");
            #endregion Config

            while (true)
            {
                Console.WriteLine("Vaultproject\n\n");
                Console.WriteLine("====================================");
                Console.WriteLine("| 1 - Create new ecrypted password | \n| 2 - Show all my passwords        | \n| 3 - Encrypt a file               | \n| 4 - Decrypt file                 |", Color.GreenYellow);
                Console.WriteLine("====================================");

                Console.Write("\n Indtast: ");

                int indtastedNr = Convert.ToInt32(Console.ReadLine());
                switch (indtastedNr)
                {
                    case 1:

                        break;
                    case 2:
                        break;
                    case 3:
                        Console.Clear();
                        Console.WriteLine("Placer dit fil på DESKTOP og indtast filens navn nedunder");
                        Console.Write("Filens navn: ");
                        string filNavnENC = Console.ReadLine();

                        var encResult = EncryptFile(ref newCrypto, keyFile, 16, currentUserPath + @"\Desktop\" + filNavnENC);
                        Console.WriteLine(encResult);
                        break;
                    case 4:
                        Console.Clear();
                        Console.WriteLine("Indtast den encrypteret filnavn nedunder");
                        Console.Write("Filens navn: ");
                        string filNavnDEC = Console.ReadLine();

                        var decResult = DecryptFile(ref newCrypto, keyFile, currentUserPath + @"\Desktop\" + filNavnDEC);
                        Console.WriteLine(decResult);
                        break;
                    default:
                        Console.WriteLine("Du indtasted forkert!");
                        break;
                }

                Console.ReadKey();
                Console.Clear();
            }



            //for (int i = 0; i < 3; i++)
            //{
            //    byte[] encrypted = GenerateNewUserPassword(ref newCrypto, keyPassword, 16, "www.google.dk", "hej", Console.ReadLine());
            //    Console.WriteLine(Convert.ToBase64String(encrypted));
            //}
            //var test = AllStoredUserPasswords;

            //var key = newCrypto.GenerateRandomNumber(32);
            //var iv = newCrypto.GenerateRandomNumber(16);
            //const string original = "This my secret message";

            //var encrypted = newCrypto.EncryptPassword(Encoding.UTF8.GetBytes(original), key, iv);
            //var decrypted = newCrypto.DecryptPassword(encrypted, key, iv);

            //var decryptedMessage = Encoding.UTF8.GetString(decrypted);

            //Console.WriteLine("AES Encryption Demonstration in .NET");
            //Console.WriteLine("------------------------------------");
            //Console.WriteLine();
            //Console.WriteLine("Original Text = " + original);
            //Console.WriteLine("Encrypted Text = " + Convert.ToBase64String(encrypted));
            //Console.WriteLine("Decrypted Text = " + decryptedMessage);

            //Console.ReadLine();


        }

        
        public static string EncryptFile(ref Cryptography newCrypto, byte[] key, int iv, string filePath)
        {
            var ivRandom = newCrypto.GenerateRandomNumber(iv);
            string result = newCrypto.EncryptFileAES(filePath, key, ivRandom);

            AllStoredEncryptedFiles.Add(
                new StoredEncryptedFile { FilePath = filePath, IV = ivRandom });

            return result;
        }

        public static string DecryptFile(ref Cryptography newCrypto, byte[] key, string filePath)
        {
            StoredEncryptedFile foundFile = AllStoredEncryptedFiles.Find(x => x.FilePath == filePath);
            string result = newCrypto.DecryptFileAES(filePath, key, foundFile.IV);
            return result;
        }

        public static byte[] GenerateNewUserPassword(ref Cryptography newCrypto, byte[] key, int iv, string site, string username, string password)
        {
            var ivRandom = newCrypto.GenerateRandomNumber(iv);
            var encrypted = newCrypto.EncryptPassword(Encoding.UTF8.GetBytes(password), key, ivRandom);

            AllStoredUserPasswords.Add(
                new StoredUserPassword { Site = site, IV = ivRandom, EncryptedPassword = encrypted }
            );
            
            return encrypted;
        }
    }
}
