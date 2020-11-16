using Acr.UserDialogs;
using MapNotepad.Enums;
using MapNotepad.Models;
using MapNotepad.Services.Map;
using MapNotepad.Services.Permission;
using MapNotepad.Services.Pins;
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
    public class MapPageViewModel : ViewModelBase
    {
        private readonly IPinService _pinService;
        private readonly IMapService _mapService;
        private readonly IPermissionsService _permissionsService;
        private readonly IUserDialogs _userDialogs;

        public MapPageViewModel(INavigationService navigationService,
                                IPinService pinService,
                                IMapService mapService,
                                IPermissionsService permissionsService,
                                IUserDialogs userDialogs)
                                : base(navigationService)
        {
            _pinService = pinService;
            _mapService = mapService;
            _permissionsService = permissionsService;
            _userDialogs = userDialogs;

            PinsCollection = new ObservableCollection<CustomPin>();
        }

        #region -- Public properties --

        private int _selectedIndex;
        public int SelectedIndex
        {
            get => _selectedIndex;
            set => SetProperty(ref _selectedIndex, value);
        }

        private bool _isSettingFrameVisible;
        public bool IsSettingFrameVisible
        {
            get => _isSettingFrameVisible;
            set => SetProperty(ref _isSettingFrameVisible, value);
        }

        private bool _isFrameShowed;
        public bool IsFrameShowed
        {
            get => _isFrameShowed;
            set => SetProperty(ref _isFrameShowed, value);
        }

        private CustomPin _myFocusedPin;
        public CustomPin MyFocusedPin
        {
            get => _myFocusedPin;
            set => SetProperty(ref _myFocusedPin, value);
        }

        private ObservableCollection<CustomPin> _pinsCollection;
        public ObservableCollection<CustomPin> PinsCollection
        {
            get => _pinsCollection;
            set => SetProperty(ref _pinsCollection, value);
        }

        private string _searchBarText;
        public string SearchBarText
        {
            get => _searchBarText;
            set => SetProperty(ref _searchBarText, value);
        }

        private CameraPosition _cameraPositionBinding;
        public CameraPosition CameraPositionBinding
        {
            get => _cameraPositionBinding;
            set => SetProperty(ref _cameraPositionBinding, value);
        }

        private string _frameNameLable;
        public string FrameNameLable
        {
            get => _frameNameLable;
            set => SetProperty(ref _frameNameLable, value);
        }

        private string _frameDescriptionLabel;
        public string FrameDescriptionLabel
        {
            get => _frameDescriptionLabel;
            set => SetProperty(ref _frameDescriptionLabel, value);
        }

        private string _frameLatitudeLabel;
        public string FrameLatitudeLabel
        {
            get => _frameLatitudeLabel;
            set => SetProperty(ref _frameLatitudeLabel, value);
        }

        private string _frameLongitudeLabel;
        public string FrameLongitudeLabel
        {
            get => _frameLongitudeLabel;
            set => SetProperty(ref _frameLongitudeLabel, value);
        }

        private bool _isMyLocationEnabled;
        public bool IsMyLocationEnabled
        {
            get => _isMyLocationEnabled;
            set => SetProperty(ref _isMyLocationEnabled, value);
        }

        private ICommand _settingsClickCommand;
        public ICommand SettingsClickCommand => _settingsClickCommand ??= new Command(OnSettingsClickCommand);

        private ICommand _userSearchingCommand;
        public ICommand UserSearchingCommand => _userSearchingCommand ??= new Command(OnUserSearchingCommandAsync);

        private ICommand _cameraChangedCommand;
        public ICommand CameraChangedCommand => _cameraChangedCommand ??= new Command<CameraPosition>(OnCameraChangedCommand);

        private ICommand _pinClickCommand;
        public ICommand PinClickCommand => _pinClickCommand ??= new Command<Pin>(OnPinClickCommandAsync);

        private ICommand _mapClickCommand;
        public ICommand MapClickCommand => _mapClickCommand ??= new Command(OnMapClickCommand);

        private ICommand _saveClickCommand;
        public ICommand SaveClickCommand => _saveClickCommand ??= new Command(OnSaveClickCommand);

        #endregion

        #region -- ViewModelBase implementation --

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.TryGetValue(nameof(CustomPin), out CustomPin pin))
            {
                MyFocusedPin = pin;
            }
            else
            {
                SetSavedPosition();
            }

            IsFrameShowed = false;
            IsSettingFrameVisible = false;
            SelectedIndex = 0;

            ResizeCollectionAsync();
        }

        public async override void Initialize(INavigationParameters parameters)
        {
            var current = Connectivity.NetworkAccess;

            if (current == NetworkAccess.Internet)
            {
                await CheckLocationPermissionsAsync();
            }
            else
            {
                IsMyLocationEnabled = false;
            }

        }

        #endregion

        #region -- Private helpers --

        private void OnSaveClickCommand()
        {
            IsSettingFrameVisible = false;

            OnUserSearchingCommandAsync();
        }

        private void OnSettingsClickCommand()
        {
            IsSettingFrameVisible = true;
        }

        private void OnMapClickCommand()
        {
            if (IsFrameShowed)
            {
                IsFrameShowed = false;
            }
            else
            {
                Debug.WriteLine("IsFrameShowed is false");
            }

        }

        private async Task CheckLocationPermissionsAsync()
        {
            var status = await _permissionsService.CheckPermissionsAsync<Permissions.LocationWhenInUse>();

            if (status != PermissionStatus.Granted)
            {
                await SetLocationPermissionsAsync();
            }
            else
            {
                IsMyLocationEnabled = true;
            }

        }

        private async Task SetLocationPermissionsAsync()
        {
            var status = await _permissionsService.RequestPermissionsAsync<Permissions.LocationWhenInUse>();

            if (status != PermissionStatus.Granted)
            {
                IsMyLocationEnabled = false;
                string alertText = Resources.Resource.LocationAlert;
                string button = Resources.Resource.OkButton;

                await _userDialogs.AlertAsync(alertText, string.Empty, button);
            }
            else
            {
                IsMyLocationEnabled = true;
            }

        }

        private async void OnPinClickCommandAsync(Pin pin)
        {
            var items = await _pinService.GetPinsAsync();
            var tappedPin = items.FirstOrDefault(x => x.Name == pin.Label);

            if (tappedPin != null)
            {
                FrameNameLable = tappedPin.Name;
                FrameDescriptionLabel = tappedPin.Description;
                FrameLatitudeLabel = tappedPin.PositionLat.ToString();
                FrameLongitudeLabel = tappedPin.PositionLong.ToString();

                IsFrameShowed = true;
            }
            else
            {
                Debug.WriteLine("tappedPin is null");
            }

        }

        private void OnCameraChangedCommand(CameraPosition position)
        {
            _mapService.SaveMapPosition(position);
        }

        private void SetSavedPosition()
        {
            CameraPositionBinding = _mapService.GetSavedMapPosition();
        }

        private async void OnUserSearchingCommandAsync()
        {
            var items = await _pinService.GetPinsByTextAsync(SearchBarText, (SearchCategories)SelectedIndex);
            PinsCollection = new ObservableCollection<CustomPin>(items.Where(x => x.IsFavourite));
        }

        private async void ResizeCollectionAsync()
        {
            var items = await _pinService.GetPinsAsync();
            PinsCollection = new ObservableCollection<CustomPin>(items.Where(x => x.IsFavourite));
        }

        #endregion

    }
}
