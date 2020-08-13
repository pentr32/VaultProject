using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleUI.Model
{
    public class StoredDocumentAndSignature
    {
        public string Document { get; set; }
        public byte[] HashedDocument { get; set; }
        public byte[] Signature { get; set; }
    }
}
