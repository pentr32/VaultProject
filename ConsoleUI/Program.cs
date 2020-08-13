using ConsoleUI.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using VaultLibrary;
using VaultLibrary.Model;

namespace ConsoleUI
{
    class Program
    {
        public static MasterPassword MasterPassword { get; set; }
        public static List<StoredUserPassword> AllStoredUserPasswords { get; set; }                     // Liste med alle encrypted password med deres IV og WebSide. 
        public static List<StoredEncryptedFile> AllStoredEncryptedFiles { get; set; }                   // Liste med alle FilePaths og deres IV.

        static void Main(string[] args)
        {

            #region Config
            MasterPassword = new MasterPassword();
            AllStoredUserPasswords = new List<StoredUserPassword>();                                    
            AllStoredEncryptedFiles = new List<StoredEncryptedFile>();                                  
            Cryptography newCrypto = new Cryptography();                                                

            var keyPassword = newCrypto.GenerateRandomNumber(32);                                       // Generer et ny key med random numbers til passwords. 
            var keyFile = newCrypto.GenerateRandomNumber(32);                                           // Generer et ny key med random numbers til filer.

            string currentUserPath = System.Environment.GetEnvironmentVariable("USERPROFILE");
            #endregion Config


            GenerateMasterPassword(ref newCrypto);

            string insertedMasterPasswordConsole;
            do
            {
                Console.Write("Insert master password:");
                insertedMasterPasswordConsole = Console.ReadLine();

            } while (CheckIfMasterPasswordMatch(ref newCrypto, insertedMasterPasswordConsole) == false);

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Vaultproject\n\n");
                Console.WriteLine("====================================");
                Console.WriteLine("| 1 - Create new ecrypted password | \n| 2 - Show all my passwords        | \n| 3 - Encrypt a file               | \n| 4 - Decrypt file                 |", Color.GreenYellow);
                Console.WriteLine("====================================");

                Console.Write("\n Indtast: ");

                int indtastedNr = Convert.ToInt32(Console.ReadLine());
                switch (indtastedNr)
                {
                    case 1:
                        Console.Write("Indtast side:");
                        string site = Console.ReadLine();
                        Console.Write("Indtast brugernavn:");
                        string username = Console.ReadLine();
                        Console.Write("Indtast password:");
                        string password = Console.ReadLine();
                        byte[] encryptedPassword = GenerateNewUserPassword(ref newCrypto, keyPassword, 16, site, username, password);
                        Console.WriteLine($"Dit krypterede password er: {Convert.ToBase64String(encryptedPassword)}");
                        break;

                    case 2:
                        foreach (StoredUserPassword userPassword in AllStoredUserPasswords)
                        {
                            string decryptedPassword = DecryptPasswordOfUser(ref newCrypto, keyPassword, userPassword.IV, userPassword.EncryptedPassword);
                            Console.WriteLine($"URL: {userPassword.Site} | Username: {userPassword.Username} | Password: {decryptedPassword}");
                            Console.WriteLine();
                        }
                        break;

                    case 3:
                        Console.Clear();
                        Console.WriteLine("Placer din fil på DESKTOP og indtast filens navn nedenunder");
                        Console.Write("Filens navn: ");
                        string filNavnENC = Console.ReadLine();

                        var encResult = EncryptFile(ref newCrypto, keyFile, 16, currentUserPath + @"\Desktop\" + filNavnENC);
                        Console.WriteLine(encResult);
                        break;

                    case 4:
                        Console.Clear();
                        Console.WriteLine("Indtast den encrypteret filnavn nedenunder");
                        Console.Write("Filens navn: ");
                        string filNavnDEC = Console.ReadLine();

                        var decResult = DecryptFile(ref newCrypto, keyFile, currentUserPath + @"\Desktop\" + filNavnDEC);
                        Console.WriteLine(decResult);
                        break;

                    default:
                        Console.WriteLine("Du indtastede forkert!");
                        break;
                }

                Console.ReadKey();
                Console.Clear();
            }

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

        #region MasterPassword
        public static void GenerateMasterPassword(ref Cryptography newCrypto)
        {
            Console.WriteLine("Opret master password");
            string masterPasswordPlain = Console.ReadLine();

            MasterPassword.GeneratedSalt = newCrypto.GenerateRandomNumber(32); // Used to generate salt. Salt lenght 32

            Console.WriteLine("Salt = " + Convert.ToBase64String(MasterPassword.GeneratedSalt));


            MasterPassword.HashedSaltPassword = newCrypto.HashPasswordWithSalt(Encoding.UTF8.GetBytes(masterPasswordPlain), MasterPassword.GeneratedSalt);

            Console.WriteLine("Hashed Password = " + Convert.ToBase64String(MasterPassword.HashedSaltPassword));
        }

        public static bool CheckIfMasterPasswordMatch(ref Cryptography newCrypto, string plainPasswordInserted)
        {
            var insertedPasswordHashedSalt = newCrypto.HashPasswordWithSalt( Encoding.UTF8.GetBytes(plainPasswordInserted), MasterPassword.GeneratedSalt);
            if (Convert.ToBase64String(MasterPassword.HashedSaltPassword) == Convert.ToBase64String(insertedPasswordHashedSalt))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion MasterPassword

        #region PasswordManage
        public static byte[] GenerateNewUserPassword(ref Cryptography newCrypto, byte[] key, int iv, string site, string username, string password)
        {
            var ivRandom = newCrypto.GenerateRandomNumber(iv);
            var encrypted = newCrypto.EncryptPassword(Encoding.UTF8.GetBytes(password), key, ivRandom);

            AllStoredUserPasswords.Add(
                new StoredUserPassword { Site = site, IV = ivRandom, EncryptedPassword = encrypted, Username = username }
            );
            
            return encrypted;
        }
        public static string DecryptPasswordOfUser(ref Cryptography newCrypto, byte[] key, byte[] iv, byte[] password)
        {
            return Encoding.UTF8.GetString(newCrypto.DecryptPassword(password,key,iv));
        }
        #endregion PasswordManage
    }
}
