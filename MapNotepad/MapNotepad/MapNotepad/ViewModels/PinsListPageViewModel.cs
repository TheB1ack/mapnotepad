using MapNotepad.Enums;
using MapNotepad.Models;
using MapNotepad.Services.Pins;
using MapNotepad.Views;
using Prism.Navigation;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

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

        private bool _isVisibleList;
        public bool IsVisibleList
        {
            get => _isVisibleList;

            set => SetProperty(ref _isVisibleList, value);
        }

        private CustomPin _itemSelected;
        public CustomPin ItemSelected
        {
            get => _itemSelected;

            set => SetProperty(ref _itemSelected, value);
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
        public ICommand EditTapCommand => _editTapCommand ??= new Command<CustomPin>(OnEditTapCommandAsync);

        private ICommand _deleteTapCommand;
        public ICommand DeleteTapCommand => _deleteTapCommand ??= new Command<CustomPin>(OnDeleteTapCommandAsync);

        private ICommand _addButtonClickCommand;
        public ICommand AddButtonClickCommand => _addButtonClickCommand ??= new Command(OnAddButtonClickCommandAsync);

        private ICommand _userSearchingCommand;
        public ICommand UserSearchingCommand => _userSearchingCommand ??= new Command(OnUserSearchingCommandAsync);

        private ICommand _imageTapCommand;
        public ICommand ImageTapCommand => _imageTapCommand ??= new Command<CustomPin>(OnImageTapCommandAsync);

        private ICommand _itemTappedCommand;
        public ICommand ItemTappedCommand => _itemTappedCommand ??= new Command(OnItemTappedCommandAsync);

        private ICommand _saveClickCommand;
        public ICommand SaveClickCommand => _saveClickCommand ??= new Command(OnSaveClickCommand);

        private ICommand _settingsClickCommand;
        public ICommand SettingsClickCommand => _settingsClickCommand ??= new Command(OnSettingsClickCommand);

        #endregion

        #region -- IterfaceName implementation --

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            OnUserSearchingCommandAsync();
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

        private async void OnImageTapCommandAsync(CustomPin pin)
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

        private async void OnAddButtonClickCommandAsync()
        {
            NavigationParameters parameters = new NavigationParameters
            {
                { Constants.ACTION, "Add pin"}
            };
            await _navigationService.NavigateAsync($"{nameof(AddEditPinPage)}", parameters);
        }

        private async void OnEditTapCommandAsync(CustomPin pin)
        {
            NavigationParameters parameters = new NavigationParameters
            {
                { nameof(CustomPin), pin },
                { Constants.ACTION, "Edit pin"}
            };

            await _navigationService.NavigateAsync($"{nameof(AddEditPinPage)}", parameters);
        }

        private async void OnDeleteTapCommandAsync(CustomPin pin)
        {
            await _pinService.RemovePinAsync(pin);

            OnUserSearchingCommandAsync();
        }

        private async void OnUserSearchingCommandAsync()
        {
            var items = await _pinService.GetPinsByTextAsync(SearchBarText, (SearchCategories)SelectedIndex);
            PinsCollection = new ObservableCollection<CustomPin>(items);

            CheckCollectionSize();
        }

        //private async void CollectionResizeAsync()
        //{
        //    var items = await _pinService.GetPinsAsync();
        //    PinsCollection = new ObservableCollection<CustomPin>(items);

        //    CheckCollectionSize();
        //}

        private void CheckCollectionSize()
        {
            if (!PinsCollection.Any())
            {
                IsVisibleText = true;
                IsVisibleList = false;
            }
            else
            {
                IsVisibleText = false;
                IsVisibleList = true;
            }
        }

        private async void OnItemTappedCommandAsync()
        {
            if (ItemSelected != null)
            {
                if (!ItemSelected.IsFavourite)
                {
                    OnImageTapCommandAsync(ItemSelected);
                }
                else
                {
                    Debug.WriteLine("pin is favourite");
                }

                ItemSelected.IsAnimated = true;

                NavigationParameters parameters = new NavigationParameters
                {
                    {nameof(CustomPin), ItemSelected }
                };             

                await _navigationService.NavigateAsync($"/{nameof(NavigationPage)}/{nameof(MainPage)}?selectedTab={nameof(MapPage)}", parameters);
            }
            else
            {
                Debug.WriteLine("ItemSelected was null");
            }
        }

        #endregion

    }
}
