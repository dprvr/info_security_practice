using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace EC.Core
{
    internal class CryptService
    {
        private readonly byte[] key;
        private readonly byte[] vector;

        public static CryptService Create(string key)
        {
            var hasher = new HashComputator();
            var usernameHash = hasher.ComputeHash(Encoding.Unicode.GetBytes(key));
            var reversedUsernameHash = hasher.ComputeHash(Encoding.Unicode.GetBytes(string.Join("*", key.Reverse())));
            return new CryptService(usernameHash, reversedUsernameHash);
        }

        private CryptService(byte[] key, byte[] vector)
        {
            this.key = key;
            this.vector = vector.Take(16).ToArray();
        }

        public void EncryptFile(string path)
        {
            byte[] content = File.ReadAllBytes(path);
            var encryptedContent = Encrypt(content);
            File.WriteAllBytes(path, encryptedContent);
        }

        public void DecryptFile(string path)
        {
            byte[] content = File.ReadAllBytes(path);
            var decryptedContent = Decrypt(content);
            File.WriteAllBytes(path, decryptedContent);
        }

        private byte[] Encrypt(byte[] data)
        {
            byte[] encryptedData;
            using(Aes aes = Aes.Create())
            {
                aes.KeySize = 256;
                aes.Key = key;
                aes.IV = vector;
                using MemoryStream outStream = new();
                using (CryptoStream cs = new(outStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(data, 0, data.Length);
                    cs.FlushFinalBlock();
                }
                encryptedData = outStream.ToArray();
            }
            return encryptedData;
        }

        private byte[] Decrypt(byte[] data)
        {
            byte[] decryptedData;
            using (Aes aes = Aes.Create())
            {
                aes.KeySize = 256;
                aes.Key = key;
                aes.IV = vector;
                using MemoryStream outStream = new();
                using (CryptoStream cs = new(outStream, aes.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(data, 0, data.Length);
                    cs.FlushFinalBlock();
                }
                decryptedData = outStream.ToArray();
            }
            return decryptedData;
        }
    }
}
