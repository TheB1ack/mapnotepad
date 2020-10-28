using MapNotepad.Services.Pin;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.Internals;

namespace MapNotepad.ViewModels
{
    public class AddEditPinPageViewModel : ViewModelBase
    {
        IPinService _pinService;

        private ObservableCollection<Pin> _pinsCollection;
        public ObservableCollection<Pin> PinsCollection
        {
            get { return _pinsCollection; }
            set
            {
                SetProperty(ref _pinsCollection, value);
            }
        }

        private string _nameEntry;
        public string NameEntry
        {
            get { return _nameEntry; }
            set
            {
                SetProperty(ref _nameEntry, value);
            }
        }
        private string _descriptionEditor;
        public string DescriptionEditor
        {
            get { return _descriptionEditor; }
            set
            {
                SetProperty(ref _descriptionEditor, value);
            }
        }
       
        public ICommand MapTapped => new Command<Position>(OnMapClicked);
        public ICommand SaveClick => new Command(SavePin);
        public AddEditPinPageViewModel(INavigationService navigationService, IPinService pinService) : base(navigationService) 
        {
            _pinService = pinService;
            PinsCollection = new ObservableCollection<Pin>();
        }
        private void OnMapClicked(Position item)
        {
            Pin pin = new Pin()
            {
                Label = NameEntry ?? "",
                Position = item
            };

            PinsCollection = new ObservableCollection<Pin>(){ pin };
        }
        private async void SavePin()
        {
            await _pinService.AddPinAsync(NameEntry, DescriptionEditor, PinsCollection.FirstOrDefault<Pin>());
            await _navigationService.GoBackAsync();
        }
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
           
        }

    }
}
