using Prism;
using System;
using Prism.Ioc;
using Prism.Unity;
using Xamarin.Forms;
using MapNotepad.Views;
using MapNotepad.ViewModels;
using MapNotepad.Services.Repository;
using MapNotepad.Services.Authorization;
using Plugin.Settings;
using MapNotepad.Services.PinsService;
using MapNotepad.Services.Settings;
using Acr.UserDialogs;

namespace MapNotepad
{
    public partial class App : PrismApplication
    {
        public App(IPlatformInitializer initializer = null)
        : base(initializer)
        {

        }
        protected override async void OnInitialized()
        {
            InitializeComponent();
            
            if (Container.Resolve<IAuthorizationService>().IsAuthorized)
            {
                await NavigationService.NavigateAsync($"{nameof(NavigationPage)}/{nameof(Views.MainPage)}");
            }
            else
            {
                await NavigationService.NavigateAsync($"{nameof(NavigationPage)}/{nameof(SingInPage)}");
            }
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //view & viewmodels
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<SingUpPage, SingUpPageViewModel>();
            containerRegistry.RegisterForNavigation<SingInPage, SingInPageViewModel>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<MapPage, MapPageViewModel>();
            containerRegistry.RegisterForNavigation<PinsListPage, PinsListPageViewModel>();
            containerRegistry.RegisterForNavigation<AddEditPinPage, AddEditPinPageViewModel>();

            //packages
            containerRegistry.RegisterInstance(CrossSettings.Current);
            containerRegistry.RegisterInstance(UserDialogs.Instance);

            //services
            containerRegistry.RegisterInstance<IRepositoryService>(Container.Resolve<RepositoryService>());
            containerRegistry.RegisterInstance<ISettingsService>(Container.Resolve<SettingsService>());
            containerRegistry.RegisterInstance<IAuthorizationService>(Container.Resolve<AuthorizationService>());
            containerRegistry.RegisterInstance<IPinService>(Container.Resolve<PinService>());
           
        }
    }
}
