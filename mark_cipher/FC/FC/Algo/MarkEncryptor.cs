using System.Collections.Generic;
using System.Linq;

namespace FC.Algo
{
    public partial class MarkCryptor
    {
        private sealed class MarkEncryptor
        {
            private const int maxRowsCount = 3;
            private const int rowSize = 10;

            public MarkEncryptor(IReadOnlyList<char> remainingAlphabet, IReadOnlyList<char> key)
            {
                LetterToCodeMap = BuildCodesMap(remainingAlphabet, key);
            }

            private static IDictionary<char, string> BuildCodesMap(IReadOnlyList<char> remainingAlphabet, IReadOnlyList<char> key)
            {
                var letterToCodeMap = new Dictionary<char, string>();

                for (int i = 0; i < key.Count; i++)
                    letterToCodeMap.Add(key[i], (i + 1).ToString());

                var prefs = new string[] { "8", "9", "0" };
                var remainsIter = remainingAlphabet.GetEnumerator();
                for (int i = 0; i < maxRowsCount; i++)
                {
                    for (int j = 0; j < rowSize - 1; j++)
                    {
                        if (!remainsIter.MoveNext())
                            break;

                        letterToCodeMap.Add(remainsIter.Current, $"{prefs[i]}{j + 1}");
                    }

                    if (!remainsIter.MoveNext())
                        break;

                    letterToCodeMap.Add(remainsIter.Current, $"{prefs[i]}0");
                }
                return letterToCodeMap;
            }

            private IDictionary<char, string> LetterToCodeMap { get; }

            public string Encrypt(IReadOnlyList<char> word)
            {
                var encryptedLetters = word.Select(l => LetterToCodeMap[l]).ToArray();
                return string.Concat(encryptedLetters);
            }
        }
    }
}
