using SkyWeatherAPI.Models;

namespace SkyWeatherAPI.Services
{
    public interface IWeatherService
    {
        Task<FiveDayWeatherModel> GetFiveDayWeatherData(string city);
        Task<WeatherModel> GetWeatherData(string city);
    }
}
