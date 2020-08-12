using System;
using System.Collections.Generic;
using System.Text;
using VaultLibrary;

namespace ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            Cryptography newCrypto = new Cryptography();
            Console.WriteLine("Vaultproject");

            var hej = GenerateNewPassword(ref newCrypto, 32, 16, "www.google.dk", "hej", Console.ReadLine());
            Console.WriteLine(Convert.ToBase64String(hej));
            
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

        public static byte[] GenerateNewPassword(ref Cryptography newCrypto, int keyLenght, int ivLenght, string site, string username, string password)
        {
            var key = newCrypto.GenerateRandomNumber(keyLenght);
            var iv = newCrypto.GenerateRandomNumber(ivLenght);
            //const string original = "This my secret message";

            var encrypted = newCrypto.EncryptPassword(Encoding.UTF8.GetBytes(password), key, iv);
            return encrypted;
        }
    }
}
