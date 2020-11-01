using MapNotepad.Models;
using MapNotepad.Services.PinsService;
using Prism.Navigation;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace MapNotepad.ViewModels
{
    public class MapPageViewModel : ViewModelBase
    {
        private readonly IPinService _pinService;
         
        public MapPageViewModel(INavigationService navigationService, IPinService pinService) : base(navigationService)
        {
            _pinService = pinService;
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
        public ICommand UserSearching => new Command(SearchPinsAsync);

        #endregion

        #region -- IterfaceName implementation --

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            SetMapPinsAsync();
            if (parameters.ContainsKey("FocusedPin"))
            {
                MyFocusedPin = (CustomPin)parameters["FocusedPin"];
            }
        }

        #endregion

        #region -- Private helpers --

        private async void SetMapPinsAsync()
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
