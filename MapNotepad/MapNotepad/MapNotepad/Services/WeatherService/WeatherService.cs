using MapNotepad.Models;
using MapNotepad.Services.REST;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MapNotepad.Services.WeatherService
{
    public class WeatherService : IWeatherService
    {
        private readonly IRestService _restService;

        public WeatherService(IRestService restService)
        {
            _restService = restService;
        }

        #region -- IterfaceName implementation --

        public Task<WeatherModel> GetWeather(double latitude, double longitude)
        {
            var url = Constants.WEATHER_URL + $"{latitude}&lon={longitude}&appid={Constants.WEATHER_APIKEY}";

            return _restService.GetAsync<WeatherModel>(url);
        }

        public async Task<IEnumerable<List>> GetFiveDaysWeater(double latitude, double longitude)
        {
            List<List> fiveDaysForcast = new List<List>();

            var result = await GetWeather(latitude, longitude);

            var listOfAllDays = result.list;
            var nDaysForcast = listOfAllDays.Where(x => x.dt_txt.Contains("15:00:00")).ToList();

            if (nDaysForcast.Count() < 5)
            {
                fiveDaysForcast.Add(listOfAllDays.FirstOrDefault());
            }

            fiveDaysForcast.AddRange(nDaysForcast);

            return fiveDaysForcast;
        }

        #endregion

    }
}
