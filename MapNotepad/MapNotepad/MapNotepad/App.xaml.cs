using Prism;
using Prism.Ioc;
using Prism.Unity;
using Xamarin.Forms;
using MapNotepad.Views;
using MapNotepad.ViewModels;
using MapNotepad.Services.Repository;
using MapNotepad.Services.Authorization;
using Plugin.Settings;
using MapNotepad.Services.Pins;
using MapNotepad.Services.Settings;
using Acr.UserDialogs;
using MapNotepad.Services.Map;
using MapNotepad.Services.MapService;
using Plugin.Permissions;
using MapNotepad.Services.Permissions;
using MapNotepad.Services.REST;
using MapNotepad.Services.WeatherService;

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
            var isAuthorized = Container.Resolve<IAuthorizationService>().IsAuthorized;

            if (isAuthorized)
            {
                await NavigationService.NavigateAsync($"/{nameof(NavigationPage)}/{nameof(HomeTabbedPage)}");
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
            containerRegistry.RegisterForNavigation<HomeTabbedPage, HomeTabbedPageViewModel>();
            containerRegistry.RegisterForNavigation<MapPage, MapPageViewModel>();
            containerRegistry.RegisterForNavigation<PinsListPage, PinsListPageViewModel>();
            containerRegistry.RegisterForNavigation<AddEditPinPage, AddEditPinPageViewModel>();
            containerRegistry.RegisterForNavigation<QrScannerPage, QrScannerPageViewModel>();
            containerRegistry.RegisterForNavigation<WeatherPage, WeatherPageViewModel>();

            //packages
            containerRegistry.RegisterInstance(CrossSettings.Current);
            containerRegistry.RegisterInstance(UserDialogs.Instance);
            containerRegistry.RegisterInstance(CrossPermissions.Current);

            //services
            containerRegistry.RegisterInstance<IRepositoryService>(Container.Resolve<RepositoryService>());
            containerRegistry.RegisterInstance<ISettingsService>(Container.Resolve<SettingsService>());
            containerRegistry.RegisterInstance<IAuthorizationService>(Container.Resolve<AuthorizationService>());
            containerRegistry.RegisterInstance<IPinService>(Container.Resolve<PinService>());
            containerRegistry.RegisterInstance<IMapService>(Container.Resolve<MapService>());
            containerRegistry.RegisterInstance<IPermissionsService>(Container.Resolve<PermissionsService>());
            containerRegistry.RegisterInstance<IRestService>(Container.Resolve<RestService>());
            containerRegistry.RegisterInstance<IWeatherService>(Container.Resolve<WeatherService>());


        }
    }
}
