using Prism;
using System;
using Prism.Ioc;
using Prism.Unity;
using Prism.Modularity;
using Xamarin.Forms;
using MapNotepad.Views;
using MapNotepad.ViewModels;

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
            await NavigationService.NavigateAsync("NavigationPage/SingInPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<SingUpPage, SingUpPageViewModel>();
            containerRegistry.RegisterForNavigation<SingInPage, SingInPageViewModel>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<MapPage, MapPageViewModel>();
            containerRegistry.RegisterForNavigation<PinsListPage, PinsListPageViewModel>();
        }
    }
}
