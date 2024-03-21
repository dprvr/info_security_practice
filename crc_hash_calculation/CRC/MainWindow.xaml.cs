using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CRC
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _message = string.Empty;
        private string _choosenFile = string.Empty;

        public MainWindow()
        {
            InitializeComponent();
        }

        private string ChoosenFile
        {
            get => _choosenFile;
            set
            {
                if(!string.IsNullOrEmpty(value))
                {
                    _choosenFile = value;
                    FileInputBox.Text = System.IO.Path.GetFileName(value);
                }
            }
        }

        private string Message
        {
            get => _message;

            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _message = value;
                    OutputBox.Text = _message;
                }
            }
        }
        
        private void ChooseFileButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                CheckFileExists = true,
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                Multiselect = false,
                Title = "Выберите файл",
            };
            var dialogResult = dialog.ShowDialog();
            if (dialogResult.HasValue && dialogResult.Value)
            {
                ChoosenFile = dialog.FileName;
            }
        }

        private void ComputeCRCButton_Click(object sender, RoutedEventArgs e)
        {
            CRCComputingResult result = new FileCRCComputation(_choosenFile);
            switch (result.ComputingStatus)
            {
                case CRCComputingStatus.UnexpectedError:
                    Message = "Во время работы программы произошла неизвестная ошибка.";
                    break;
                case CRCComputingStatus.ComputedSuccess:
                    Message = $"CRC8 Был успешно рассчитан. CRC = [{result.CRCHashSum}].";
                    break;
                case CRCComputingStatus.FileAccessError:
                    Message = "Похоже у вас нет прав доступа на чтение выбранного файла.";
                    break;
                case CRCComputingStatus.FileNotExistError:
                    Message = "Выбранный файл был перемещен или удален.";
                    break;
                case CRCComputingStatus.FileReadingError:
                    Message = "Ошибка чтения файла.";
                    break;
            }

        }

    }
}
