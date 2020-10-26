using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace MapNotepad.ViewModels
{
    public class SingInPageViewModel : BindableBase
    {
        protected INavigationService _navigationService { get; private set; }
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
        public SingInPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }
        private bool TryActivateButton()
        {
            bool flag = true;
            if (string.IsNullOrWhiteSpace(EmailEntry))
            {
                flag = false;
            }
            if (string.IsNullOrWhiteSpace(PasswordEntry))
            {
                flag = false;
            }
            return flag;
        }
        private async void TryToSingInAsync()
        {
            if (IsButtonEnable)
            {
                await _navigationService.NavigateAsync("../MainPage");
            }
        }
        private async void NavigateToSingUpPage()
        {
            await _navigationService.NavigateAsync("SingUpPage");
        }
    }
}
