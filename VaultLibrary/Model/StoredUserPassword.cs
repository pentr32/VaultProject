using System;
using System.Collections.Generic;
using System.Text;

namespace VaultLibrary.Model
{
    public class StoredUserPassword
    {
        public string Site { get; set; }
        public string EncryptedPassword { get; set; }
        public byte[] IV { get; set; }
    }
}
