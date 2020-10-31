using Acr.UserDialogs;
using MapNotepad.Services.Authorization;
using Prism.Navigation;
using System.Net.Mail;
using System.Windows.Input;
using Xamarin.Forms;

namespace MapNotepad.ViewModels
{
    public class SingUpPageViewModel : ViewModelBase
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IUserDialogs _userDialogs;
       
        public SingUpPageViewModel(INavigationService navigationService, IAuthorizationService authorizationService, IUserDialogs userDialogs) : base(navigationService)
        {
            _authorizationService = authorizationService;
            _userDialogs = userDialogs;
        }

        #region -- Public properties --

        private string _usernameEntry;
        public string UsernameEntry
        {
            get
            {
                return _usernameEntry;
            }
            set
            {
                _usernameEntry = value;
                IsButtonEnable = TryActivateButton();
            }
        }

        private string _emailEntry;
        public string EmailEntry
        {
            get
            {
                return _emailEntry;
            }
            set
            {
                _emailEntry = value;
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

        private string _sPasswordEntry;
        public string SPasswordEntry
        {
            get
            {
                return _sPasswordEntry;
            }
            set
            {
                SetProperty(ref _sPasswordEntry, value);
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
       
        public ICommand SingUpBClick => new Command(VerifyAndSaveAsync);

        #endregion

        #region -- Private helpers --

        private bool TryActivateButton()
        {
            bool isValid = true;
            if (string.IsNullOrWhiteSpace(UsernameEntry))
            {
                isValid = false;
            }
            else if (string.IsNullOrWhiteSpace(EmailEntry))
            {
                isValid = false;
            }
            else if(string.IsNullOrWhiteSpace(PasswordEntry))
            {
                isValid = false;
            }
            else if(string.IsNullOrWhiteSpace(SPasswordEntry))
            {
                isValid = false;
            }

            return isValid;
        }
        private async void VerifyAndSaveAsync()
        {
            if (CheckUserInput())
            {
                bool isSignedUp = await _authorizationService.SignUpAsync(UsernameEntry, EmailEntry, PasswordEntry);
                if (isSignedUp)
                {
                    NavigationParameters parameters = new NavigationParameters();
                    parameters.Add("email", EmailEntry);

                    await _navigationService.GoBackAsync(parameters);
                }
                else
                {
                    _userDialogs.Alert("This Email is already taken!", "", "OK");
                }
            }
        }
        private bool CheckUserInput()
        {
            bool isValid = true;
            if (UsernameEntry.Length <= 3 || UsernameEntry.Length >= 15)
            {
                _userDialogs.Alert("Name must be between 3 and 15 characters long!", "", "OK");
                isValid = false;
            }
            else if (!EmailValidation(EmailEntry))
            {
                _userDialogs.Alert("Email is not valid", "", "OK");
                isValid = false;
            }
            else if (!System.Text.RegularExpressions.Regex.IsMatch(PasswordEntry, @"^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])") || PasswordEntry.Length <= 4 || PasswordEntry.Length >= 20)
            {
                _userDialogs.Alert("Password must contain one capital letter, one number and be between 4 and 20 characters long!", "", "OK");
                isValid = false;
            }
            else if (PasswordEntry != SPasswordEntry)
            {
                _userDialogs.Alert("Passwords must match!", "", "OK");
                isValid = false;
            }

            return isValid;
        }
        private bool EmailValidation(string email)
        {
            bool isValid = true;
            try
            {
                var mail = new MailAddress(email);
            }
            catch
            {
                isValid = false;
            }

            return isValid;
        }

        #endregion

    }
}
