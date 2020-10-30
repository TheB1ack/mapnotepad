using Acr.UserDialogs;
using MapNotepad.Extentions;
using MapNotepad.Models;
using MapNotepad.Services.PinsService;
using Plugin.Settings;
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
using Xamarin.Forms.Internals;

namespace MapNotepad.ViewModels
{
    public class AddEditPinPageViewModel : ViewModelBase
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

        private string _nameEntry;
        public string NameEntry
        {
            get { return _nameEntry; }
            set
            {
                SetProperty(ref _nameEntry, value);
            }
        }
        private string _descriptionEditor;
        public string DescriptionEditor
        {
            get { return _descriptionEditor; }
            set
            {

                SetProperty(ref _descriptionEditor, value);


            }
        }
        private double _latitudeEntry;
        public double LatitudeEntry
        {
            get { return _latitudeEntry; }
            set
            {
                if (value > 90 || value < -90)
                {
                    UserDialogs.Instance.Alert("Unvalid latitude!", "", "OK");
                }
                else
                {
                    SetProperty(ref _latitudeEntry, value);
                }
                
            }
        }
        private double _longitudeEntry;
        public double LongitudeEntry
        {
            get { return _longitudeEntry; }
            set
            {
                if (value > 180 || value < 0)
                {
                    UserDialogs.Instance.Alert("Unvalid latitude!", "", "OK");
                }
                else
                {
                    SetProperty(ref _longitudeEntry, value);
                    
                }
            }
        }
        private CustomPin _myFocusedPin;
        public CustomPin MyFocusedPin
        {
            get { return _myFocusedPin; }
            set
            {
                SetProperty(ref _myFocusedPin, value);
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
        private bool _isMapVisible;
        public bool IsMapVisible
        {
            get { return _isMapVisible; }
            set
            {
                SetProperty(ref _isMapVisible, value);
            }
        }
        private bool _isEntryVisible;
        public bool IsEntryVisible
        {
            get { return _isEntryVisible; }
            set
            {
                SetProperty(ref _isEntryVisible, value);
            }
        }


        public ICommand MapTapped => new Command<Position>(OnMapClicked);
        public ICommand SaveClick => new Command(SavePin);
        public ICommand CheckBoxSet => new Command(ShowMap);
        public AddEditPinPageViewModel(INavigationService navigationService, IPinService pinService) : base(navigationService)
        {
            _pinService = pinService;
            PinsCollection = new ObservableCollection<CustomPin>();
        }
        private void ShowMap()
        {
            if (IsCheckBoxChecked)
            {
                ChangePinPlace();
                IsEntryVisible = false;
                IsMapVisible = true;
            }
            else
            {
                IsEntryVisible = true;
                IsMapVisible = false;
            }
        }
        private void ChangePinPlace()
        {
            if(PinsCollection.Count() != 0)
            {
                PinsCollection.First().PositionLat = LatitudeEntry;
                PinsCollection.First().PositionLong = LongitudeEntry;
            }
            else
            {
                OnMapClicked(new Position(LatitudeEntry, LongitudeEntry));
            }
        }
        private void OnMapClicked(Position item)
        {
            CustomPin pin = new CustomPin()
            {
                Name = NameEntry ?? " ",
                PositionLat = item.Latitude,
                PositionLong = item.Longitude
            };
            LongitudeEntry = pin.PositionLong;
            LatitudeEntry = pin.PositionLat;

            PinsCollection = new ObservableCollection<CustomPin>() { pin };
        }
        private async void SavePin()
        {
            if (EntryCheck())
            {
                if (MyFocusedPin == null)
                {
                    await _pinService.AddPinAsync(NameEntry, DescriptionEditor, PinsCollection.First());
                    await _navigationService.GoBackAsync();
                }
                else
                {
                    MyFocusedPin.Name = NameEntry;
                    MyFocusedPin.Description = DescriptionEditor;
                    MyFocusedPin.PositionLat = LatitudeEntry;
                    MyFocusedPin.PositionLong = LongitudeEntry;  

                    await _pinService.UpdatePinAsync(MyFocusedPin);
                    await _navigationService.GoBackAsync();
                }
            }
        }
        private bool EntryCheck()
        {
            bool isOk = true;
            if (string.IsNullOrWhiteSpace(NameEntry))
            {
                isOk = false;
                UserDialogs.Instance.Alert("Field Pin name musn't be empty!", "", "OK");
            }
            else if (IsCheckBoxChecked)
            {
                if (PinsCollection.Count == 0)
                {
                    isOk = false;
                    UserDialogs.Instance.Alert("Please, select your marker on map!", "", "OK");
                }                
            }

            return isOk;
        }

        private void FillData(CustomPin pin)
        {
            NameEntry = pin.Name;
            DescriptionEditor = pin.Description;
            LongitudeEntry = pin.PositionLong;
            LatitudeEntry = pin.PositionLat;
            MyFocusedPin = pin;
            PinsCollection = new ObservableCollection<CustomPin>() { pin };
        }
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            IsCheckBoxChecked = true;

            var pin = (CustomPin)parameters["pin"];
            if (pin != null)
            {
                FillData(pin);
            }
        }

    }
}
