using Acr.UserDialogs;
using MapNotepad.Services.Authorization;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Unity;
using Xamarin.Forms;

namespace MapNotepad.ViewModels
{
    public class SingInPageViewModel : ViewModelBase
    {
        IAuthorizationService _authorizationService;

        private string _emailEntry;
        public string EmailEntry
        {
            get { return _emailEntry; }
            set
            {
                SetProperty(ref _emailEntry, value);
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
        private bool _isButtonEnable;
        public bool IsButtonEnable
        {
            get { return _isButtonEnable; }
            set
            {
                SetProperty(ref _isButtonEnable, value);
            }
        }

        public ICommand SingInBClick => new Command(TryToSingInAsync);
        public ICommand CreateAccountBClick => new Command(NavigateToSingUpPage);
        public SingInPageViewModel(INavigationService navigationService, IAuthorizationService authorizationService) : base(navigationService)
        {
            _authorizationService = authorizationService;
        }
        private bool TryActivateButton()
        {
            bool isOk = true;
            if (string.IsNullOrWhiteSpace(EmailEntry))
            {
                isOk = false;
            }
            if (string.IsNullOrWhiteSpace(PasswordEntry))
            {
                isOk = false;
            }

            return isOk;
        }
        private async void TryToSingInAsync()
        {
            bool isOk = await _authorizationService.SingInAsync(EmailEntry, PasswordEntry);
            if (isOk)
            {
                await _navigationService.NavigateAsync("../MainPage");
            }
            else
            {
                UserDialogs.Instance.Alert("Incorrect password or email!", "", "OK");
                PasswordEntry = "";
            }
        }
        private async void NavigateToSingUpPage()
        {
            await _navigationService.NavigateAsync("SingUpPage");
        }
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            EmailEntry = (string)parameters["email"];
        }
    }
}
