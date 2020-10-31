using Acr.UserDialogs;
using MapNotepad.Models;
using MapNotepad.Services.PinsService;
using Prism.Navigation;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace MapNotepad.ViewModels
{
    public class AddEditPinPageViewModel : ViewModelBase
    {
        private readonly IPinService _pinService;
        private readonly IUserDialogs _userDialogs;

        public AddEditPinPageViewModel(INavigationService navigationService, IPinService pinService, IUserDialogs userDialogs) : base(navigationService)
        {
            _userDialogs = userDialogs;
            _pinService = pinService;
            PinsCollection = new ObservableCollection<CustomPin>();
        }

        #region -- Public properties --

        private ObservableCollection<CustomPin> _pinsCollection;
        public ObservableCollection<CustomPin> PinsCollection
        {
            get 
            { 
                return _pinsCollection; 
            }
            set
            {
                SetProperty(ref _pinsCollection, value);
            }
        }

        private string _nameEntry;
        public string NameEntry
        {
            get 
            { 
                return _nameEntry; 
            }
            set
            {
                SetProperty(ref _nameEntry, value);
            }
        }

        private string _descriptionEditor;
        public string DescriptionEditor
        {
            get 
            { 
                return _descriptionEditor; 
            }
            set
            {
                SetProperty(ref _descriptionEditor, value);
            }
        }

        private double _latitudeEntry;
        public double LatitudeEntry
        {
            get 
            { 
                return _latitudeEntry; 
            }
            set
            {
                if (value > 90 || value < -90)
                {
                    _userDialogs.Alert("Unvalid latitude!", "", "OK");
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
            get 
            { 
                return _longitudeEntry; 
            }
            set
            {
                if (value > 180 || value < 0)
                {
                    _userDialogs.Alert("Unvalid latitude!", "", "OK");
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
            get
            { 
                return _myFocusedPin; 
            }
            set
            {
                SetProperty(ref _myFocusedPin, value);
            }
        }

        private bool _isCheckBoxChecked;
        public bool IsCheckBoxChecked
        {
            get 
            { 
                return _isCheckBoxChecked; 
            }
            set
            {
                SetProperty(ref _isCheckBoxChecked, value);
            }
        }

        private bool _isMapVisible;
        public bool IsMapVisible
        {
            get 
            { 
                return _isMapVisible; 
            }
            set
            {
                SetProperty(ref _isMapVisible, value);
            }
        }

        private bool _isEntryVisible;
        public bool IsEntryVisible
        {
            get 
            { 
                return _isEntryVisible; 
            }
            set
            {
                SetProperty(ref _isEntryVisible, value);
            }
        }

        public ICommand MapTapped => new Command<Position>(OnMapClicked);
        public ICommand SaveClick => new Command(SavePinAsync);
        public ICommand CheckBoxSet => new Command(ShowMap);

        #endregion

        #region -- IterfaceName implementation --

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            IsCheckBoxChecked = true;

            var pin = (CustomPin)parameters["pin"];
            if (pin != null)
            {
                FillData(pin);
            }
        }

        #endregion

        #region -- Private helpers --

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
        private void OnMapClicked(Position clickPosition)
        {
            CustomPin pin = new CustomPin()
            {
                Name = NameEntry ?? " ",
                PositionLat = clickPosition.Latitude,
                PositionLong = clickPosition.Longitude
            };
            LongitudeEntry = pin.PositionLong;
            LatitudeEntry = pin.PositionLat;

            PinsCollection = new ObservableCollection<CustomPin>() { pin };
        }
        private async void SavePinAsync()
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
            bool isValid = true;
            if (string.IsNullOrWhiteSpace(NameEntry))
            {
                isValid = false;
                _userDialogs.Alert("Field Pin name musn't be empty!", "", "OK");
            }
            else if (IsCheckBoxChecked)
            {
                if (PinsCollection.Count == 0)
                {
                    isValid = false;
                    _userDialogs.Alert("Please, select your marker on map!", "", "OK");
                }                
            }

            return isValid;
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
        #endregion
    }
}
