using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FC
{
    public static class Utilities
    {
        public static string MessageToUserFormat => "[white]{0}[/]";
        public static string UserInputErrorFormat => "[red]{0}[/]";
        public static string UserTypingFormat => "[yellow]{0}[/]";
    }

    public static class ValidationUtilities
    {
        public static bool Empty<T>(this IEnumerable<T> sequence) => !sequence.Any();

        public static bool TxtFilePath(this string filepath)
        {
            return Path.HasExtension(filepath) && Path.GetExtension(filepath).Equals(".txt");
        }

        public static bool FilePathContainsInvalidCharacters(this string filepath)
        {
            return Path.GetInvalidPathChars().Intersect(filepath).Any();
        }

        public static bool ParentFileDirectoryExist(this string filepath) => Directory.Exists(filepath);

        public static bool ExistingFilePath(this string filepath) => File.Exists(filepath);
    }
}
