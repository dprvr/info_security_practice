using System;
using System.Security.Cryptography;
using System.Text;

namespace EC.Core
{
    internal class HashComputator
    {
        public string ComputeHash(string content)
        {
            string hash;
            using (SHA256 sha = SHA256.Create())
            {
                var contentBytes = Encoding.Unicode.GetBytes(content);
                var hashBytes = sha.ComputeHash(contentBytes);
                hash = Convert.ToBase64String(hashBytes);
            }
            return hash;
        }

        public byte[] ComputeHash(byte[] bytes)
        {
            using SHA256 sha = SHA256.Create();
            var hashBytes = sha.ComputeHash(bytes);
            return hashBytes;
        }


    }
}
