using System;
using System.Collections.Generic;
using System.Linq;

namespace FC.Algo
{
    public partial class MarkCryptor : ICryptAlgorithm
    {
        private static readonly IReadOnlyList<char> _russianAlhabet = new char[] {'а', 'б', 'в', 'г', 'д', 'е', 'ё', 'ж', 'з', 'и', 'й', 'к', 'л', 'м', 'н', 'о', 'п', 'р', 'с', 'т', 'у', 'ф', 'х', 'ц', 'ч', 'ш', 'щ', 'ь', 'ы', 'ъ', 'э', 'ю', 'я'};

        private readonly MarkEncryptor markEncryptor = null;
        private readonly MarkDecryptor markDecryptor = null;

        public static MarkCryptor Create(IReadOnlyList<char> alphabet, string key)
        {
            return new MarkCryptor(alphabet, key);
        }

        public static MarkCryptor CreateRusAlphabetCryptor(string key)
        {
            return new MarkCryptor(_russianAlhabet, key);
        }

        public MarkCryptor(IReadOnlyList<char> alphabet, string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            var keyLetters = key.ToLower().ToCharArray();

            if (keyLetters.Distinct().Count() != keyLetters.Length)
                throw new ArgumentException("The key must contains of non-repeatable russian letters.");

            if (keyLetters.Intersect(_russianAlhabet).Count() != keyLetters.Length)
                throw new ArgumentException("The key contains of non-repeatable russian letters sequence.");

            Alphabet = alphabet;
            Key = key;
            KeyLetters = keyLetters;
            RemainingLetters = alphabet.Except(keyLetters).ToArray();
        }

        public string Key { get; }

        private IReadOnlyList<char> Alphabet { get; }

        private IReadOnlyList<char> KeyLetters { get; }

        private IReadOnlyList<char> RemainingLetters { get; }

        private MarkEncryptor Encryptor
        {
            get => markEncryptor ?? new MarkEncryptor(RemainingLetters, KeyLetters);
        }

        private MarkDecryptor Decryptor
        {
            get => markDecryptor ?? new MarkDecryptor(RemainingLetters, KeyLetters);
        }

        public string Encrypt(string text)
        {
            if (string.IsNullOrEmpty(text))
                throw new ArgumentNullException(nameof(text));

            var letters = text.ToLower().ToCharArray();

            if (letters.Except(Alphabet).Any())
                throw new ArgumentException($"The provided {text} has invalid letters.");

            var encryptedText = Encryptor.Encrypt(letters);
            return encryptedText;
        }

        public string Decrypt(string text)
        {
            if (string.IsNullOrEmpty(text))
                throw new ArgumentNullException(text);

            var decryptedText = Decryptor.Decrypt(text);
            return decryptedText;
        }

    }
}
