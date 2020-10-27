using Acr.UserDialogs;
using MapNotepad.Services.Authorization;
using Prism.Mvvm;
using Prism.Navigation;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace MapNotepad.ViewModels
{
    public class SingUpPageViewModel : ViewModelBase
    {
        IAuthorizationService _authorizationService;

        private string _usernameEntry;
        public string UsernameEntry
        {
            get { return _usernameEntry; }
            set
            {
                _usernameEntry = value;
                IsButtonEnable = TryActivateButton();
            }
        }
        private string _emailEntry;
        public string EmailEntry
        {
            get { return _emailEntry; }
            set
            {
                _emailEntry = value;
                IsButtonEnable = TryActivateButton();
            }
        }
        private string _passwordEntry;
        public string PasswordEntry
        {
            get { return _passwordEntry; }
            set
            {
                SetProperty(ref _passwordEntry, value);
                IsButtonEnable = TryActivateButton();
            }
        }
        private string _sPasswordEntry;
        public string SPasswordEntry
        {
            get { return _sPasswordEntry; }
            set
            {
                SetProperty(ref _sPasswordEntry, value);
                IsButtonEnable = TryActivateButton();
            }
        }
        private bool _isButtonEnable;
        public bool IsButtonEnable
        {
            get { return _isButtonEnable; }
            set
            {
                SetProperty(ref _isButtonEnable, value);
            }
        }
        public ICommand SingUpBClick => new Command(VerifyAndSaveAsync);
        public SingUpPageViewModel(INavigationService navigationService, IAuthorizationService authorizationService) : base(navigationService)
        {
            _authorizationService = authorizationService;
        }

        private bool TryActivateButton()
        {
            bool flag = true;
            if (string.IsNullOrWhiteSpace(UsernameEntry))
            {
                flag = false;
            }
            if (string.IsNullOrWhiteSpace(EmailEntry))
            {
                flag = false;
            }
            if (string.IsNullOrWhiteSpace(PasswordEntry))
            {
                flag = false;
            }
            if (string.IsNullOrWhiteSpace(SPasswordEntry))
            {
                flag = false;
            }
            return flag;
        }
        private async void VerifyAndSaveAsync()
        {
            if (CheckUserInput())
            {
                bool flag = await _authorizationService.SingUpAsync(UsernameEntry, EmailEntry, PasswordEntry);
                if (flag)
                {
                    NavigationParameters parameters = new NavigationParameters();
                    parameters.Add("email", EmailEntry);
                    await _navigationService.GoBackAsync(parameters);
                }
            }
        }
        private bool CheckUserInput()
        {
            bool flag = true;
            if (UsernameEntry.Length <= 3 || UsernameEntry.Length >= 15)
            {
                UserDialogs.Instance.Alert("Name must be between 3 and 15 characters long!", "", "OK");
                flag = false;
            }
            //else if (!EmailValidation(EmailEntry)) 
            //{
              //  UserDialogs.Instance.Alert("Email is not valid", "", "OK");
                //flag = false;
           // }
            else if (!System.Text.RegularExpressions.Regex.IsMatch(PasswordEntry, @"^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])") || PasswordEntry.Length <= 4 || PasswordEntry.Length >= 20)
            {
                UserDialogs.Instance.Alert("Password must contain one capital letter, one number and be between 4 and 20 characters long!", "", "OK");
                flag = false;
            }
            else if (PasswordEntry != SPasswordEntry)
            {
                UserDialogs.Instance.Alert("Passwords must match!", "", "OK");
                flag = false;
            }

            return flag;
        }
        private bool EmailValidation(string email)
        {
            bool flag = true;
            try
            {
                var mail = new System.Net.Mail.MailAddress(email);
            }
            catch
            {
                flag = false;
            }

            return flag;
        }
    }
}
