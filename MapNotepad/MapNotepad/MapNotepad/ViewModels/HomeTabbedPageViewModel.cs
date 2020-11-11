using Acr.UserDialogs;
using MapNotepad.Services.Authorization;
using MapNotepad.Views;
using Prism.Navigation;
using System.Diagnostics;
using System.Windows.Input;
using Xamarin.Forms;

namespace MapNotepad.ViewModels
{
    public class HomeTabbedPageViewModel : ViewModelBase
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IUserDialogs _userDialogs;

        public HomeTabbedPageViewModel(INavigationService navigationService, 
                                       IAuthorizationService authorizationService,
                                       IUserDialogs userDialogs) 
                                       : base(navigationService)
        {
            _authorizationService = authorizationService;
            _userDialogs = userDialogs;          
        }

        #region -- Public properties --

        private ICommand _logOutClickCommand;
        public ICommand LogOutClickCommand => _logOutClickCommand ??= new Command(OnLogOutClickCommandAsync);

        private ICommand _qrClickCommand;
        public ICommand qrClickCommand => _qrClickCommand ??= new Command(OnqrClickCommand);

        #endregion

        #region -- Private helpers --

        private async void OnqrClickCommand()
        {
            await _navigationService.NavigateAsync($"{nameof(QrScannerPage)}");
        }

        private async void OnLogOutClickCommandAsync()
        {
            var text = Resources.Resource.UserLogoutAlert;
            var okText = Resources.Resource.ContinueButton;
            var cancelText = Resources.Resource.CancelButton;
            var isConfirmed = await _userDialogs.ConfirmAsync(text, string.Empty, okText, cancelText);

            if (isConfirmed)
            {
                _authorizationService.LogOut();
                await _navigationService.NavigateAsync($"/{nameof(NavigationPage)}/{nameof(SingInPage)}");
            }

        }

        #endregion
    }
}
