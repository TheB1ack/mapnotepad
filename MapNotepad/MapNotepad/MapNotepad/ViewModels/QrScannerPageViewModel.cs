using MapNotepad.Models;
using MapNotepad.Services.Pins;
using MapNotepad.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using ZXing;

namespace MapNotepad.ViewModels
{
    public class QrScannerPageViewModel : ViewModelBase
    {
        private readonly IPinService _pinService;

        public QrScannerPageViewModel(INavigationService navigationService,
                                      IPinService pinService) 
                                      : base(navigationService)
        {
            _pinService = pinService;
        }

        #region -- Public properties --

        private Result _scanResult;
        public Result ScanResult
        {
            get => _scanResult;

            set => SetProperty(ref _scanResult, value);
        }

        private ICommand _scanCommand;
        public ICommand ScanCommand => _scanCommand ??= new Command(OnScanCommand);

        #endregion

        #region -- Private helpers --

        private  async void OnScanCommand()
        {
            var pin = GetPin();
            await _pinService.AddPinAsync(pin);

            var parameters = new NavigationParameters
            {
                {nameof(pin), pin }
            };
            await _navigationService.NavigateAsync($"/{nameof(NavigationPage)}/{nameof(MainPage)}?selectedTab={nameof(MapPage)}", parameters);
        }

        private CustomPin GetPin()
        {
            var result = ScanResult.Text;
            var items = result.Split('|');

            var pin = new CustomPin
            {
                Name = items[0],
                Description = items[1],
                Category = Int32.Parse(items[3]),
                PositionLat = Double.Parse(items[4]),
                PositionLong = Double.Parse(items[5]),
                IsFavourite = true,
                FavouriteImageSource = "empty_heart.png"
            };

            return pin;
        }
        #endregion
    }
}
