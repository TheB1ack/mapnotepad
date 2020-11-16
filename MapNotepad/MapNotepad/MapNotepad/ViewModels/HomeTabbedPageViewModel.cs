using Acr.UserDialogs;
using MapNotepad.Services.Authorization;
using MapNotepad.Services.Permission;
using MapNotepad.Views;
using Prism.Navigation;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MapNotepad.ViewModels
{
    public class HomeTabbedPageViewModel : ViewModelBase
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IUserDialogs _userDialogs;
        private readonly IPermissionsService _permissionsService;

        public HomeTabbedPageViewModel(INavigationService navigationService,
                                       IAuthorizationService authorizationService,
                                       IUserDialogs userDialogs,
                                       IPermissionsService permissionsService)
                                       : base(navigationService)
        {
            _authorizationService = authorizationService;
            _userDialogs = userDialogs;
            _permissionsService = permissionsService;
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
            var result = await CheckCameraPermissions();

            if (result)
            {
                await _navigationService.NavigateAsync($"{nameof(QrScannerPage)}");
            }
            else
            {
                Debug.WriteLine("result was false");
            }

        }
        private async Task<bool> CheckCameraPermissions()
        {
            var isGranted = false;
            var status = await _permissionsService.CheckPermissionsAsync<Permissions.Camera>();

            if (status != PermissionStatus.Granted)
            {
                isGranted = await SetCameraPermissionsAsync();
            }
            else
            {
                isGranted = true;
            }

            return isGranted;
        }

        private async Task<bool> SetCameraPermissionsAsync()
        {
            var isGranted = false;
            var status = await _permissionsService.RequestPermissionsAsync<Permissions.Camera>();

            if (status != PermissionStatus.Granted)
            {
                string alertText = Resources.Resource.CameraAlert;
                string button = Resources.Resource.OkButton;

                await _userDialogs.AlertAsync(alertText, string.Empty, button);

                isGranted = false;
            }
            else
            {
                isGranted = true;
            }

            return isGranted;
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
