    using MapNotepad.Extentions;
using MapNotepad.Models;
using MapNotepad.Services.PinsService;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Unity;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace MapNotepad.ViewModels
{
    public class PinsListPageViewModel : ViewModelBase
    {
        IPinService _pinService;

        private ObservableCollection<CustomPin> _pinsCollection;
        public ObservableCollection<CustomPin> PinsCollection
        {
            get { return _pinsCollection; }
            set
            {
                SetProperty(ref _pinsCollection, value);
            }
        }
        private bool _isVisibleText;
        public bool IsVisibleText
        {
            get { return _isVisibleText; }
            set
            {
                SetProperty(ref _isVisibleText, value);
            }
        }
        private CustomPin _itemSelected;
        public CustomPin ItemSelected
        {
            get
            {
                return _itemSelected;
            }
            set
            {
                _itemSelected = value;
                NavigateToMapPage();
            }
        }
        private string _searchBarText;
        public string SearchBarText
        {
            get { return _searchBarText; }
            set
            {
                SetProperty(ref _searchBarText, value);
            }
        }
        private bool _isCheckBoxChecked;
        public bool IsCheckBoxChecked
        {
            get { return _isCheckBoxChecked; }
            set
            {
                SetProperty(ref _isCheckBoxChecked, value);
            }
        }
        public ICommand EditTap => new Command(GoToAddEditPinPageAsync);
        public ICommand DeleteTap => new Command(TryToDeleteItem);
        public ICommand AddButtonClicked => new Command(GoToAddEditPinPageAsync);
        public ICommand UserSearching => new Command(SearchPins);
        public ICommand CheckBoxSet => new Command(SetFavouritePin);
        public PinsListPageViewModel(INavigationService navigationService, IPinService pinService) : base(navigationService)
        {
            _pinService = pinService;
        }
        private void SetFavouritePin()
        {
            
        }
        private async void GoToAddEditPinPageAsync(object item = null)
        {
            var pin = item as CustomPin;
            NavigationParameters parameters = new NavigationParameters();
            parameters.Add("pin", pin);
            await _navigationService.NavigateAsync("AddEditPinPage", parameters);    
        }
        private async void TryToDeleteItem(object item)
        { 
            var pin = item as CustomPin;
            await _pinService.RemovePinAsync(pin);

            CollectionResize();
        }
        private async void SearchPins()
        {
            if (!string.IsNullOrWhiteSpace(SearchBarText))
            {
                PinsCollection = await _pinService.GetPinsByText(SearchBarText);
                CheckCollectionSize();
            }
            else
            {
                PinsCollection = await _pinService.GetPinsAsync();
            }
        }
        private async void CollectionResize()
        {
            PinsCollection = await _pinService.GetPinsAsync();

            CheckCollectionSize();
        }
        private void CheckCollectionSize()
        {
            if (PinsCollection.Count == 0)
            {
                IsVisibleText = true;
            }
            else
            {
                IsVisibleText = false;
            }
        }
        private void NavigateToMapPage()
        {
            if(ItemSelected != null)
            {
                NavigationParameters parameters = new NavigationParameters();
                ItemSelected.IsAnimated = true;
                parameters.Add("FocusedPin", ItemSelected);
                _navigationService.NavigateAsync("../MainPage?selectedTab=MapPage", parameters);
            }
        }
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            CollectionResize();           
        }
    }
}
