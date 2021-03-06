﻿using Acr.UserDialogs;
using MapNotepad.Models;
using MapNotepad.Services.Map;
using MapNotepad.Services.Permissions;
using MapNotepad.Services.Pins;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
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

        private bool _isVisibleFrame;
        public bool IsVisibleFrame
        {
            get => _isVisibleFrame;

            set => SetProperty(ref _isVisibleFrame, value);
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
       
        private bool _myLocationEnabled;
        public bool MyLocationEnabled
        {
            get => _myLocationEnabled;

            set => SetProperty(ref _myLocationEnabled, value);
        }

        private ICommand _userSearchingCommand;
        public ICommand UserSearchingCommand => _userSearchingCommand ??= new Command(OnUserSearchingCommandAsync);

        private ICommand _cameraChangedCommand;
        public ICommand CameraChangedCommand => _cameraChangedCommand ??= new Command<CameraPosition>(OnCameraChangedCommand);

        private ICommand _closeFrameCommand;
        public ICommand CloseFrameCommand => _closeFrameCommand ??= new Command(OnCloseFrameCommand);

        private ICommand _pinClickCommand;
        public ICommand PinClickCommand => _pinClickCommand ??= new Command<Pin>(OnPinClickCommand);

        private ICommand _mapClickCommand;
        public ICommand MapClickCommand => _mapClickCommand ??= new Command(OnMapClickCommand);

        #endregion

        #region -- IterfaceName implementation --

        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            SetSavedPosition();
            await SetMapPinsAsync();

            if (parameters.TryGetValue(nameof(CustomPin), out CustomPin pin))
            {
                MyFocusedPin = pin;
            }
            else
            {
                Debug.WriteLine("Parameters are missing CustomPin");
            }

        }

        public override void Initialize(INavigationParameters parameters)
        {
            SetLocationAsync();
        }

        #endregion

        #region -- Private helpers --

        private async void SetLocationAsync()
        {
            var status = await _permissionsService.RequestPermissionsAsync<LocationPermission>();

            if (status != PermissionStatus.Granted)
            {
                var result = await _permissionsService.ShowRequestPermissionRationaleAsync(Permission.Location);

                if (result)
                {
                    await _userDialogs.AlertAsync("App needs your location to work correctly!", string.Empty, "I understand");
                }
                else
                {
                    Debug.WriteLine("result was false");
                }

                status = await CrossPermissions.Current.RequestPermissionAsync<LocationPermission>();
            }
            else
            {
                Debug.WriteLine("status wasn't Granted");
            }

            if (status == PermissionStatus.Granted)
            {
                MyLocationEnabled = true;
            }
            else if (status != PermissionStatus.Unknown)
            {
                MyLocationEnabled = false;
            }
            else
            {
                Debug.WriteLine("status was Unknown");
            }

        }
        private void OnMapClickCommand()
        {
            if(IsVisibleFrame)
            {
                IsVisibleFrame = false;
            }
            else
            {
                Debug.WriteLine("IsVisibleFrame was false");
            }

        }
        private async void OnPinClickCommand(Pin pin)
        {
            var items = await _pinService.GetPinsAsync();
            var tappedPin = items.FirstOrDefault(x => x.Name == pin.Label);

            if (tappedPin != null)
            {
                FrameNameLable = tappedPin.Name;
                FrameDescriptionLabel = tappedPin.Description;
                FrameLatitudeLabel = tappedPin.PositionLat.ToString();
                FrameLongitudeLabel = tappedPin.PositionLong.ToString();
                IsVisibleFrame = true;
            }
            else
            {
                Debug.WriteLine("Searched pin by name eas null");
            }

        }
        private void OnCloseFrameCommand()
        {
            IsVisibleFrame = false;
        }
        private void OnCameraChangedCommand(CameraPosition position)
        {
            _mapService.SaveMapPosition(position);
        }
        private void SetSavedPosition()
        {
            CameraPositionBinding = _mapService.GetSavedMapPosition();
        }
        private async Task SetMapPinsAsync()
        {
            var items = await _pinService.GetPinsAsync();
            var favouriteItems = items.Where(x => x.IsFavourite);

            PinsCollection = new ObservableCollection<CustomPin>(favouriteItems);
        }
        private async void OnUserSearchingCommandAsync()
        {
            if (!string.IsNullOrWhiteSpace(SearchBarText))
            {
                var items = await _pinService.GetPinsByTextAsync(SearchBarText);
                PinsCollection = new ObservableCollection<CustomPin>(items);
            }
            else
            {
                await SetMapPinsAsync();
            }
        }

        #endregion

    }
}
