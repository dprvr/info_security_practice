using System;
using System.Collections.Generic;
using System.IO;

namespace DupsSearcher.Utils
{
    public class FileInfoComparer : IEqualityComparer<FileInfo>
    {
        public bool Equals(FileInfo x, FileInfo y)
        {
            if (x is null || y is null)
            {
                return x is null && y is null;
            }

            return x.FullName == y.FullName;
        }

        public int GetHashCode(FileInfo obj)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            return obj.FullName.GetHashCode();
        }
    }
}
