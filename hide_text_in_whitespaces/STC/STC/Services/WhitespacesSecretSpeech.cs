using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace STC.Services
{
    public class WhitespacesSecretSpeech : IHidingAlgorithm
    {
        private const string _ws = " ";
        private const string _dws = "  ";
        private const string _endToken = ">?<";
        private const string _singleWSPattern = @"\S \S";
        private const string _singleOrDoubleWSPattern = @"\S {1,2}\S";
        private const string _multipleWSPattern = @" +";

        public string HideMessage(string container, IReadOnlyList<bool> message)
        {
            if (string.IsNullOrEmpty(container))
                throw new ArgumentException($"The {nameof(container)} was null or empty.", nameof(container));
            if (message.Empty())
                throw new ArgumentException($"The {nameof(message)} was empty.");

            var cleanedContainer = RemoveMultipleWhitespaces(container);

            if (message.Count > Regex.Matches(cleanedContainer, _singleWSPattern).Count)
                throw new ArgumentException($"The {nameof(container)} size doesn't enough to hide the given {nameof(message)}, whitespaces count must be equal or less than message in bits length");

            var codedMessage = EncodeMessage(message);
            var secretContainer = ReplaceWSsWithCodes(cleanedContainer, codedMessage);
            return secretContainer;

            static string RemoveMultipleWhitespaces(string container)
            {
                return Regex.Replace(container, _multipleWSPattern, _ws);
            }

            static IReadOnlyCollection<string> EncodeMessage(IReadOnlyList<bool> message)
            {
                return message
                    .Select(bit => bit ? _dws : _ws)
                    .Append(_endToken)
                    .ToArray();
            }

            static string ReplaceWSsWithCodes(string text, IReadOnlyCollection<string> codedMessage)
            {
                var regex = new Regex(_singleWSPattern);
                var codesIterator = codedMessage.GetEnumerator();
                var modified = regex.Replace(text, ReplaceWSWithCode, codedMessage.Count, 0);
                return modified;

                string ReplaceWSWithCode(Match match)
                {
                    codesIterator.MoveNext();
                    var coded = match.Value.Replace(_ws, codesIterator.Current);
                    return coded;
                }
            }
        }

        public IReadOnlyList<bool> ExtractMessage(string container)
        {
            if (string.IsNullOrEmpty(container))
                throw new ArgumentException($"The {container} was null or empty.", nameof(container));
            if (Regex.Matches(container, _singleOrDoubleWSPattern).Count == 0)
                throw new ArgumentException($"The given text-container doesn't have any secret message coded with whitespases coding method");

            var codedContainerPart = GetCodedPartOfContainer(container);
            var codedMessage = ExtractCodedMessage(codedContainerPart);
            var message = DecodeMessage(codedMessage);
            return message;

            static string GetCodedPartOfContainer(string container)
            {
                var endTokenPos = container.IndexOf(_endToken);
                var messageSource = endTokenPos < 0
                    ? container
                    : container.Substring(0, endTokenPos + 1);
                return messageSource;
            }

            static IEnumerable<string> ExtractCodedMessage(string container)
            {
                var matches = Regex.Matches(container, _singleOrDoubleWSPattern);
                var codedMessage = matches.Select(m => m.Value.RemoveFirstChar().RemoveLastChar());
                return codedMessage;
            }

            static IReadOnlyList<bool> DecodeMessage(IEnumerable<string> codedMessage)
            {
                return codedMessage
                    .Select(code => code.Equals(_dws))
                    .ToArray();
            }
        }
    }
}
