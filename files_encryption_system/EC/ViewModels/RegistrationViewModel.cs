using EC.Core;
using EC.Util;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace EC.ViewModels
{
    internal class RegistrationViewModel : PageViewModel
    {
        public string Login { get; set; }
        public ICommand Return { get; }
        public ICommand Register { get; }
        public ICommand ReturnToHomePage { get; }

       
        internal RegistrationViewModel(Navigator navigator) : base(navigator)
        {
            Login = string.Empty;
            Return = new DelegateCommand(ReturnToMain);
            Register = new DelegateCommand(RegisterNewUser, CanRegister);
            ReturnToHomePage = new DelegateCommand(ReturnToMain);
        }

        private bool CanRegister(object? obj)
        {
            if (string.IsNullOrEmpty(Login))
                return false;
            var ps = (object[])obj;
            var password1 = ps[0] as PasswordBox;
            var password2 = ps[1] as PasswordBox;
            if (password1 is null || password2 is null)
                return false;
            if (string.IsNullOrEmpty(password1.Password) || string.IsNullOrEmpty(password2.Password))
                return false;
            return true;
        }

        private void RegisterNewUser(object? obj)
        {
            var ps = (object[])obj;
            var password1 = ps[0] as PasswordBox;
            var password2 = ps[1] as PasswordBox;
            if(!password1.Password.Equals(password2.Password))
            {
                MessageBox.Show("Введенные пароли не совпадают.");
                return;
            }
            if (AppService.Instance.UserExist(Login))
            {
                MessageBox.Show("Пользователь с таким именем уже существует.");
                return;
            }
            string password = password1.Password;
            AppService.Instance.CreateNewUser(Login, password);
            MessageBox.Show("Создание аккаунта прошло успешно.");
            Navigator.NavigateToEnter();
        }

        private void ReturnToMain(object? obj)
        {
            Navigator.NavigateToEnter();
        }

    }
}
