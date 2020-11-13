using Prism.Mvvm;
using Prism.Navigation;

namespace MapNotepad.ViewModels
{
    public class ViewModelBase : BindableBase, IInitialize, INavigationAware, IDestructible
    {        
        public ViewModelBase(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        protected INavigationService _navigationService { get; private set; }

        #region -- IInitialize implementation --

        public virtual void Initialize(INavigationParameters parameters)
        {

        }

        #endregion

        #region -- INavigationAware implementation --

        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {

        }

        #endregion

        #region -- IDestructible implementation --

        public virtual void Destroy()
        {

        }

        #endregion

    }
}
