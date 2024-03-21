using EC.Util;
using System;
using System.Windows.Controls;

namespace EC.ViewModels
{
    internal class MainWindowViewModel : BaseViewModel
    {
        private Page _curPage;
        

        public MainWindowViewModel()
        {
            _curPage = new HomePage { DataContext = new EnterViewModel(Navigator)};
        }

        private Navigator Navigator
        {
            get
            {
                return new Navigator(this);
            }
        }

        public Page CurPage
        {
            get => _curPage;
            set
            {
                _curPage = value;
                NotifyThatPropertyChanged();
            }
        }

        internal void SetView(BaseViewModel viewModel)
        {
            CurPage = viewModel switch
            {
                EnterViewModel enter => new HomePage { DataContext = enter },
                RegistrationViewModel registration => new RegistrationPage { DataContext = registration},
                UserDataViewModel user => new UserPage {  DataContext = user },
                _ => throw new ArgumentException(null, nameof(viewModel)),                    
            };
        }

    }
}
