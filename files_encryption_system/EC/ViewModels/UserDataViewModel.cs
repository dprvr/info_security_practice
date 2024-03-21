using EC.Core;
using EC.Util;
using System.Diagnostics;
using System.Windows.Input;

namespace EC.ViewModels
{
    internal class UserDataViewModel : PageViewModel
    {
        public string UserName { get; }
        public ICommand LogOff { get; }
        public ICommand OpenUserDir { get; }

        private string UserDirPath { get; }

        public UserDataViewModel(Navigator navigator, string username, string pathToUserData) : base(navigator)
        {
            UserName = username;
            UserDirPath = pathToUserData;
            OpenUserDir = new DelegateCommand(OpenDirInExplorer);
            LogOff = new DelegateCommand(LogOut);
        }

        private void LogOut(object? obj)
        {
            AppService.Instance.LogoutUser(UserName);
            Navigator.NavigateToEnter();
        }

        private void OpenDirInExplorer(object? param)
        {
            Process.Start("explorer.exe", UserDirPath);
        }

    }
}
