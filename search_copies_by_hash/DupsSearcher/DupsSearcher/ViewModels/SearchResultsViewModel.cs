using DupsSearcher.Utils;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DupsSearcher
{
    internal class SearchResultsViewModel : BaseViewModel
    {
        private readonly string _searchedFilePath;
        private readonly string _searchFolderPath;
        private readonly IReadOnlyCollection<FileInfo> _copies;
        private readonly IReadOnlyCollection<(FileInfo, FileHandlingError)> _unhandled;

        public SearchResultsViewModel(CopiesSearchResult copiesSearchResult)
        {
            _searchedFilePath = copiesSearchResult.SearchedFilePath;
            _searchFolderPath = copiesSearchResult.SearchedDirPath;
            _copies = copiesSearchResult.Copies;
            _unhandled = copiesSearchResult.Unhandled;
        }

        public string SearchedFolderPath
        {
            get => _searchFolderPath;
        }

        public string SearchedFilePath
        {
            get => _searchedFilePath;
        }

        public bool UnhandledVisible
        {
            get => Unhandled.Count == 0
                ? false
                : true;
        }

        public string UnhandledFilesMessage
        {
            get
            {
                StringBuilder message = new StringBuilder();
                if (Unhandled.Count == 1)
                {
                    message.AppendLine($"Во время поиска не удалось обработать 1 файл:");
                }
                else if (Unhandled.Count > 1 && Unhandled.Count < 5)
                {
                    message.AppendLine($"Во время поиска не удалось обработать {Unhandled.Count} файла:");
                }
                else if(Unhandled.Count > 4)
                {
                    message.AppendLine($"Во время поиска не удалось обработать {Unhandled.Count} файлов:");
                }
                return message.ToString();
            }
        }

        public string CopiesFoundMessage
        {
            get
            {
                StringBuilder message = new StringBuilder();
                if(Copies.Count == 0)
                {
                    message.AppendLine($"Копий файла {SearchedFilePath} в папке {SearchedFolderPath} не найдено.");
                }
                else if(Copies.Count == 1)
                {
                    message.AppendLine($"В папке {SearchedFolderPath} была найдена 1 копия файла {SearchedFilePath}:");
                }
                else if(Copies.Count > 1 && Copies.Count < 5)
                {
                    message.AppendLine($"В папке {SearchedFolderPath} было найдено {Copies.Count} копии файла {SearchedFilePath}:");
                }
                else
                {
                    message.AppendLine($"В папке {SearchedFolderPath} было найдено {Copies.Count} копий файла {SearchedFilePath}:");
                }
                return message.ToString();
            }
        }

        public IReadOnlyCollection<object> Unhandled
        {
            get
            {
                
                return _unhandled
                    .Select(t => new { Name = t.Item1.Name, Path = t.Item1.FullName, Error = MapMessage(t.Item2)})
                    .ToArray();

                string MapMessage(FileHandlingError error)
                {
                    string message = string.Empty;
                    switch (error)
                    {
                        case FileHandlingError.FileReadingError:
                            message = "Ошибка чтения файла.";
                            break;
                        case FileHandlingError.FileAccessError:
                            message = "Ошибка доступа к файлу - нет прав на чтение файла.";
                            break;
                        case FileHandlingError.FileNotExistError:
                            message = "Видимо файл был перемещен или удален сторонним процессом во время поиска.";
                            break;
                        default:
                            break;
                    }
                    return message;
                }
            }
        }

        public IReadOnlyCollection<FileInfo> Copies
        {
            get => _copies;
        }

        public bool CopiesVisible 
        {
            get => _copies.Count > 0;
        }
    }
}
