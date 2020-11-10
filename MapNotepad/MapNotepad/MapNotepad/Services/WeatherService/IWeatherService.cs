using MapNotepad.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MapNotepad.Services.WeatherService
{
    public interface IWeatherService
    {
        Task<WeatherModel> GetWeather(double latitude, double longitude);
        Task<IEnumerable<List>> GetFiveDaysWeater(double latitude, double longitude);
    }
}
