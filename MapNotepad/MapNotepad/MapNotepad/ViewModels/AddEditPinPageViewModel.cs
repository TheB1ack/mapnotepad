using Acr.UserDialogs;
using MapNotepad.Models;
using MapNotepad.Services.Pins;
using Prism.Navigation;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace MapNotepad.ViewModels
{
    public class AddEditPinPageViewModel : ViewModelBase
    {
        private readonly IPinService _pinService;
        private readonly IUserDialogs _userDialogs;

        public AddEditPinPageViewModel(INavigationService navigationService, 
                                       IPinService pinService, 
                                       IUserDialogs userDialogs) 
                                       : base(navigationService)
        {
            _userDialogs = userDialogs;
            _pinService = pinService;
            PinsCollection = new ObservableCollection<CustomPin>();
        }

        #region -- Public properties --

        private ObservableCollection<CustomPin> _pinsCollection;
        public ObservableCollection<CustomPin> PinsCollection
        {
            get => _pinsCollection;

            set => SetProperty(ref _pinsCollection, value);
        }

        private string _oldName;
        public string OldName
        {
            get => _oldName;

            set => SetProperty(ref _oldName, value);
        }

        private string _nameEntry;
        public string NameEntry
        {
            get => _nameEntry;

            set => SetProperty(ref _nameEntry, value);
        }

        private string _descriptionEditor;
        public string DescriptionEditor 
        {
            get => _descriptionEditor;

            set => SetProperty(ref _descriptionEditor, value);
        }

        private double _latitudeEntry;
        public double LatitudeEntry
        {
            get => _latitudeEntry;

            set => SetProperty(ref _latitudeEntry, value);
        }

        private double _longitudeEntry;
        public double LongitudeEntry
        {
            get => _longitudeEntry;

            set => SetProperty(ref _longitudeEntry, value);
        }

        private CustomPin _myFocusedPin;
        public CustomPin MyFocusedPin
        {
            get => _myFocusedPin;

            set => SetProperty(ref _myFocusedPin, value);
        }

        private bool _isCheckBoxChecked;
        public bool IsCheckBoxChecked
        {
            get => _isCheckBoxChecked;

            set => SetProperty(ref _isCheckBoxChecked, value);
        }

        private bool _isMapVisible;
        public bool IsMapVisible
        {
            get => _isMapVisible;

            set => SetProperty(ref _isMapVisible, value);
        }

        private bool _isEntryVisible;
        public bool IsEntryVisible
        {
            get => _isEntryVisible;

            set => SetProperty(ref _isEntryVisible, value);
        }

        private ICommand _mapTappedCommad;
        public ICommand MapTappedCommad => _mapTappedCommad ??= new Command<Position>(OnMapTappedCommand);

        private ICommand _saveClickCommand;
        public ICommand SaveClickCommand => _saveClickCommand ??= new Command(OnSaveClickCommand);

        private ICommand _checkBoxSetCommand;
        public ICommand CheckBoxSetCommand => _checkBoxSetCommand ??= new Command(OnCheckBoxSetCommand);

        #endregion

        #region -- IterfaceName implementation --

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            IsCheckBoxChecked = true;

            if (parameters.TryGetValue(nameof(CustomPin), out CustomPin pin))
            {
                FillData(pin);
            }
            else
            {
                Debug.WriteLine("Parameters are missing CustomPin");
            }

        }

        #endregion

        #region -- Private helpers --

        private void OnCheckBoxSetCommand()
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
            if (PinsCollection.Any()) 
            {
                PinsCollection.First().PositionLat = LatitudeEntry;
                PinsCollection.First().PositionLong = LongitudeEntry;
            }
            else
            {
                OnMapTappedCommand(new Position(LatitudeEntry, LongitudeEntry));
            }

        }
        private void OnMapTappedCommand(Position clickPosition)
        {
            CustomPin pin = new CustomPin
            {
                Name = NameEntry ?? string.Empty, 
                PositionLat = clickPosition.Latitude,
                PositionLong = clickPosition.Longitude
            };

            LongitudeEntry = pin.PositionLong;
            LatitudeEntry = pin.PositionLat;

            PinsCollection = new ObservableCollection<CustomPin> 
            { 
                pin 
            };
        }
        private async void OnSaveClickCommand()
        {
            var isValidEntry = await EntryCheckAsync();
            if (isValidEntry)
            {
                var isValidName = await _pinService.CheckPinName(NameEntry);

                if (isValidName || OldName == NameEntry)
                {
                    if (MyFocusedPin == null)
                    {
                        CustomPin customPin = new CustomPin
                        {
                            Name = NameEntry,
                            Description = DescriptionEditor ?? string.Empty,
                            PositionLat = PinsCollection.First().PositionLat,
                            PositionLong = PinsCollection.First().PositionLong,
                            FavouriteImageSource = "empty_heart.png"
                        };

                        await _pinService.AddPinAsync(customPin);
                    }
                    else
                    {
                        MyFocusedPin.Name = NameEntry;
                        MyFocusedPin.Description = DescriptionEditor;
                        MyFocusedPin.PositionLat = LatitudeEntry;
                        MyFocusedPin.PositionLong = LongitudeEntry;
                        MyFocusedPin.IsFavourite = false;

                        await _pinService.UpdatePinAsync(MyFocusedPin);
                    }

                    await _navigationService.GoBackAsync();
                }
                else
                {
                    _userDialogs.Alert("Pin name is already exist!", "", "OK");
                }

            }
            else
            {
                Debug.WriteLine("Entry text is unvalid!");
            }

        }
        private async Task<bool> EntryCheckAsync()
        {
            var isValid = true;

            if (string.IsNullOrWhiteSpace(NameEntry))
            {
                isValid = false;
                await _userDialogs.AlertAsync("Field Pin name musn't be empty!", "", "OK");
            }
            else
            {
                Debug.WriteLine("NameEntry is ok");
            }

            if (IsCheckBoxChecked)
            {
                if (!PinsCollection.Any())
                {
                    isValid = false;
                    await _userDialogs.AlertAsync("Please, select your marker on map!", "", "OK");
                }
                else
                {
                    Debug.WriteLine("Collection contains pin");
                }

            }
            else
            {
                if (LongitudeEntry > 180 || LongitudeEntry < 0)
                {
                    await _userDialogs.AlertAsync("Unvalid longitude!", "", "OK");
                    isValid = false;
                }
                else if(LatitudeEntry > 90 || LatitudeEntry < -90)
                {
                    await _userDialogs.AlertAsync("Unvalid latitude!", "", "OK");
                    isValid = false;
                }
                else
                {
                    Debug.WriteLine("Longitude and latitude is ok");
                }

            }

            return isValid;
        }

        private void FillData(CustomPin pin)
        {
            OldName = pin.Name;
            NameEntry = pin.Name;
            DescriptionEditor = pin.Description;
            LongitudeEntry = pin.PositionLong;
            LatitudeEntry = pin.PositionLat;

            MyFocusedPin = pin;

            PinsCollection = new ObservableCollection<CustomPin> 
            { 
                pin 
            };
        }

        #endregion
    }
}
