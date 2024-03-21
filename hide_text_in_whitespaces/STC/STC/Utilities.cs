using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace STC
{
    public static class ConvertUtilities
    {
        public static IEnumerable<bool> ToBits(this byte[] bytes) => new BitArray(bytes).Cast<bool>();

        public static byte[] ToBytes(this IEnumerable<bool> bits)
        {
            int bytesCount = bits.Count() / 8;
            var bts = new byte[bytesCount];
            var bas = new BitArray(bits.Take(8 * bytesCount).ToArray());
            bas.CopyTo(bts, 0);
            return bts;
        }
    }

    public static class StringUtilities
    {
        public static string RemoveFirstChar(this string str) => str.Remove(0, 1);

        public static string RemoveLastChar(this string str) => str.Remove(str.Length - 1, 1);

        public static string BuildUserErrorMessage(this string message) => $"ERROR:\nIt seems like you input invalid command parameters:\t\n{message}\n.Please check that input parameters are correct and try again.";


    }

    public static class ValidationUtilities
    {
        public static bool Empty<T>(this IEnumerable<T> sequence) => !sequence.Any();

        public static bool TxtFilePath(this string filepath)
        {
            return Path.HasExtension(filepath)
                && Path.GetExtension(filepath).Equals("txt");
        }

        public static bool FilePathContainsInvalidCharacters(this string filepath)
        {
            return Path.GetInvalidPathChars().Intersect(filepath).Any();
        }

        public static bool ParentFileDirectoryExist(this string filepath) => Directory.Exists(filepath);

        public static bool ExistingFilePath(this string filepath) => File.Exists(filepath);
    }
}
