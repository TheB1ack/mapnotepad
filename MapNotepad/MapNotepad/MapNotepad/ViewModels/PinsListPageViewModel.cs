using MapNotepad.Models;
using MapNotepad.Services.Pin;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Unity;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace MapNotepad.ViewModels
{
    public class PinsListPageViewModel : ViewModelBase
    {
        IPinService _pinService;

        private ObservableCollection<CustomPin> _itemsCollection;
        public ObservableCollection<CustomPin> ItemsCollection
        {
            get { return _itemsCollection; }
            set
            {
                SetProperty(ref _itemsCollection, value);
            }
        }
        private bool _isVisibleText;
        public bool IsVisibleText
        {
            get { return _isVisibleText; }
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
            }
        }
        public ICommand EditTap => new Command(GoToAddEditPinPageAsync);
        public ICommand DeleteTap => new Command(TryToDeleteItem);
        public ICommand AddButtonClicked => new Command(GoToAddEditPinPageAsync);
        public PinsListPageViewModel(INavigationService navigationService, IPinService pinService) : base(navigationService)
        {
            _pinService = pinService;
        }
        private async void GoToAddEditPinPageAsync(object item = null)
        {
            await _navigationService.NavigateAsync("AddEditPinPage");
            
        }
        private void TryToDeleteItem(object item)
        {
            
        }
        private async void CollectionResize()
        {
            ItemsCollection = await _pinService.GetPinsAsync();

            if (ItemsCollection.Count == 0)
            {
                IsVisibleText = true;
            }
            else
            {
                IsVisibleText = false;
            }
        }
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            CollectionResize();           
        }
    }
}
