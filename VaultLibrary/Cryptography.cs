using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace VaultLibrary
{
    public class Cryptography
    {
        #region HashingWithSalt - Master Password
        //public byte[] GenerateSalt()
        //{
        //    const int saltLength = 32;

        //    using (var randomNumberGenerator = new RNGCryptoServiceProvider())
        //    {
        //        var randomNumber = new byte[saltLength];
        //        randomNumberGenerator.GetBytes(randomNumber);

        //        return randomNumber;
        //    }
        //}

        private byte[] Combine(byte[] first, byte[] second)
        {
            var ret = new byte[first.Length + second.Length];

            Buffer.BlockCopy(first, 0, ret, 0, first.Length);
            Buffer.BlockCopy(second, 0, ret, first.Length, second.Length);

            return ret;
        }

        public byte[] HashPasswordWithSalt(byte[] toBeHashed, byte[] salt)
        {
            using (var sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(Combine(toBeHashed, salt));
            }
        }
        #endregion HashingWithSalt



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

        public string EncryptFileAES(string filePath, byte[] key, byte[] iv)
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
                return "Filen kunne ikke findes eller kunne ikke encrypteres!";
            }
        }

        public string DecryptFileAES(string filePath, byte[] key, byte[] iv)
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
            catch (Exception e)
            {
                return "Filen kunne ikke findes eller den er allerede decrypteret!";
            }
        }
    }
}
