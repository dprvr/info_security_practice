
using DupsSearcher.Utils;
using System;
using System.IO;
using System.Windows.Forms;
using System.Windows.Input;


namespace DupsSearcher
{
    internal class SearchInitViewModel : BaseViewModel
    {
        private readonly ICommand _selectDirCommand;
        private readonly ICommand _selectFileCommand;
        private readonly ICommand _searchCommand; 

        private string _selectedFilePath;
        private string _selectedDirPath;
        
        public SearchInitViewModel()
        {
            _selectDirCommand = new DelegateCommand(ReadDirectory);
            _selectFileCommand = new DelegateCommand(ReadFile);
            _searchCommand = new DelegateCommand(SearchCopies, CanExecuteSearch);
        }

        private string SelectedFilePath
        {
            get => _selectedFilePath;
            set
            {
                _selectedFilePath = value ?? throw new ArgumentNullException(nameof(SelectedFilePath));
                OnPropertyChanged(nameof(SelectedFile));
            }
        }

        private string SelectedDirectoryPath
        {
            get => _selectedDirPath;
            set
            {
                _selectedDirPath = value ?? throw new ArgumentNullException(nameof(SelectedDirectoryPath));
                OnPropertyChanged(nameof(SelectedDirectory));
            }
        }

        public string SelectedFile
        {
            get => Path.GetFileName(SelectedFilePath) ?? string.Empty;
        }

        public string SelectedDirectory
        {
            get => Path.GetFileName(SelectedDirectoryPath) ?? string.Empty;
        }

        public ICommand SelectFile => _selectFileCommand;
        public ICommand SelectDirectory => _selectDirCommand;
        public ICommand Search => _searchCommand;

       
        private void ReadFile(object obj)
        {
            var fileOpenDialog = new Microsoft.Win32.OpenFileDialog()
            {
                CheckFileExists = true,
                CheckPathExists = true,
                ValidateNames = true,
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
            };
            bool? dialogResult = fileOpenDialog.ShowDialog();
            if (dialogResult.Value)
            {
                SelectedFilePath = fileOpenDialog.FileName;
            }
        }

        private void ReadDirectory(object obj)
        {
            using (FolderBrowserDialog openDirDialog = new FolderBrowserDialog()
            {
                RootFolder = Environment.SpecialFolder.Desktop,
                ShowNewFolderButton = false,
            })
            {
                DialogResult dialogResult = openDirDialog.ShowDialog();
                if (dialogResult == DialogResult.OK)
                {
                    SelectedDirectoryPath = openDirDialog.SelectedPath;
                }
            };
        }
        private bool CanExecuteSearch(object obj)
        {
            return !string.IsNullOrEmpty(_selectedDirPath) && !string.IsNullOrEmpty(_selectedFilePath);
        }
        private void SearchCopies(object obj)
        {
            CopiesSearchResult searchResult = new FileCopiesSearch(SelectedFilePath, SelectedDirectoryPath);
            if(searchResult.Error != SearchResult.Ok)
            {
                string searchBreak = "Поиск приостановлен";
                switch (searchResult.Error)
                {
                    case SearchResult.SearchDirEmptyError:
                        var res = MessageBox.Show("Указанная для поиска папка пуста.", searchBreak, MessageBoxButtons.OK);
                        break;
                    case SearchResult.SearchDirNotAccessebleError:
                        MessageBox.Show("Ошибка доступа к указанной папке - чтение запрещено.", searchBreak, MessageBoxButtons.OK);
                        break;
                    case SearchResult.SearchDirNotExistError:
                        MessageBox.Show("Похоже заданная в параметрах поиска папка уже не существует.", searchBreak, MessageBoxButtons.OK);
                        break;
                    case SearchResult.KeyFileNotAccesseble:
                        MessageBox.Show("Заданный в параметрах поиска файл недоступен для чтения.", searchBreak, MessageBoxButtons.OK);
                        break;
                    case SearchResult.KeyFileNotExist:
                        MessageBox.Show("Заданный в параметрах поиска файл перемещен или удален.", searchBreak, MessageBoxButtons.OK);
                        break;
                    case SearchResult.KeyFileReadingError:
                        MessageBox.Show("Ошибка чтения файла.", searchBreak, MessageBoxButtons.OK);
                        break;
                    case SearchResult.UnexpectedProgrammError:
                        MessageBox.Show("Произошла неизвестная ошибка, работа программы будет завершена.", "Сбой", MessageBoxButtons.OK);
                        System.Windows.Application.Current.Shutdown();
                        break;
                    case SearchResult.HandlingError:
                        MessageBox.Show("Произошла неизвестная ошибка, работа программы будет завершена.", "Сбой", MessageBoxButtons.OK);
                        System.Windows.Application.Current.Shutdown();
                        break;
                }
                return;
            }
            SearchResultsViewModel resultsVM = new SearchResultsViewModel(searchResult);
            SearchResults resultsWindow = new SearchResults()
            {
                Owner = System.Windows.Application.Current.MainWindow,
                DataContext = resultsVM,
            };
            resultsWindow.Show();
        }

    }
}
