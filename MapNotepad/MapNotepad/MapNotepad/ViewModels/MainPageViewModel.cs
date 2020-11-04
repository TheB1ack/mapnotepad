﻿using MapNotepad.Services.Authorization;
using MapNotepad.Views;
using Prism.Navigation;
using System.Windows.Input;
using Xamarin.Forms;

namespace MapNotepad.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private readonly IAuthorizationService _authorizationService;       

        public MainPageViewModel(INavigationService navigationService, 
                                 IAuthorizationService authorizationService) 
                                 : base(navigationService)
        {
            _authorizationService = authorizationService;
        }

        #region -- Public properties --

        private ICommand _logOutClickCommand;
        public ICommand LogOutClickCommand => _logOutClickCommand ??= new Command(OnLogOutClickCommandAsync);

        #endregion

        #region -- Private helpers --

        private async void OnLogOutClickCommandAsync()
        {
            _authorizationService.LogOut();

            await _navigationService.NavigateAsync($"/{nameof(NavigationPage)}/{nameof(SingInPage)}");
        }

        #endregion
    }
}
