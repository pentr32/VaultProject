using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace VaultLibrary
{
    public class Cryptography
    {
        //public List<StoredUserPassword> AllStoredUserPasswords { get; set; }

        //public Cryptography()
        //{
        //    AllStoredUserPasswords = new List<StoredUserPassword>();
        //}

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

        public string EncryptFileDES(string filePath, byte[] key, byte[] iv)
        {
            try
            {
                byte[] file = File.ReadAllBytes(filePath);
                using (var AES = new AesCryptoServiceProvider())
                {
                    AES.IV = iv;
                    AES.Key = key;
                    AES.Mode = CipherMode.CBC;
                    AES.Padding = PaddingMode.PKCS7;

                    using (var memStream = new MemoryStream())
                    {
                        CryptoStream cryptoStream = new CryptoStream(memStream, AES.CreateEncryptor(), CryptoStreamMode.Write);

                        cryptoStream.Write(file, 0, file.Length);
                        cryptoStream.FlushFinalBlock();
                        File.WriteAllBytes(filePath, memStream.ToArray());
                    }
                }

                return "Filen er encrypteret!";
            }
            catch
            {
                return "Der sket en fejl!";
            }
        }

        public string DecryptFileDES(string filePath, byte[] key, byte[] iv)
        {
            try
            {
                byte[] file = File.ReadAllBytes(filePath);
                using (var AES = new AesCryptoServiceProvider())
                {
                    AES.IV = iv;
                    AES.Key = key;
                    AES.Mode = CipherMode.CBC;
                    AES.Padding = PaddingMode.PKCS7;

                    using (var memStream = new MemoryStream())
                    {
                        CryptoStream cryptoStream = new CryptoStream(memStream, AES.CreateDecryptor(), CryptoStreamMode.Write);

                        cryptoStream.Write(file, 0, file.Length);
                        cryptoStream.FlushFinalBlock();
                        File.WriteAllBytes(filePath, memStream.ToArray());
                    }
                }

                return "Filen er decrypteret!";
            }
            catch
            {
                return "Der sket en fejl!";
            }
        }
    }
}
