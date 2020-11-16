using Acr.UserDialogs;
using MapNotepad.Models;
using MapNotepad.Services.Permission;
using MapNotepad.Services.Pins;
using MapNotepad.Validators;
using Prism.Navigation;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
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

        private CustomPin _existPin;
        public CustomPin ExistPin
        {
            get => _existPin;

            set => SetProperty(ref _existPin, value);
        }

        private CameraPosition _cameraPositionBinding;
        public CameraPosition CameraPositionBinding
        {
            get => _cameraPositionBinding;

            set => SetProperty(ref _cameraPositionBinding, value);
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

        #region -- ViewModelBase implementation --

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
                Debug.WriteLine("parameters are empty");
            }

            if (parameters.TryGetValue(nameof(CustomPin), out CustomPin pin))
            {
                FillData(pin);
            }
            else
            {
                Debug.WriteLine("parameters are empty");
            }

            SetCameraPosition(4.20);
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
            var latitudeResult = Validator.LatitudeValidator(LatitudeEntry);
            var longitudeResult = Validator.LongitudeValidator(LongitudeEntry);

            if (latitudeResult && longitudeResult)
            {
                var position = new Position(LatitudeEntry, LongitudeEntry);

                OnMapTappedCommand(position);
                SetCameraPosition(7.3);
            }
            else
            {
                Debug.WriteLine("latitude and longitude are invalid");
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
                    if (ExistPin == null)
                    {
                        CustomPin customPin = new CustomPin
                        {
                            Name = NameEntry,
                            Description = DescriptionEditor ?? string.Empty,
                            PositionLat = PinsCollection.First().PositionLat,
                            PositionLong = PinsCollection.First().PositionLong,
                            FavouriteImageSource = Resources.Resource.EmptyHeartImage,
                            Category = PickerItem
                        };

                        await _pinService.AddPinAsync(customPin);
                    }
                    else
                    {
                        ExistPin.Name = NameEntry;
                        ExistPin.Description = DescriptionEditor;
                        ExistPin.PositionLat = LatitudeEntry;
                        ExistPin.PositionLong = LongitudeEntry;
                        ExistPin.IsFavourite = false;
                        ExistPin.Category = PickerItem;

                        await _pinService.UpdatePinAsync(ExistPin);
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
                Debug.WriteLine("isValidEntry was null");
            }

        }

        private async Task<bool> EntryCheckAsync()
        {
            var isValid = true;

            var pinNameResult = Validator.PinNameValidator(NameEntry);
            var latitudeResult = Validator.LatitudeValidator(LatitudeEntry);
            var longitudeResult = Validator.LongitudeValidator(LongitudeEntry);

            if (!pinNameResult)
            {
                isValid = false;
                string alertText = Resources.Resource.PinNameAlert;
                string button = Resources.Resource.OkButton;

                await _userDialogs.AlertAsync(alertText, string.Empty, button);
            }
            else
            {
                if (!latitudeResult)
                {
                    isValid = false;
                    string alertText = Resources.Resource.LatitudeAlert;
                    string button = Resources.Resource.OkButton;

                    await _userDialogs.AlertAsync(alertText, string.Empty, button);
                }
                else if (!longitudeResult)
                {
                    isValid = false;
                    string alertText = Resources.Resource.LongitudeAlert;
                    string button = Resources.Resource.OkButton;

                    await _userDialogs.AlertAsync(alertText, string.Empty, button);
                }
                else
                {
                    isValid = true;
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
            PickerItem = pin.Category;

            ExistPin = pin;

            PinsCollection = new ObservableCollection<CustomPin>
            {
                pin
            };
        }

        private void SetCameraPosition(double zoom)
        {
            var position = new Position(LatitudeEntry, LongitudeEntry);
            CameraPositionBinding = new CameraPosition(position, zoom);
        }

        private async void CheckLocationPermissionsAsync()
        {
            var status = await _permissionsService.CheckPermissionsAsync<Permissions.LocationAlways>();

            if (status != PermissionStatus.Granted)
            {
                IsMyLocationEnabled = false;
            }
            else
            {
                IsMyLocationEnabled = true;
            }

        }

        #endregion

    }
}
