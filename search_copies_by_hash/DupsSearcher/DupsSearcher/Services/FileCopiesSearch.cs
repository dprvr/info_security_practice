using DupsSearcher.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.Cryptography;

namespace DupsSearcher
{
    enum SearchResult
    {
        HandlingError,
        Ok,
        UnexpectedProgrammError,    
        KeyFileNotExist,
        KeyFileNotAccesseble,
        KeyFileReadingError,        
        SearchDirNotExistError,
        SearchDirNotAccessebleError,
        SearchDirEmptyError,
    }

    public enum FileHandlingError
    {
        FileReadingError,
        FileAccessError,
        FileNotExistError,
    }

    internal class FileCopiesSearch
    {
        private readonly string _filePath;
        private readonly string _folderPath;

        private SearchResult _searchError;
        private string _keyHash;
        private bool _searchProcessed;
        private FileInfo[] _topDirFiles;
        private FileInfo[] _subDirsFiles;

        private List<(FileInfo, FileHandlingError)> _unhandledFiles;
        private List<(FileInfo file, string hash)> _handledFiles;
        private (FileInfo file, string hash)[] _copies;

        public FileCopiesSearch(string filePath, string folderPath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentException($"\"{nameof(filePath)}\" не может быть неопределенным или пустым.", nameof(filePath));
            }

            if (string.IsNullOrEmpty(folderPath))
            {
                throw new ArgumentException($"\"{nameof(folderPath)}\" не может быть неопределенным или пустым.", nameof(folderPath));
            }

            _copies = Array.Empty<(FileInfo, string)>();
            _unhandledFiles = new List<(FileInfo, FileHandlingError)>();
            _handledFiles = new List<(FileInfo, string)>();
            _filePath = filePath;
            _folderPath = folderPath;
        }

        private CopiesSearchResult SearchResults
        {
            get
            {
                if (!_searchProcessed)
                {
                    ProcessSearch();
                }

                return new CopiesSearchResult()
                {
                    Copies = _copies.Select(t => t.file).ToArray(),
                    Error = _searchError,
                    SearchedDirPath = _folderPath,
                    SearchedFilePath = _filePath,
                    Unhandled = _unhandledFiles.ToArray(),
                };
            }
        }

        public static implicit operator CopiesSearchResult(FileCopiesSearch search)
        {
            if (search is null)
                throw new ArgumentNullException(nameof(search));
            return search.SearchResults;
        }

        private void ProcessSearch()
        {
            try
            {
                _searchProcessed = TryComputeKeyHash()
                && TryGetFolderFiles()
                && SearchFolderContainsFiles()
                && TryComputeTopDirFilesHash()
                && TryComputeSubDirsHash()
                && FilesHandled()
                && FindFilesCopies();
            }
            catch
            {
                _searchError = SearchResult.UnexpectedProgrammError;
            }
            _searchProcessed = true;
        }

        private bool TryComputeKeyHash()
        {
            bool computed = false;
            try
            {
                FileInfo file = new FileInfo(_filePath);
                _keyHash = ComputeHash(file);
                computed = true;
            }
            catch (IOException ex) when (ex is DirectoryNotFoundException || ex is FileNotFoundException)
            {
                _searchError = SearchResult.KeyFileNotExist;
            }
            catch (IOException)
            {
                _searchError = SearchResult.KeyFileReadingError;
            }
            catch (SystemException ex) when (ex is SecurityException || ex is UnauthorizedAccessException)
            {
                _searchError = SearchResult.KeyFileNotAccesseble;
            }
            catch (ObjectDisposedException)
            {
                _searchError = SearchResult.KeyFileNotExist;
            }
            return computed;
        }

        private bool TryGetFolderFiles()
        {
            bool got = false;
            try
            {
                DirectoryInfo dir = new DirectoryInfo(_folderPath);
                _topDirFiles = dir.GetFiles("*", SearchOption.TopDirectoryOnly).ToArray();
                _subDirsFiles = dir.GetFiles("*", SearchOption.AllDirectories).Except(_topDirFiles, new FileInfoComparer()).ToArray();
                got = true;
            }
            catch(SecurityException)
            {
                _searchError = SearchResult.SearchDirNotAccessebleError;
            }
            catch (UnauthorizedAccessException)
            {
                _searchError = SearchResult.SearchDirNotAccessebleError;
            }
            catch (DirectoryNotFoundException)
            {
                _searchError = SearchResult.SearchDirNotExistError;
            }
            return got;
        }

        private bool SearchFolderContainsFiles()
        {
            bool contains = _topDirFiles.Length > 0 || _subDirsFiles.Length > 0;
            if(!contains)
            {
                _searchError = SearchResult.SearchDirEmptyError;
            }
            return contains;
        }

        private bool TryComputeTopDirFilesHash()
        {
            bool computed = false;
            foreach (FileInfo file in _topDirFiles)
            {
                try
                {
                    string hash = ComputeHash(file);
                    _handledFiles.Add((file, hash));
                    computed = true;
                }
                catch (DirectoryNotFoundException)
                {
                    _searchError = SearchResult.SearchDirNotExistError;
                    break;
                }
                catch (IOException)
                {
                    _unhandledFiles.Add((file, FileHandlingError.FileReadingError));
                }
                catch (UnauthorizedAccessException)
                {
                    _unhandledFiles.Add((file, FileHandlingError.FileAccessError));
                }
                catch (ObjectDisposedException)
                {
                    _unhandledFiles.Add((file, FileHandlingError.FileReadingError));
                }
            }
            return computed;
        }

        private bool TryComputeSubDirsHash()
        {
            bool computed = false;
            foreach (FileInfo file in _subDirsFiles)
            {
                try
                {
                    string hash = ComputeHash(file);
                    _handledFiles.Add((file, hash));
                    computed = true;
                }
                catch (IOException ex) when (ex is FileNotFoundException || ex is DirectoryNotFoundException)
                {
                    _unhandledFiles.Add((file, FileHandlingError.FileNotExistError));
                }
                catch (IOException)
                {
                    _unhandledFiles.Add((file, FileHandlingError.FileReadingError));
                }
                catch (ObjectDisposedException)
                {
                    _unhandledFiles.Add((file, FileHandlingError.FileReadingError));
                }
                catch (UnauthorizedAccessException)
                {
                    _unhandledFiles.Add((file, FileHandlingError.FileAccessError));
                }
            } 
            return computed;
        }

        private bool FilesHandled()
        {
            if (_handledFiles.Count == 0)
            {
                _searchError = SearchResult.HandlingError;
                return false;
            }
            return true;
        }

        private bool FindFilesCopies()
        {
            _copies = _handledFiles
                    .Where(t => t.hash.Equals(_keyHash))
                    .ToArray();
            _searchError = SearchResult.Ok;
            return true;
        }

        private string ComputeHash(FileInfo file)
        {
            using (var stream = file.OpenRead())
            {
                using (var sha = new SHA256Managed())
                {
                    return Convert.ToBase64String(sha.ComputeHash(stream));
                }
            }
        }

    }

    internal class CopiesSearchResult
    {
        public string SearchedFilePath { get; set; }
        public string SearchedDirPath { get; set; }
        public IReadOnlyCollection<FileInfo> Copies { get; set; }
        public IReadOnlyCollection<(FileInfo, FileHandlingError)> Unhandled { get; set; }
        public SearchResult Error { get; set; }
    }

}
