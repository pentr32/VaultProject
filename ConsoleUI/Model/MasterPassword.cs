using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleUI.Model
{
    class MasterPassword
    {
        public byte[] GeneratedSalt { get; set; }
        public byte[] HashedSaltPassword { get; set; }
    }
}
