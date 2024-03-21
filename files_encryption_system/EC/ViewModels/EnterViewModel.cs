using EC.Core;
using EC.Util;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace EC.ViewModels
{
    internal class EnterViewModel : PageViewModel
    {
        public string Login { get; set; }
        
        public ICommand Enter { get; }

        public ICommand Register { get; }

        public EnterViewModel(Navigator navigator) : base(navigator)
        {
            Login = string.Empty;
            Enter = new DelegateCommand(TryEnter, CanTryEnter);
            Register = new DelegateCommand(GoToRegister);
        }

        private void GoToRegister(object? obj)
        {
            Navigator.NavigateToRegistration();
        }

        private bool CanTryEnter(object? parameter)
        {
            if (string.IsNullOrEmpty(Login))
                return false;
            var box = parameter as PasswordBox;
            if (box is null)
                return false;
            if (string.IsNullOrEmpty(box.Password))
                return false;
            return true;
        }

        private void TryEnter(object? parameter)
        {
            var box = parameter as PasswordBox;
            string password = box.Password;
            if (!AppService.Instance.TryLogin(Login, password, out string pathToUserData))
            {
                MessageBox.Show("Невозможно войти в приложение с введенными данными. Неправильный логин и/или пароль.", "Ошибка");
                return;
            }
            Navigator.NavigateToUserData(Login, pathToUserData);
        }
    }
}
