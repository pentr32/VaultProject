using System;
using System.Collections.Generic;
using System.Text;

namespace VaultLibrary.Model
{
    public class StoredUserPassword
    {
        public string Site { get; set; }
        public string Username { get; set; }
        public byte[] EncryptedPassword { get; set; }
        public byte[] IV { get; set; }
    }
}
