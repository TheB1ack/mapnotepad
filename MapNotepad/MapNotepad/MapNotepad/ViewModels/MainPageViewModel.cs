using MapNotepad.Services.Authorization;
using MapNotepad.Views;
using Prism.Navigation;
using System.Windows.Input;
using Xamarin.Forms;

namespace MapNotepad.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private readonly IAuthorizationService _authorizationService;       

        public MainPageViewModel(INavigationService navigationService, IAuthorizationService authorizationService) : base(navigationService)
        {
            _authorizationService = authorizationService;
        }

        #region -- Public properties --
        public ICommand LogOutClick => new Command(LogOutAsync);
        #endregion

        #region -- Private helpers --
        private async void LogOutAsync()
        {
            _authorizationService.LogOut();
            await _navigationService.NavigateAsync($"../{nameof(SingInPage)}");
        }
        #endregion
    }
}
