using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager_Services.Domains.Security
{
    public static class SHA256Hasher
    {
        public static string Hash(string input)
        {
            byte[] digest = SHA256.HashData(Encoding.ASCII.GetBytes(input));
            StringBuilder sb = new();

            foreach (byte b in digest)
            {
                sb.Append(b.ToString("X2", CultureInfo.InvariantCulture));
            }

            return sb.ToString();
        }
    }
}
