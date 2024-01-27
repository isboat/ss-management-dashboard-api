using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Management.Dashboard.Models.Encryption
{
    public class EncryptedResult
    {
        public string? Hashed { get; set; }

        public string?  UsedSalt { get; set; }
    }
}
