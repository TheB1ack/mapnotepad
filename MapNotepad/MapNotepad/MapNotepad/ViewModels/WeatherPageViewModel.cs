using MapNotepad.Extentions;
using MapNotepad.Models;
using MapNotepad.Services.Pins;
using MapNotepad.Services.WeatherService;
using MapNotepad.Views;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MapNotepad.ViewModels
{
    public class WeatherPageViewModel : ViewModelBase
    {
        private readonly IWeatherService _weatherService;
        private readonly IPinService _pinService;

        public WeatherPageViewModel(INavigationService navigationService,
                                    IWeatherService weatherService,
                                    IPinService pinService)
                                    : base(navigationService)
        {
            _weatherService = weatherService;
            _pinService = pinService;
        }

        #region-- Public properties --  

        private string _labetText;
        public string LabetText
        {
            get => _labetText;

            set => SetProperty(ref _labetText, value);
        }

        private bool _isShowLabel;
        public bool IsShowLabel
        {
            get => _isShowLabel;

            set => SetProperty(ref _isShowLabel, value);
        }

        private bool _isShowContent;
        public bool IsShowContent
        {
            get => _isShowContent;

            set => SetProperty(ref _isShowContent, value);
        }

        private string _day5Icon;
        public string Day5Icon
        {
            get => _day5Icon;

            set => SetProperty(ref _day5Icon, value);
        }

        private string _day5Date;
        public string Day5Date
        {
            get => _day5Date;

            set => SetProperty(ref _day5Date, value);
        }

        private string _day5Temp;
        public string Day5Temp
        {
            get => _day5Temp;

            set => SetProperty(ref _day5Temp, value);
        }

        private string _day4Date;
        public string Day4Date
        {
            get => _day4Date;

            set => SetProperty(ref _day4Date, value);
        }

        private string _day4Temp;
        public string Day4Temp
        {
            get => _day4Temp;

            set => SetProperty(ref _day4Temp, value);
        }

        private string _day4Icon;
        public string Day4Icon
        {
            get => _day4Icon;

            set => SetProperty(ref _day4Icon, value);
        }

        private string _day3Date;
        public string Day3Date
        {
            get => _day3Date;

            set => SetProperty(ref _day3Date, value);
        }

        private string _day3Temp;
        public string Day3Temp
        {
            get => _day3Temp;

            set => SetProperty(ref _day3Temp, value);
        }

        private string _day3Icon;
        public string Day3Icon
        {
            get => _day3Icon;

            set => SetProperty(ref _day3Icon, value);
        }

        private string _day2Date;
        public string Day2Date
        {
            get => _day2Date;

            set => SetProperty(ref _day2Date, value);
        }

        private string _day2Temp;
        public string Day2Temp
        {
            get => _day2Temp;

            set => SetProperty(ref _day2Temp, value);
        }

        private string _day2Icon;
        public string Day2Icon
        {
            get => _day2Icon;

            set => SetProperty(ref _day2Icon, value);
        }

        private string _day1PoP;
        public string Day1PoP
        {
            get => _day1PoP;

            set => SetProperty(ref _day1PoP, value);
        }

        private string _day1WindSpeed;
        public string Day1WindSpeed
        {
            get => _day1WindSpeed;

            set => SetProperty(ref _day1WindSpeed, value);
        }

        private string _day1Cloudiness;
        public string Day1Cloudiness
        {
            get => _day1Cloudiness;

            set => SetProperty(ref _day1Cloudiness, value);
        }

        private string _day1WeatherDesc;
        public string Day1WeatherDesc
        {
            get => _day1WeatherDesc;

            set => SetProperty(ref _day1WeatherDesc, value);
        }

        private string _day1Weather;
        public string Day1Weather
        {
            get => _day1Weather;

            set => SetProperty(ref _day1Weather, value);
        }

        private string _day1Date;
        public string Day1Date
        {
            get => _day1Date;

            set => SetProperty(ref _day1Date, value);
        }

        private string _day1Temp;
        public string Day1Temp
        {
            get => _day1Temp;

            set => SetProperty(ref _day1Temp, value);
        }

        private string _day1FillTemp;
        public string Day1FillTemp
        {
            get => _day1FillTemp;

            set => SetProperty(ref _day1FillTemp, value);
        }

        private string _day1MinTemp;
        public string Day1MinTemp
        {
            get => _day1MinTemp;

            set => SetProperty(ref _day1MinTemp, value);
        }

        private string _day1MaxTemp;
        public string Day1MaxTemp
        {
            get => _day1MaxTemp;

            set => SetProperty(ref _day1MaxTemp, value);
        }

        private List<string> _pickerSource;
        public List<string> PickerSource
        {
            get => _pickerSource;

            set => SetProperty(ref _pickerSource, value);
        }

        private string _selectedItem;
        public string SelectedItem
        {
            get => _selectedItem;

            set => SetProperty(ref _selectedItem, value);
        }

        private ICommand _selectedItemChangedCommand;
        public ICommand SelectedItemChangedCommand => _selectedItemChangedCommand ??= new Command(OnSelectedItemChangedCommand);

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

            string text = Resources.Resource.EmptyListLabel;
            LabetText = text;
            IsShowLabel = true;
            IsShowContent = false;

            FillPicker();
        }

        #endregion

        #region -- Private Helpers --

        private async void FillPicker()
        {
            var items = await _pinService.GetPinsAsync();
            var pickerItems = new List<string>();

            foreach (var item in items)
            {
                pickerItems.Add(item.Name);
            }

            PickerSource = pickerItems;
        }

        private async void OnSelectedItemChangedCommand()
        {
            var items = await _pinService.GetPinsAsync();
            var pin = items.Where(x => x.Name == SelectedItem).FirstOrDefault();

            await ShowWether(pin);
        }

        private async Task ShowWether(CustomPin pin)
        {
            if (pin != null)
            {
                var current = Connectivity.NetworkAccess;

                if (current == NetworkAccess.Internet)
                {
                    var weatherResult = await _weatherService.GetFiveDaysWeater(pin.PositionLat, pin.PositionLong);
                    var days = weatherResult.ToList();

                    SetDay1Forcast(days[0]);
                    SetAllDaysForcast(days);

                    IsShowLabel = false;
                    IsShowContent = true;
                }
                else
                {
                    string text = Resources.Resource.InternetLabel;
                    LabetText = text;
                    IsShowLabel = true;
                    IsShowContent = false;
                }

            }
            else
            {
                IsShowLabel = true;
                IsShowContent = false;
            }

        }

        private void SetAllDaysForcast(List<WeatherInfo> list)
        {
            Day2Date = Convert.ToDateTime(list[1].Date).ToString("dddd", CultureInfo.CreateSpecificCulture("en-US"));
            Day3Date = Convert.ToDateTime(list[2].Date).ToString("dddd", CultureInfo.CreateSpecificCulture("en-US"));
            Day4Date = Convert.ToDateTime(list[3].Date).ToString("dddd", CultureInfo.CreateSpecificCulture("en-US"));
            Day5Date = Convert.ToDateTime(list[4].Date).ToString("dddd", CultureInfo.CreateSpecificCulture("en-US"));

            Day2Temp = ConvertToIntC(list[1].TemperatureInfo.Value).ToString() + "°C";
            Day3Temp = ConvertToIntC(list[2].TemperatureInfo.Value).ToString() + "°C";
            Day4Temp = ConvertToIntC(list[3].TemperatureInfo.Value).ToString() + "°C";
            Day5Temp = ConvertToIntC(list[4].TemperatureInfo.Value).ToString() + "°C";

            Day2Icon = SetIconURL(list[1].DaysWeather.First().Icon);
            Day3Icon = SetIconURL(list[2].DaysWeather.First().Icon);
            Day4Icon = SetIconURL(list[3].DaysWeather.First().Icon);
            Day5Icon = SetIconURL(list[4].DaysWeather.First().Icon);
        }

        private void SetDay1Forcast(WeatherInfo list)
        {
            Day1Date = Convert.ToDateTime(list.Date).ToString("dddd", CultureInfo.CreateSpecificCulture("en-US"));

            Day1Temp = ConvertToIntC(list.TemperatureInfo.Value).ToString() + "°C";
            Day1FillTemp = "Fills like - " + ConvertToIntC(list.TemperatureInfo.FeelsLike).ToString() + "°C";
            Day1MinTemp = ConvertToIntC(list.TemperatureInfo.Minimum).ToString() + "°C";
            Day1MaxTemp = ConvertToIntC(list.TemperatureInfo.Maximum).ToString() + "°C";

            Day1Weather = list.DaysWeather.First().Name;
            Day1WeatherDesc = list.DaysWeather.First().Description;

            Day1Cloudiness = list.Clouds.Cloudiness.ToString();

            Day1WindSpeed = list.Wind.Speed.ToString() + "m/s";

            Day1PoP = list.PoP.ToString();
        }

        private int ConvertToIntC(double K)
        {
            var C = K - 273.15;

            return Convert.ToInt32(C);
        }

        private void GoToSelectedPin(CustomPin pin)
        {
            var parameters = new NavigationParameters
            {
                 { nameof(CustomPin), pin }
            };

            _navigationService.FixedSelectTab(typeof(MapPage), parameters);
        }

        private string SetIconURL(string icon)
        {
            return Constants.WEATHER_ICONS + icon + ".png";   
        }

        #endregion

    }
}
