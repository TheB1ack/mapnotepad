using Acr.UserDialogs;
using MapNotepad.Services.Authorization;
using MapNotepad.Validators;
using Prism.Navigation;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MapNotepad.ViewModels
{
    public class SingUpPageViewModel : ViewModelBase
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IUserDialogs _userDialogs;
       
        public SingUpPageViewModel(INavigationService navigationService, 
                                   IAuthorizationService authorizationService, 
                                   IUserDialogs userDialogs) 
                                   : base(navigationService)
        {
            _authorizationService = authorizationService;
            _userDialogs = userDialogs;

            IsUsernameValid = true;
            IsEmailValid = true;
            IsPasswordValid = true;
            IsSPasswordValid = true;
        }

        #region -- Public properties --

        private bool _isUsernameValid;
        public bool IsUsernameValid
        {
            get => _isUsernameValid;

            set => SetProperty(ref _isUsernameValid, value);
        }

        private bool _isEmailValid;
        public bool IsEmailValid
        {
            get => _isEmailValid;

            set => SetProperty(ref _isEmailValid, value);
        }

        private bool _isPasswordValid;
        public bool IsPasswordValid
        {
            get => _isPasswordValid;

            set => SetProperty(ref _isPasswordValid, value);
        }

        private bool _isSPasswordValid;
        public bool IsSPasswordValid
        {
            get => _isSPasswordValid;

            set => SetProperty(ref _isSPasswordValid, value);
        }

        private string _usernameEntry;
        public string UsernameEntry
        {
            get => _usernameEntry;

            set => SetProperty(ref _usernameEntry, value);
        }

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

        private string _sPasswordEntry;
        public string SPasswordEntry
        {
            get => _sPasswordEntry;

            set => SetProperty(ref _sPasswordEntry, value);
        }

        private bool _isButtonEnable;
        public bool IsButtonEnable
        {
            get => _isButtonEnable;

            set => SetProperty(ref _isButtonEnable, value);
        }

        public ICommand _singUpButtonClickCommand;
        public ICommand SingUpButtonClickCommand => _singUpButtonClickCommand ??= new Command(OnSingUpButtonClickCommandAsync);

        private ICommand _textChangedCommand;
        public ICommand TextChangedCommand => _textChangedCommand ??= new Command(OnTextChangedCommand);

        #endregion

        #region -- Private helpers --

        private void OnTextChangedCommand()
        {
            IsButtonEnable = ActivateButton();
        }

        private bool ActivateButton()
        {
            var isValid = true;

            if (string.IsNullOrWhiteSpace(UsernameEntry) || 
                string.IsNullOrWhiteSpace(EmailEntry) || 
                string.IsNullOrWhiteSpace(PasswordEntry) ||
                string.IsNullOrWhiteSpace(SPasswordEntry))
            {
                isValid = false;
            }

            return isValid;
        }

        private async void OnSingUpButtonClickCommandAsync()
        {
            var isValid = await CheckUserInputAsync();

            if (isValid)
            {
                var isSignedUp = await _authorizationService.SignUpAsync(UsernameEntry, EmailEntry, PasswordEntry);

                if (isSignedUp)
                {
                    NavigationParameters parameters = new NavigationParameters
                    {
                        {Constants.EMAIL,  EmailEntry}
                    };

                    await _navigationService.GoBackAsync(parameters);
                }
                else
                {
                    string alertText = Resources.Resource.ExistEmailAlert;
                    string button = Resources.Resource.OkButton;

                    await _userDialogs.AlertAsync(alertText, string.Empty, button);
                }

            }
            else
            {
                Debug.WriteLine("CheckUserInput method returned false");
            }

        }

        private async Task<bool> CheckUserInputAsync()
        {
            IsUsernameValid = true;
            IsEmailValid = true;
            IsPasswordValid = true;
            IsSPasswordValid = true;

            var isValid = false;

            var usernameResult = Validator.UserNameValidator(UsernameEntry);
            var emailResult = Validator.EmailValidator(EmailEntry);
            var passwordResult = Validator.PasswordValidator(PasswordEntry);

            if (!usernameResult)
            {
                isValid = false;
                IsUsernameValid = false;

                string alertText = Resources.Resource.BadUserNameAlert;
                string button = Resources.Resource.OkButton;

                await _userDialogs.AlertAsync(alertText, string.Empty, button);
            }
            else if(!emailResult)
            {
                isValid = false;
                IsEmailValid = false;

                string alertText = Resources.Resource.BadEmailAlert;
                string button = Resources.Resource.OkButton;

                await _userDialogs.AlertAsync(alertText, string.Empty, button);
            }
            else if (!passwordResult)
            {
                isValid = false;
                IsPasswordValid = false;

                string alertText = Resources.Resource.BadPasswordAlert;
                string button = Resources.Resource.OkButton;

                await _userDialogs.AlertAsync(alertText, string.Empty, button);
            }
            else if(PasswordEntry != SPasswordEntry)
            {
                isValid = false;
                IsSPasswordValid = false;

                string alertText = Resources.Resource.PasswordsNotMatchAlert;
                string button = Resources.Resource.OkButton;

                await _userDialogs.AlertAsync(alertText, string.Empty, button);
            }
            else
            {
                isValid = true;
            }

            return isValid;
        } 

        #endregion

    }
}
