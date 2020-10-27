﻿using Prism;
using System;
using Prism.Ioc;
using Prism.Unity;
using Prism.Modularity;
using Xamarin.Forms;
using MapNotepad.Views;
using MapNotepad.ViewModels;
using MapNotepad.Services.Repository;
using MapNotepad.Services.Authorization;
using Plugin.Settings;

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
            if (CrossSettings.Current.GetValueOrDefault("UserId", -1) != -1)
            {
                await NavigationService.NavigateAsync("NavigationPage/MainListPage");
            }
            else
            {
                await NavigationService.NavigateAsync("NavigationPage/SingInPage");
            }
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<SingUpPage, SingUpPageViewModel>();
            containerRegistry.RegisterForNavigation<SingInPage, SingInPageViewModel>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<MapPage, MapPageViewModel>();
            containerRegistry.RegisterForNavigation<PinsListPage, PinsListPageViewModel>();

            containerRegistry.RegisterInstance<IRepositoryService>(Container.Resolve<RepositoryService>());
            containerRegistry.RegisterInstance<IAuthorizationService>(Container.Resolve<AuthorizationService>());
        }
    }
}