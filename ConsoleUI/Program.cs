using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using VaultLibrary;
using VaultLibrary.Model;

namespace ConsoleUI
{
    class Program
    {
        public static List<StoredUserPassword> AllStoredUserPasswords { get; set; }

        static void Main(string[] args)
        {
            AllStoredUserPasswords = new List<StoredUserPassword>();

            Cryptography newCrypto = new Cryptography();

            #region PassordEncrypt
            var key = newCrypto.GenerateRandomNumber(32);
            #endregion PassordEncrypt

            Console.WriteLine("Vaultproject");

            
            for (int i = 0; i < 3; i++)
            {
                byte[] encrypted = GenerateNewUserPassword(ref newCrypto, key, 16, "www.google.dk", "hej", Console.ReadLine());
                Console.WriteLine(Convert.ToBase64String(encrypted));
            }
            Console.WriteLine("------------");
            foreach (var item in AllStoredUserPasswords)
            {
                Console.WriteLine(DecryptPasswordOfUser(ref newCrypto, key, item.IV, item.EncryptedPassword));
            }
            var test = AllStoredUserPasswords;

            //var abe = EncryptFile(ref newCrypto, 32, 16, @"C:\Users\robe1819\Desktop\mercedes.jpeg");
            //Console.WriteLine(abe);

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

        
        public static string EncryptFile(ref Cryptography newCrypto, int keyLenght, int ivLenght, string filePath)
        {
            var key = newCrypto.GenerateRandomNumber(keyLenght);
            var iv = newCrypto.GenerateRandomNumber(ivLenght);

            string result = newCrypto.EncryptFileDES(filePath, key, iv);
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
        public static string DecryptPasswordOfUser(ref Cryptography newCrypto, byte[] key, byte[] iv, byte[] password)
        {
            return Encoding.UTF8.GetString(newCrypto.DecryptPassword(password,key,iv));
        }
    }
}
