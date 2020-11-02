using MapNotepad.Models;
using MapNotepad.Services.Map;
using MapNotepad.Services.Pins;
using Prism.Navigation;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.Internals;

namespace MapNotepad.ViewModels
{
    public class MapPageViewModel : ViewModelBase
    {
        private readonly IPinService _pinService;
        private readonly IMapService _mapService;


        public MapPageViewModel(INavigationService navigationService, IPinService pinService, IMapService mapService) : base(navigationService)
        {
            _pinService = pinService;
            _mapService = mapService;
            PinsCollection = new ObservableCollection<CustomPin>();
        }

        #region -- Public properties --

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

        private string _searchBarText;
        public string SearchBarText
        {
            get 
            { 
                return _searchBarText; 
            }
            set
            {
                SetProperty(ref _searchBarText, value);
            }
        }
        private CameraPosition _cameraPositionBinding;
        public CameraPosition CameraPositionBinding
        {
            get
            {
                return _cameraPositionBinding;
            }
            set
            {
                SetProperty(ref _cameraPositionBinding, value);
            }
        }
        public ICommand UserSearching => new Command(SearchPinsAsync);
        public ICommand OnCameraChangedBinding => new Command<CameraPosition>(SavePosition);

        #endregion

        #region -- IterfaceName implementation --

        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            SetSavedPosition();
            await SetMapPinsAsync();
            if (parameters.ContainsKey("FocusedPin"))
            {
                MyFocusedPin = (CustomPin)parameters["FocusedPin"];
            }
        }

        #endregion

        #region -- Private helpers --

        private void SavePosition(CameraPosition Position)
        {
            _mapService.SaveMapPosition(Position);
        }
        private void SetSavedPosition()
        {
            CameraPositionBinding = _mapService.GetSavedMapPosition();
        }
        private async Task SetMapPinsAsync()
        {
            var items = await _pinService.GetPinsAsync();
            var favouriteItems = items.Where(x => x.IsFavourite == true).ToList();

            PinsCollection = new ObservableCollection<CustomPin>(favouriteItems);
        }
        private async void SearchPinsAsync()
        {
            if (!string.IsNullOrWhiteSpace(SearchBarText))
            {
                PinsCollection = await _pinService.GetPinsByTextAsync(SearchBarText);
            }
            else
            {
                SetMapPinsAsync();
            }
        }

        #endregion

    }
}
