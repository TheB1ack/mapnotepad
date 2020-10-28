using MapNotepad.Extentions;
using MapNotepad.Models;
using MapNotepad.Services.Authorization;
using MapNotepad.Services.Pin;
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

        private ObservableCollection<Pin> _pinsCollection;
        public ObservableCollection<Pin> PinsCollection
        {
            get { return _pinsCollection; }
            set
            {
                SetProperty(ref _pinsCollection, value);
            }
        }
        public MapPageViewModel(INavigationService navigationService, IPinService pinService) : base(navigationService)
        {
            _pinService = pinService;

            PinsCollection = new ObservableCollection<Pin>();
        }
        private async void SetMapPins()
        {
            var oldPins = await _pinService.GetPinsAsync();

            var items = CustomPinsToPins(oldPins);
            PinsCollection = items;
        }
        private ObservableCollection<Pin> CustomPinsToPins(ObservableCollection<CustomPin> oldCollection)
        {
            var newCollection = new ObservableCollection<Pin>();
            foreach (var item in oldCollection)
            {
                newCollection.Add(item.ConvertToPin());
            }

            return newCollection;
        }
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            SetMapPins();
        }
    }
}
