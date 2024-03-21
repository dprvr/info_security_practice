using System.Collections.Generic;
using System.Text;

namespace FC.Algo
{
    public partial class MarkCryptor
    {
        private sealed class MarkDecryptor
        {
            private readonly Dictionary<string, char> _codeToLetterMap;

            public MarkDecryptor(IReadOnlyList<char> remainingAlphabet, IReadOnlyList<char> key)
            {
                _codeToLetterMap = BuildCodesMap();

                Dictionary<string, char> BuildCodesMap()
                {
                    var dict = new Dictionary<string, char>();
                    for (int i = 0; i < key.Count; i++)
                    {
                        dict.Add((i + 1).ToString(), key[i]);
                    }

                    var lettersIterator = remainingAlphabet.GetEnumerator();
                    var indexes = new string[] { "8", "9", "0"};
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 9; j++)
                        {
                            if (!lettersIterator.MoveNext())
                                break;

                            dict.Add(indexes[i] + (j + 1).ToString(), lettersIterator.Current);
                        }

                        if (!lettersIterator.MoveNext())
                            break;

                        dict.Add(indexes[i] + "0", lettersIterator.Current);
                    }
                    return dict;
                }

            }

            public string Decrypt(string codes)
            {
                var sb = new StringBuilder();
                var codesIterator = codes.GetEnumerator();

                while (codesIterator.MoveNext())
                {
                    var curCode = codesIterator.Current.ToString();

                    if(_codeToLetterMap.TryGetValue(curCode, out char letter))
                    {
                        sb.Append(letter);
                    }
                    else
                    {
                        if (!codesIterator.MoveNext())
                            throw new MarkDecodingException($"The message: {codes} can't be fully decoded with this key. Was succesfully decoded: {sb}. The invalid code was: {curCode}.");
                        var nextCode = codesIterator.Current.ToString();
                        
                        if (_codeToLetterMap.TryGetValue(curCode + nextCode, out char decLetter))
                            sb.Append(decLetter);
                        else
                            throw new MarkDecodingException($"The message: {codes} can't be fully decoded with this key. Was succesfully decoded: {sb}. The invalid code was: {curCode + nextCode}.");
                    }
                        
                }

                return sb.ToString();
            }
        }
    }
}
