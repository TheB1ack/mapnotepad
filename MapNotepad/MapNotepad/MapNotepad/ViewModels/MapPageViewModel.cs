using MapNotepad.Extentions;
using MapNotepad.Models;
using MapNotepad.Services.Authorization;
using MapNotepad.Services.PinsService;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace MapNotepad.ViewModels
{
    public class MapPageViewModel : ViewModelBase
    {
        IPinService _pinService;

        private CustomPin _myFocusedPin;
        public CustomPin MyFocusedPin
        {
            get { return _myFocusedPin; }
            set
            {
                SetProperty(ref _myFocusedPin, value);
            }
        }
        private ObservableCollection<CustomPin> _pinsCollection;
        public ObservableCollection<CustomPin> PinsCollection
        {
            get { return _pinsCollection; }
            set
            {
                SetProperty(ref _pinsCollection, value);
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
        public ICommand UserSearching => new Command(SearchPins);
        public ICommand PinTapCommand => new Command<Pin>(ShowSelectedPin);
        public MapPageViewModel(INavigationService navigationService, IPinService pinService) : base(navigationService)
        {
            _pinService = pinService;
            PinsCollection = new ObservableCollection<CustomPin>();
        }
        private void ShowSelectedPin(Pin pin)
        {
            
           
        }
        private async void SetMapPins()
        {
            PinsCollection = await _pinService.GetPinsAsync();
        }
        private async void SearchPins()
        {
            if (!string.IsNullOrWhiteSpace(SearchBarText))
            {
                PinsCollection = await _pinService.GetPinsByText(SearchBarText);
            }
            else
            {
                SetMapPins();
            }
        }
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            SetMapPins();
            if (parameters.ContainsKey("FocusedPin"))
            {
                MyFocusedPin = (CustomPin)parameters["FocusedPin"];
            }
        }
    }
}
