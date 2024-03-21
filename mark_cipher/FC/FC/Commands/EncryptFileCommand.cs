using FC.Algo;

using Spectre.Console;
using Spectre.Console.Cli;

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;
using CliFx;
using CliFx.Attributes;
using System.Threading.Tasks;
using CliFx.Infrastructure;

namespace FC.Commands
{
    [Command("Encrypt file", Description = "Encrypts the given file text with mark algorithm.")]
    public class EncryptFileCommand : ICommand
    {
        [CommandParameter(0, Name = "EncryptingFilePath", Description = "The path to the file which text will be encrypt.", Validators = new Type[] { })]
        public string EncryptingFilePath { get; init; }

        [CommandParameter(1, Name = "CryptKey", Description = "The crypt key that using to encrypt/decrypt text.", Validators = new Type[] { })]
        public string CryptKey { get; init; }

        [CommandParameter(2, Name = "OutFilePath", Description = "The path to file than will be used to save encrypted text.")]
        public string OutFilePath { get; init; }
        

        public ValueTask ExecuteAsync(IConsole console)
        {
            //if (string.IsNullOrEmpty(encryptingFilePath))
            //    return ValidationResult.Error($"The given path was empty.");
            //if (encryptingFilePath.FilePathContainsInvalidCharacters())
            //    return ValidationResult.Error($"The given file path({encryptingFilePath}) contains invalid characters.");
            //if (!encryptingFilePath.TxtFilePath())
            //    return ValidationResult.Error($"The input file can be only txt file.");
            //if (encryptingFilePath.ExistingFilePath())
            //    return ValidationResult.Error($"The file with path({encryptingFilePath}) doesn't exist.");

            var text = File.ReadAllText(EncryptingFilePath);
            if (string.IsNullOrEmpty(text))
                throw new ArgumentException("");

            var matches = Regex.Matches(text, "[а-я]|[А-Я]");
            if (matches.Count < 1)
                throw new ArgumentException($"The given file ({EncryptingFilePath}) doesn't contains any data units that can't be encrypt. The data unit is russian letter and nothing else.");
            var encryptingText = string.Concat(matches.Select(m => m.Value).ToArray());

            var encryptedText = MarkCryptor.CreateRusAlphabetCryptor(CryptKey).Encrypt(encryptingText);
            File.WriteAllText(OutFilePath, encryptedText);
            return default;
        }
    }
}
