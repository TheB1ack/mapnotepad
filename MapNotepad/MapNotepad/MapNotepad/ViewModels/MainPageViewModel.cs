using Acr.UserDialogs;
using MapNotepad.Services.Authorization;
using MapNotepad.Views;
using Prism.Navigation;
using System.Diagnostics;
using System.Windows.Input;
using Xamarin.Forms;
using ZXing.Mobile;

namespace MapNotepad.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IUserDialogs _userDialogs;

        public MainPageViewModel(INavigationService navigationService, 
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
            //var scanner = new MobileBarcodeScanner();
            await _navigationService.NavigateAsync($"{nameof(QrScannerPage)}");
            //var result = await scanner.Scan();
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
            else
            {
                Debug.WriteLine("isConfirmed was false");
            }

        }

        #endregion
    }
}
