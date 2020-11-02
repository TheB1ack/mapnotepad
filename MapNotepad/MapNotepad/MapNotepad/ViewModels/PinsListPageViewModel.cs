using MapNotepad.Models;
using MapNotepad.Services.Pins;
using MapNotepad.Views;
using Prism.Navigation;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace MapNotepad.ViewModels
{
    public class PinsListPageViewModel : ViewModelBase
    {
        private readonly IPinService _pinService;
      
        public PinsListPageViewModel(INavigationService navigationService, IPinService pinService) : base(navigationService)
        {
            _pinService = pinService;
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

        private bool _isVisibleText;
        public bool IsVisibleText
        {
            get
            {
                return _isVisibleText;
            }
            set
            {
                SetProperty(ref _isVisibleText, value);
            }
        }

        private CustomPin _itemSelected;
        public CustomPin ItemSelected
        {
            get
            {
                return _itemSelected;
            }
            set
            {
                _itemSelected = value;
                NavigateToMapPageAsync();
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

        public ICommand EditTap => new Command(GoToAddEditPinPageAsync);
        public ICommand DeleteTap => new Command(TryToDeleteItemAsync);
        public ICommand AddButtonClicked => new Command(GoToAddEditPinPageAsync);
        public ICommand UserSearching => new Command(SearchPinsAsync);
        public ICommand ImageTapCommand => new Command(SwitchFavouriteAsync);

        #endregion

        #region -- IterfaceName implementation --

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            CollectionResizeAsync();
        }

        #endregion

        #region -- Private helpers --

        private async void SwitchFavouriteAsync(object item)
        {
            var pin = item as CustomPin;
            if(pin.IsFavourite)
            {
                pin.FavouriteImageSource = "empty_heart.png";
                pin.IsFavourite = !pin.IsFavourite;                
            }
            else
            {
                pin.FavouriteImageSource = "full_heart.png";
                pin.IsFavourite = !pin.IsFavourite;               
            }

            await _pinService.UpdatePinAsync(pin);
            PinsCollection = await _pinService.GetPinsAsync();
        }
        private async void GoToAddEditPinPageAsync(object item)
        {
            var pin = item as CustomPin;
            NavigationParameters parameters = new NavigationParameters();
            parameters.Add("pin", pin);

            await _navigationService.NavigateAsync($"{nameof(AddEditPinPage)}", parameters);    
        }
        private async void TryToDeleteItemAsync(object item)
        { 
            var pin = item as CustomPin;
            await _pinService.RemovePinAsync(pin);

            CollectionResizeAsync();
        }
        private async void SearchPinsAsync()
        {
            if (!string.IsNullOrWhiteSpace(SearchBarText))
            {
                PinsCollection = await _pinService.GetPinsByTextAsync(SearchBarText);
                CheckCollectionSize();
            }
            else
            {
                PinsCollection = await _pinService.GetPinsAsync();
            }
        }
        private async void CollectionResizeAsync()
        {
            PinsCollection = await _pinService.GetPinsAsync();

            CheckCollectionSize();
        }
        private void CheckCollectionSize()
        {
            if (PinsCollection.Count == 0)
            {
                IsVisibleText = true;
            }
            else
            {
                IsVisibleText = false;
            }
        }
        private async void NavigateToMapPageAsync()
        {
            if(ItemSelected != null)
            {
                NavigationParameters parameters = new NavigationParameters();
                ItemSelected.IsAnimated = true;
                parameters.Add("FocusedPin", ItemSelected);

                await _navigationService.NavigateAsync($"../{nameof(MainPage)}?selectedTab={nameof(MapPage)}", parameters);
            }
        }

        #endregion

    }
}
