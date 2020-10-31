using Acr.UserDialogs;
using MapNotepad.Services.Authorization;
using MapNotepad.Views;
using Prism.Navigation;
using System.Windows.Input;
using Xamarin.Forms;

namespace MapNotepad.ViewModels
{
    public class SingInPageViewModel : ViewModelBase
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IUserDialogs _userDialogs;

        public SingInPageViewModel(INavigationService navigationService, IAuthorizationService authorizationService, IUserDialogs userDialogs) : base(navigationService)
        {
            _userDialogs = userDialogs;
            _authorizationService = authorizationService;
        }

        #region -- Public properties --

        private string _emailEntry;
        public string EmailEntry
        {
            get
            {
                return _emailEntry;
            }
            set
            {
                SetProperty(ref _emailEntry, value);
                IsButtonEnable = TryActivateButton();
            }
        }
        private string _passwordEntry;
        public string PasswordEntry
        {
            get
            {
                return _passwordEntry;
            }
            set
            {
                SetProperty(ref _passwordEntry, value);
                IsButtonEnable = TryActivateButton();
            }
        }
        private bool _isButtonEnable;
        public bool IsButtonEnable
        {
            get
            {
                return _isButtonEnable;
            }
            set
            {
                SetProperty(ref _isButtonEnable, value);
            }
        }

        public ICommand SingInBClick => new Command(TryToSignInAsync);
        public ICommand CreateAccountBClick => new Command(NavigateToSignUpPageAsync);

        #endregion

        #region -- IterfaceName implementation --

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            EmailEntry = (string)parameters["email"];
        }

        #endregion

        #region -- Private helpers --

        private bool TryActivateButton()
        {
            bool isActive = true;
            if (string.IsNullOrWhiteSpace(EmailEntry))
            {
                isActive = false;
            }
            if (string.IsNullOrWhiteSpace(PasswordEntry))
            {
                isActive = false;
            }

            return isActive;
        }
        private async void TryToSignInAsync()
        {
            bool isSignedIn = await _authorizationService.SignInAsync(EmailEntry, PasswordEntry);
            if (isSignedIn)
            {
                await _navigationService.NavigateAsync($"../{nameof(MainPage)}");
            }
            else
            {
                _userDialogs.Alert("Incorrect password or email!", "", "OK");
                PasswordEntry = "";
            }
        }
        private async void NavigateToSignUpPageAsync()
        {
            await _navigationService.NavigateAsync($"{nameof(SingUpPage)}");
        }

        #endregion

    }
}
