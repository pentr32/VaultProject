using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleUI.Model
{
    public class StoredEncryptedFile
    {
        public string FilePath { get; set; }
        public byte[] IV { get; set; }
    }
}
