using Acr.UserDialogs;
using MapNotepad.Services.Authorization;
using Prism.Navigation;
using System;
using System.Diagnostics;
using System.Net.Mail;
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
        }

        #region -- Public properties --

        private string _usernameEntry;
        public string UsernameEntry
        {
            get => _usernameEntry;

            set => _usernameEntry = value;
        }

        private string _emailEntry;
        public string EmailEntry
        {
            get => _emailEntry;

            set => _emailEntry = value;
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
            IsButtonEnable = TryActivateButton();
        }
        private bool TryActivateButton()
        {
            var isValid = true;

            if (string.IsNullOrWhiteSpace(UsernameEntry) || 
                string.IsNullOrWhiteSpace(EmailEntry) || 
                string.IsNullOrWhiteSpace(PasswordEntry) ||
                string.IsNullOrWhiteSpace(SPasswordEntry))
            {
                isValid = false;
            }
            else
            {
                Debug.WriteLine("Validation failed");
            }


            return isValid;
        }
        private async void OnSingUpButtonClickCommandAsync()
        {
            if (CheckUserInput())
            {
                var isSignedUp = await _authorizationService.SignUpAsync(UsernameEntry, EmailEntry, PasswordEntry);

                if (isSignedUp)
                {
                    NavigationParameters parameters = new NavigationParameters
                    {
                        {"Email",  EmailEntry}
                    };

                    await _navigationService.GoBackAsync(parameters);
                }
                else
                {
                   await _userDialogs.AlertAsync("This Email is already taken!", "", "OK");
                }
            }
            else
            {
                Debug.WriteLine("CheckUserInput method returned false");
            }
        }
        private bool CheckUserInput() //need to be reworked
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
            else if (!System.Text.RegularExpressions.Regex.IsMatch(PasswordEntry, @"^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])") || PasswordEntry.Length <= 4 || PasswordEntry.Length >= 20) // regex
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
        private bool EmailValidation(string email) //need to be reworked
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
