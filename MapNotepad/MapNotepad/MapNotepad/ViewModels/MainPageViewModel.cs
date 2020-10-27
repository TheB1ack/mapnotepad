using MapNotepad.Services.Authorization;
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
    public class MainPageViewModel : ViewModelBase
    {

        IAuthorizationService _authorizationService;
        public ICommand LogOutClick => new Command(LogOut);
        public MainPageViewModel(INavigationService navigationService, IAuthorizationService authorizationService) : base(navigationService)
        {
            _authorizationService = authorizationService;
        }
        private async void LogOut()
        {
            _authorizationService.LogOut();
            await _navigationService.NavigateAsync("../SingInPage");
        }
    }
}
