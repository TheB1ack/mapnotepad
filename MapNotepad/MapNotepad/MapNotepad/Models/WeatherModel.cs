using Newtonsoft.Json;
using System.Collections.Generic;

namespace MapNotepad.Models
{
    public class WeatherModel
    {
        [JsonProperty("list")]
        public List<WeatherInfo> WeatherList { get; set; }

    }

    public class Temperature
    {
        [JsonProperty("temp")]
        public double Value { get; set; }

        [JsonProperty("feels_like")]
        public double FeelsLike { get; set; }

        [JsonProperty("temp_min")]
        public double Minimum { get; set; }

        [JsonProperty("temp_max")]
        public double Maximum { get; set; }

    }

    public class Weather
    {
        [JsonProperty("main")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }

    }

    public class Clouds
    {
         [JsonProperty("all")]
        public int Cloudiness { get; set; }

    }

    public class Wind
    {
        [JsonProperty("speed")]
        public double Speed { get; set; }

    }


    public class WeatherInfo
    {
        [JsonProperty("main")]
        public Temperature TemperatureInfo { get; set; }

        [JsonProperty("weather")]
        public List<Weather> DaysWeather { get; set; }

        [JsonProperty("clouds")]
        public Clouds Clouds { get; set; }

        [JsonProperty("wind")]
        public Wind Wind { get; set; }

        [JsonProperty("pop")]
        public double PoP { get; set; }

        [JsonProperty("dt_txt")]
        public string Date { get; set; }

    }

}
