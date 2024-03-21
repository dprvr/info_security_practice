using System;
using System.IO;

namespace EC.Core
{
    internal class UsersDirectoriesManager
    {
        private readonly string UsersDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "ECTest");

        internal void LockUserDirectory(string username)
        {
            var pathToUserDirectory = Path.Combine(UsersDataPath, username.ToLowerInvariant());
            var files = Directory.GetFiles(pathToUserDirectory, "*", SearchOption.AllDirectories);
            
            var cryptService = CryptService.Create(username);
            foreach (var file in files)
            {
                cryptService.EncryptFile(file);
            }
        }

        internal string UnlockUserDirectory(string username)
        {
            var pathToUserDirectory = Path.Combine(UsersDataPath, username.ToLowerInvariant());
            if (!Directory.Exists(pathToUserDirectory))
            {
                Directory.CreateDirectory(pathToUserDirectory);
            }
            else
            {
                var files = Directory.GetFiles(pathToUserDirectory, "*", SearchOption.AllDirectories);
                if (files.Length == 0)
                    return pathToUserDirectory;
                var cryptService = CryptService.Create(username);
                foreach (var file in files)
                {
                    cryptService.DecryptFile(file);
                }
            }
            return pathToUserDirectory;
        }
    }
}
