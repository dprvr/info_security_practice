using EC.Util;

namespace EC.ViewModels
{
    internal abstract class PageViewModel : BaseViewModel
    {
        internal Navigator Navigator { get; init; }

        protected PageViewModel(Navigator navigator)
        {
            Navigator = navigator;
        }
    }
}
