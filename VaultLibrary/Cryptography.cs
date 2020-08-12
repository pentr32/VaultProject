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
