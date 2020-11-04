using Acr.UserDialogs;
using MapNotepad.Services.Authorization;
using MapNotepad.Views;
using Prism.Navigation;
using System.Diagnostics;
using System.Windows.Input;
using Xamarin.Forms;

namespace MapNotepad.ViewModels
{
    public class SingInPageViewModel : ViewModelBase
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IUserDialogs _userDialogs;

        public SingInPageViewModel(INavigationService navigationService, 
                                   IAuthorizationService authorizationService, 
                                   IUserDialogs userDialogs) 
                                   : base(navigationService)
        {
            _userDialogs = userDialogs;
            _authorizationService = authorizationService;
        }

        #region -- Public properties --

        private string _emailEntry;
        public string EmailEntry
        {
            get => _emailEntry;

            set => SetProperty(ref _emailEntry, value);
        }
        private string _passwordEntry;
        public string PasswordEntry
        {
            get => _passwordEntry;

            set => SetProperty(ref _passwordEntry, value);
        }
        private bool _isButtonEnable;
        public bool IsButtonEnable
        {
            get => _isButtonEnable;

            set => SetProperty(ref _isButtonEnable, value);
        }

        private ICommand _signInButtonClickCommand;
        public ICommand SignInButtonClickCommand => _signInButtonClickCommand ??= new Command(OnSignInButtonClickCommandAsync);

        private ICommand _createAccountButtonClickCommand;
        public ICommand CreateAccountButtonClickCommand => _createAccountButtonClickCommand ??= new Command(OnCreateAccountButtonClickCommandAsync);

        private ICommand _textChangedCommand;
        public ICommand TextChangedCommand => _textChangedCommand ??= new Command(OnTextChangedCommand);

        #endregion

        #region -- IterfaceName implementation --

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if(parameters.TryGetValue(Constants.EMAIL, out string email))
            {
                EmailEntry = email;
            }
            else
            {
                Debug.WriteLine("Parameters are missing String");
            }
        }

        #endregion

        #region -- Private helpers --

        private void OnTextChangedCommand()
        {
            IsButtonEnable = TryActivateButton();
        }
        private bool TryActivateButton()
        {
            var isActive = true;

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
        private async void OnSignInButtonClickCommandAsync()
        {
            var isSignedIn = await _authorizationService.SignInAsync(EmailEntry, PasswordEntry);
            
            if (isSignedIn)
            {
                await _navigationService.NavigateAsync($"/{nameof(NavigationPage)}/{nameof(MainPage)}");
            }
            else
            {
                string alertText = Resources.Resource.BadPasswordEmailAlert;
                string button = Resources.Resource.OkButton;

                await _userDialogs.AlertAsync(alertText, string.Empty, button);  
                PasswordEntry = string.Empty;
            }
        }
        private async void OnCreateAccountButtonClickCommandAsync()
        {
            await _navigationService.NavigateAsync($"{nameof(SingUpPage)}");
        }

        #endregion

    }
}
