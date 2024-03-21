using EC.ViewModels;

namespace EC.Util
{
    internal class Navigator
    {
        private readonly MainWindowViewModel _main;

        internal Navigator(MainWindowViewModel main)
        {
            _main = main;
        }

        internal void NavigateToRegistration()
        {
            _main.SetView(new RegistrationViewModel(this));
        }

        internal void NavigateToEnter()
        {
            _main.SetView(new EnterViewModel(this));
        }

        internal void NavigateToUserData(string username, string pathToUserData)
        {
            _main.SetView(new UserDataViewModel(this, username, pathToUserData));
        }
    }
}
