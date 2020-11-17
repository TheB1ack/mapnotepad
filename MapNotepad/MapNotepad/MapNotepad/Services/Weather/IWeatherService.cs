using MapNotepad.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MapNotepad.Services.Weather
{
    public interface IWeatherService
    {
        Task<WeatherModel> GetWeatherAsync(double latitude, double longitude);

        Task<IEnumerable<WeatherInfo>> GetFiveDaysWeaterAsync(double latitude, double longitude);

    }
}
