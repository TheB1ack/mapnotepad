using MapNotepad.Models;
using MapNotepad.Services.Pins;
using MapNotepad.Views;
using Prism.Navigation;
using System.Collections;
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
    public class PinsListPageViewModel : ViewModelBase
    {
        private readonly IPinService _pinService;

        public PinsListPageViewModel(INavigationService navigationService,
                                     IPinService pinService)
                                     : base(navigationService)
        {
            _pinService = pinService;
        }

        #region -- Public properties --

        private ObservableCollection<CustomPin> _pinsCollection;
        public ObservableCollection<CustomPin> PinsCollection
        {
            get => _pinsCollection;

            set => SetProperty(ref _pinsCollection, value);
        }

        private bool _isVisibleText;
        public bool IsVisibleText
        {
            get => _isVisibleText;

            set => SetProperty(ref _isVisibleText, value);
        }

        private CustomPin _itemSelected;
        public CustomPin ItemSelected
        {
            get => _itemSelected;

            set => _itemSelected = value;
        }

        private string _searchBarText;
        public string SearchBarText
        {
            get => _searchBarText;

            set => SetProperty(ref _searchBarText, value);
        }

        private bool _isCheckBoxChecked;
        public bool IsCheckBoxChecked
        {
            get => _isCheckBoxChecked;

            set => SetProperty(ref _isCheckBoxChecked, value);
        }

        private ICommand _editTapCommand;
        public ICommand EditTapCommand => _editTapCommand ??= new Command<CustomPin>(OnEditTapCommand);

        private ICommand _deleteTapCommand;
        public ICommand DeleteTapCommand => _deleteTapCommand ??= new Command<CustomPin>(OnDeleteTapCommand);

        private ICommand _addButtonClickCommand;
        public ICommand AddButtonClickCommand => _addButtonClickCommand ??= new Command(OnAddButtonClickCommand);

        private ICommand _userSearchingCommand;
        public ICommand UserSearchingCommand => _userSearchingCommand ??= new Command(OnUserSearchingCommand);

        private ICommand _imageTapCommand;
        public ICommand ImageTapCommand => _imageTapCommand ??= new Command<CustomPin>(OnImageTapCommand);

        private ICommand _itemTappedCommand;
        public ICommand ItemTappedCommand => _itemTappedCommand ??= new Command(OnItemTappedCommand);

        #endregion

        #region -- IterfaceName implementation --

        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            await CollectionResizeAsync();
        }

        #endregion

        #region -- Private helpers --

        private async void OnImageTapCommand(CustomPin pin)
        {
            if (pin.IsFavourite)
            {
                pin.FavouriteImageSource = "empty_heart.png";
            }
            else
            {
                pin.FavouriteImageSource = "full_heart.png";
            }

            pin.IsFavourite = !pin.IsFavourite;

            await _pinService.UpdatePinAsync(pin);
            var items = await _pinService.GetPinsAsync();

            PinsCollection = new ObservableCollection<CustomPin>(items);
        }
        private async void OnAddButtonClickCommand()
        {
            await _navigationService.NavigateAsync($"{nameof(AddEditPinPage)}");
        }
        private async void OnEditTapCommand(CustomPin pin)
        {
            NavigationParameters parameters = new NavigationParameters
            {
                { nameof(CustomPin), pin }
            };

            await _navigationService.NavigateAsync($"{nameof(AddEditPinPage)}", parameters);
        }
        private async void OnDeleteTapCommand(CustomPin pin)
        {
            await _pinService.RemovePinAsync(pin);

            await CollectionResizeAsync();
        }
        private async void OnUserSearchingCommand()
        {
            IEnumerable<CustomPin> items;

            if (!string.IsNullOrWhiteSpace(SearchBarText))
            {
                items = await _pinService.GetPinsByTextAsync(SearchBarText);

                CheckCollectionSize();
            }
            else
            {
                items = await _pinService.GetPinsAsync();
            }

            PinsCollection = new ObservableCollection<CustomPin>(items);
        }
        private async Task CollectionResizeAsync()
        {
            var items = await _pinService.GetPinsAsync();
            PinsCollection = new ObservableCollection<CustomPin>(items);

            CheckCollectionSize();
        }
        private void CheckCollectionSize()
        {
            if (!PinsCollection.Any())
            {
                IsVisibleText = true;
            }
            else
            {
                IsVisibleText = false;
            }
        }
        private async void OnItemTappedCommand()
        {
            if (ItemSelected != null)
            {
                ItemSelected.IsAnimated = true;

                NavigationParameters parameters = new NavigationParameters
                {
                    {nameof(CustomPin), ItemSelected }
                };             

                await _navigationService.NavigateAsync($"{nameof(NavigationPage)}/{nameof(MainPage)}?selectedTab={nameof(MapPage)}", parameters);
            }
            else
            {
                Debug.WriteLine("ItemSelected was null");
            }
        }

        #endregion

    }
}
