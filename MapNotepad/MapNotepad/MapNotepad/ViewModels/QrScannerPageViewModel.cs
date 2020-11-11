using Acr.UserDialogs;
using MapNotepad.Models;
using MapNotepad.Services.Pins;
using Newtonsoft.Json;
using Prism.Navigation;
using System.Diagnostics;
using System.Windows.Input;
using Xamarin.Forms;
using ZXing;

namespace MapNotepad.ViewModels
{
    public class QrScannerPageViewModel : ViewModelBase
    {
        private readonly IPinService _pinService;
        private readonly IUserDialogs _userDialogs;

        public QrScannerPageViewModel(INavigationService navigationService,
                                      IPinService pinService,
                                      IUserDialogs userDialogs)
                                      : base(navigationService)
        {
            _pinService = pinService;
            _userDialogs = userDialogs;
        }

        #region -- Public properties --

        private Result _scanResult;
        public Result ScanResult
        {
            get => _scanResult;

            set => SetProperty(ref _scanResult, value);
        }

        private ICommand _scanResultCommand;
        public ICommand ScanResultCommand => _scanResultCommand ??= new Command(OnScanResultCommand);


        #endregion

        #region -- Private helpers --

        private async void OnScanResultCommand()
        {
            if (ScanResult != null)
            {
                var pin = TryGetPin(ScanResult.Text);
                if (pin != null)
                {
                    var isValid = await _pinService.CheckPinName(pin.Name);
                    if (isValid)
                    {
                        await _pinService.AddPinAsync(pin);

                        var parameters = new NavigationParameters
                        {
                            {nameof(CustomPin), pin }
                        };

                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            await _navigationService.GoBackAsync(parameters);
                        });
                        ScanResult = null;
                    }
                    else
                    {
                        string alertText = Resources.Resource.ScanPinNameAlert;
                        string button = Resources.Resource.OkButton;

                        await _userDialogs.AlertAsync(alertText, string.Empty, button);
                    }
                }
                else
                {
                    string alertText = Resources.Resource.InvalidQrAlert;
                    string button = Resources.Resource.OkButton;

                    await _userDialogs.AlertAsync(alertText, string.Empty, button);
                }
            }
    
        }

        private CustomPin TryGetPin(string text)
        {
            CustomPin pin = null;
            try
            {
                pin = JsonConvert.DeserializeObject<CustomPin>(text);
                pin.IsFavourite = true;
                pin.FavouriteImageSource = "full_heart.png";
            }
            catch(JsonException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return pin;
        }
        #endregion
    }
}
