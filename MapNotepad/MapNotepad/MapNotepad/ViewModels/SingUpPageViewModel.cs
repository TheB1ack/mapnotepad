using Prism.Mvvm;
using Prism.Navigation;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace MapNotepad.ViewModels
{
    public class SingUpPageViewModel : BindableBase
    {
        protected INavigationService _navigationService { get; private set; }
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
        public SingUpPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        private bool TryActivateButton()
        {
            bool flag = true;
            if (string.IsNullOrWhiteSpace(UsernameEntry))
            {
                flag =  false;
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
            NavigationParameters parameters = new NavigationParameters();
            parameters.Add("email", UsernameEntry);
            await _navigationService.GoBackAsync(parameters);
        }
    }
}
