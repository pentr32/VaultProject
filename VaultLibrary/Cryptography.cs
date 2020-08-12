using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using VaultLibrary.Model;

namespace VaultLibrary
{
    public class Cryptography
    {
        public byte[] KeyPassword { get; set; }

        public List<StoredUserPassword> AllStoredUserPasswords { get; set; }

        public Cryptography()
        {
            AllStoredUserPasswords = new List<StoredUserPassword>();
        }

        public byte[] GenerateRandomNumber(int length)
        {
            using (var randomNumberGenerator = new RNGCryptoServiceProvider())
            {
                var randomNumber = new byte[length];
                randomNumberGenerator.GetBytes(randomNumber);

                return randomNumber;
            }
        }

        public byte[] EncryptPassword(byte[] dataToEncrypt, byte[] key, byte[] iv)
        {
            using (var aes = new AesCryptoServiceProvider())
            {
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                aes.Key = key;
                aes.IV = iv;

                using (var memoryStream = new MemoryStream())
                {
                    var cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(),
                        CryptoStreamMode.Write);

                    cryptoStream.Write(dataToEncrypt, 0, dataToEncrypt.Length);
                    cryptoStream.FlushFinalBlock();

                    return memoryStream.ToArray();
                }
            }
        }

        public byte[] DecryptPassword(byte[] dataToDecrypt, byte[] key, byte[] iv)
        {
            using (var aes = new AesCryptoServiceProvider())
            {
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                aes.Key = key;
                aes.IV = iv;

                using (var memoryStream = new MemoryStream())
                {
                    var cryptoStream = new CryptoStream(memoryStream, aes.CreateDecryptor(),
                        CryptoStreamMode.Write);

                    cryptoStream.Write(dataToDecrypt, 0, dataToDecrypt.Length);
                    cryptoStream.FlushFinalBlock();

                    var decryptBytes = memoryStream.ToArray();

                    return decryptBytes;
                }
            }
        }

        public void EncryptFileDES(string filePath, byte[] key, byte[] iv)
        {
            try
            {
                byte[] file = File.ReadAllBytes(filePath);
                using (var DES = new DESCryptoServiceProvider())
                {
                    DES.IV = iv;
                    DES.Key = key;
                    DES.Mode = CipherMode.CBC;
                    DES.Padding = PaddingMode.PKCS7;

                    using (var memStream = new MemoryStream())
                    {
                        CryptoStream cryptoStream = new CryptoStream(memStream, DES.CreateEncryptor(), CryptoStreamMode.Write);

                        cryptoStream.Write(file, 0, file.Length);
                        cryptoStream.FlushFinalBlock();
                        File.WriteAllBytes(filePath, memStream.ToArray());
                    }
                }

                Console.WriteLine("Filen er encrypted!");
            }
            catch (Exception e)
            {
                var error = e;
                Console.WriteLine("Der sket en fejl!");
            }
        }

        public void DecryptFileDES(string filePath)
        {

        }
    }
}
