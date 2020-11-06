using Acr.UserDialogs;
using MapNotepad.Enums;
using MapNotepad.Models;
using MapNotepad.Services.Permissions;
using MapNotepad.Services.Pins;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Prism.Navigation;
using System.Collections.Generic;
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
        private readonly IPermissionsService _permissionsService;

        public AddEditPinPageViewModel(INavigationService navigationService,
                                       IPinService pinService,
                                       IUserDialogs userDialogs,
                                       IPermissionsService permissionsService)
                                       : base(navigationService)
        {
            _userDialogs = userDialogs;
            _pinService = pinService;
            _permissionsService = permissionsService;

            PinsCollection = new ObservableCollection<CustomPin>();
        }

        #region -- Public properties --

        private int _pickerItem;
        public int PickerItem
        {
            get => _pickerItem;
            set => SetProperty(ref _pickerItem, value);
        }

        private ObservableCollection<CustomPin> _pinsCollection;
        public ObservableCollection<CustomPin> PinsCollection
        {
            get => _pinsCollection;

            set => SetProperty(ref _pinsCollection, value);
        }

        private string _title;
        public string Title
        {
            get => _title;

            set => SetProperty(ref _title, value);
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

        private bool _isMyLocationEnabled;
        public bool IsMyLocationEnabled
        {
            get => _isMyLocationEnabled;

            set => SetProperty(ref _isMyLocationEnabled, value);
        }

        private ICommand _mapTappedCommad;
        public ICommand MapTappedCommad => _mapTappedCommad ??= new Command<Position>(OnMapTappedCommand);

        private ICommand _saveClickCommand;
        public ICommand SaveClickCommand => _saveClickCommand ??= new Command(OnSaveClickCommandAsync);

        private ICommand _checkBoxSetCommand;
        public ICommand CheckBoxSetCommand => _checkBoxSetCommand ??= new Command(OnCheckBoxSetCommand);

        #endregion

        #region -- IterfaceName implementation --

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            CheckLocationPermissionsAsync();

            IsCheckBoxChecked = true;

            if (parameters.TryGetValue(Constants.ACTION, out string action))
            {
                Title = action;
            }
            else
            {
                Debug.WriteLine("Missing parameters");
            }

            if(parameters.TryGetValue(nameof(CustomPin), out CustomPin pin))
            {
                FillData(pin);
            }
            else
            {
                Debug.WriteLine("Missing parameters");
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
                Debug.WriteLine("Collection is empty");
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

        private async void OnSaveClickCommandAsync()
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
                            FavouriteImageSource = "empty_heart.png",
                            Category = PickerItem
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
                        MyFocusedPin.Category = PickerItem;

                        await _pinService.UpdatePinAsync(MyFocusedPin);
                    }

                    await _navigationService.GoBackAsync();
                }
                else
                {
                    string alertText = Resources.Resource.ExistNameAlert;
                    string button = Resources.Resource.OkButton;

                    await _userDialogs.AlertAsync(alertText, string.Empty, button);
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
                string alertText = Resources.Resource.PinNameAlert;
                string button = Resources.Resource.OkButton;

                await _userDialogs.AlertAsync(alertText, string.Empty, button);
            }
            else if (IsCheckBoxChecked)
            {
                if (!PinsCollection.Any())
                {
                    isValid = false;
                    string alertText = Resources.Resource.MapAlert;
                    string button = Resources.Resource.OkButton;

                    await _userDialogs.AlertAsync(alertText, string.Empty, button);
                }
                else
                {
                    Debug.WriteLine("Collection contains pin");
                }

            }
            else
            {
                isValid = await CheckLatLongAsync();
            }

            return isValid;
        }

        private async Task<bool> CheckLatLongAsync()
        {
            var isValid = true;

            if (LongitudeEntry > 180 || LongitudeEntry < 0)
            {
                isValid = false;
                string alertText = Resources.Resource.LongitudeAlert;
                string button = Resources.Resource.OkButton;

                await _userDialogs.AlertAsync(alertText, string.Empty, button);
            }
            else if (LatitudeEntry > 90 || LatitudeEntry < -90)
            {
                isValid = false;
                string alertText = Resources.Resource.LatitudeAlert;
                string button = Resources.Resource.OkButton;

                await _userDialogs.AlertAsync(alertText, string.Empty, button);
            }
            else
            {
                Debug.WriteLine("Longitude and latitude is ok");
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
            PickerItem = pin.Category;

            MyFocusedPin = pin;

            PinsCollection = new ObservableCollection<CustomPin>
            {
                pin
            };
        }

        private async void CheckLocationPermissionsAsync()
        {
            var status = await _permissionsService.CheckPermissionsAsync<LocationPermission>();

            if (status != PermissionStatus.Granted)
            {
                SetLocationEnable(false);
            }
            else
            {
                SetLocationEnable(true);
            }

        }
        private void SetLocationEnable(bool isSet)
        {
            IsMyLocationEnabled = isSet;
        }

        #endregion
    }
}
