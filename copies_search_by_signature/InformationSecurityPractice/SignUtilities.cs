using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace InformationSecurityPractice
{
    public static class SignUtilities
    {
        public static string CastToString<T>(this IEnumerable<T> seq, string sep = " ")
        {
            return seq.Aggregate(new StringBuilder(), (sb, next) => sb.Append($"{next}{sep}"),
                b => b.Remove(b.Length - sep.Length, sep.Length).ToString());
        }

        public static string GetPathToCurProject()
        {
            var workingDir = Environment.CurrentDirectory;
            var projectDir = Directory.GetParent(workingDir).Parent.Parent.Parent.FullName;
            return projectDir;
        }

        public static byte[] ReadSignature(this FileInfo file, int signatureLen, 
            (int len, SeekOrigin from) offset)
        {
            ValidateInput();
            byte[] block = new byte[signatureLen];
            using (FileStream fs = file.OpenRead())
            {
                fs.Seek(offset.len, offset.from);
                fs.Read(block, 0, signatureLen);
            }
            return block;

            void ValidateInput()
            {
                _ = file ?? 
                    throw new ArgumentNullException($"The {nameof(file)} was null");
                if (signatureLen <= 0)
                    throw new ArgumentException("The signature length must be bigger than 0");
                if (!file.Exists)
                    throw new ArgumentException($"The file({nameof(file)}) doesn't exist");
            }
        }

        public static IEnumerable<FileInfo> GetAllFiles(this DirectoryInfo dir, [Optional]string searchPattern)
        {
            string pattern = string.IsNullOrEmpty(searchPattern)? "*.*" : searchPattern;
            return dir.GetFiles(pattern, SearchOption.AllDirectories);
        }

        public static IEnumerable<FileInfo> SearchBySignature(this IEnumerable<FileInfo> files, byte[] signature,
            (int len, SeekOrigin from) offset)
        {
            return (from f in files
                   let s = ReadSignature(f, signature.Length, offset)
                   where signature.SequenceEqual(s)
                   select f).ToList();
        }

    }
}
