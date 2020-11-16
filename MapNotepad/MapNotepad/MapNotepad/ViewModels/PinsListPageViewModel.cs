﻿using MapNotepad.Enums;
using MapNotepad.Extentions;
using MapNotepad.Models;
using MapNotepad.Services.Pins;
using MapNotepad.Views;
using Newtonsoft.Json;
using Prism.Navigation;
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

        private string _barcodeValue;
        public string BarcodeValue
        {
            get => _barcodeValue;

            set => SetProperty(ref _barcodeValue, value);
        }

        private bool _isQrFrameVisible;
        public bool IsQrFrameVisible
        {
            get => _isQrFrameVisible;

            set => SetProperty(ref _isQrFrameVisible, value);
        }

        private bool _isVisibleButton;
        public bool IsVisibleButton
        {
            get => _isVisibleButton;

            set => SetProperty(ref _isVisibleButton, value);
        }

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

        private string _searchBarText;
        public string SearchBarText
        {
            get => _searchBarText;

            set => SetProperty(ref _searchBarText, value);
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

        private ICommand _shareTapCommand;
        public ICommand ShareTapCommand => _shareTapCommand ??= new Command<CustomPin>(OnShareTapCommand);

        private ICommand _closeFrameCommand;
        public ICommand CloseFrameCommand => _closeFrameCommand ??= new Command(OnCloseFrameCommand);

        #endregion

        #region -- ViewModelBase implementation --

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.TryGetValue(nameof(CustomPin), out CustomPin pin))
            {
                GoToSelectedPin(pin);
            }
            else
            {
                Debug.WriteLine("parameters are empty");
            }

            SelectedIndex = 0;
            IsQrFrameVisible = false;
            IsSettingFrameVisible = false;
            IsVisibleButton = true;

            ResizeCollectionAsync();
        }

        #endregion

        #region -- Private helpers --

        private void OnCloseFrameCommand()
        {
            IsQrFrameVisible = false;
            IsVisibleButton = true;
        }

        private void OnShareTapCommand(CustomPin pin)
        {
            var saveString = JsonConvert.SerializeObject(pin);
            BarcodeValue = saveString;

            IsQrFrameVisible = true;
            IsVisibleButton = false;
        }

        private async void ResizeCollectionAsync()
        {
            var items = await _pinService.GetPinsAsync();
            PinsCollection = new ObservableCollection<CustomPin>(items);

            CheckCollectionSize();
        }

        private void OnSaveClickCommand()
        {
            IsSettingFrameVisible = false;
            IsVisibleButton = true;

            OnUserSearchingCommandAsync();
        }

        private void OnSettingsClickCommand()
        {
            IsSettingFrameVisible = true;
            IsVisibleButton = false;
        }

        private async void OnImageTapCommandAsync(CustomPin pin)
        {
            if (pin.IsFavourite)
            {
                pin.FavouriteImageSource = Resources.Resource.EmptyHeartImage;
            }
            else
            {
                pin.FavouriteImageSource = Resources.Resource.FullHeartImage;
            }

            pin.IsFavourite = !pin.IsFavourite;
            await _pinService.UpdatePinAsync(pin);

            OnUserSearchingCommandAsync();
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

        private async void OnItemTappedCommandAsync(object selectedItem)
        {
            if (selectedItem is CustomPin pin)
            {
                if (!pin.IsFavourite)
                {
                    pin.FavouriteImageSource = Resources.Resource.FullHeartImage;
                    pin.IsFavourite = true;

                    await _pinService.UpdatePinAsync(pin);
                }
                else
                {
                    Debug.WriteLine("pin is favourite");
                }

                GoToSelectedPin(pin);
            }
            else
            {
                Debug.WriteLine("selected pin isn't CustomPin");
            }

        }

        private void GoToSelectedPin(CustomPin pin)
        {
            var parameters = new NavigationParameters
            {
                 { nameof(CustomPin), pin }
            };

            _navigationService.FixedSelectTab(typeof(MapPage), parameters);
        }

        #endregion

    }
}
