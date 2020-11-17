using MapNotepad.Models;
using MapNotepad.Services.REST;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MapNotepad.Services.Weather
{
    public class WeatherService : IWeatherService
    {
        private readonly IRestService _restService;

        public WeatherService(IRestService restService)
        {
            _restService = restService;
        }

        #region -- IWeatherService implementation --

        public Task<WeatherModel> GetWeatherAsync(double latitude, double longitude)
        {
            var url = Constants.WEATHER_URL + $"{latitude}&lon={longitude}&appid={Constants.WEATHER_APIKEY}";

            return _restService.GetAsync<WeatherModel>(url);
        }

        public async Task<IEnumerable<WeatherInfo>> GetFiveDaysWeaterAsync(double latitude, double longitude)
        {
            List<WeatherInfo> fiveDaysForcast = new List<WeatherInfo>();

            var result = await GetWeatherAsync(latitude, longitude);

            if (result != null)
            {
                var listOfAllDays = result.WeatherList;
                var nDaysForcast = listOfAllDays.Where(x => x.Date.Contains("15:00:00")).ToList();

                if (nDaysForcast.Count() < 5)
                {
                    fiveDaysForcast.Add(listOfAllDays.FirstOrDefault());
                }
                else
                {
                    Debug.WriteLine("nDaysForcast.Count() is >= 5");
                }

                fiveDaysForcast.AddRange(nDaysForcast);
            }
            else
            {
                fiveDaysForcast = null;
            }

            return fiveDaysForcast;
        }

        #endregion

    }
}
