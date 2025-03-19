using Microsoft.Extensions.Configuration;
using SkyWeatherAPI.Models;
using System.Text.Json;

namespace SkyWeatherAPI.Services
{
    public class WeatherService : IWeatherService
    {
        private static readonly HttpClient client;

        
        string ApiKey = Environment.GetEnvironmentVariable("OpenweathermapApiKey");
 
        static WeatherService()
        {  
            client = new HttpClient()
            {                
                //BaseAddress = new Uri("https://api.openweathermap.org")
                 BaseAddress = new Uri(SkyConfig.Configuration.GetValue<string>("OpenweathermapBaseUrl"))
            };            
        }

        public WeatherService()
        {
        }
      

        public async Task<WeatherModel> GetWeatherData(string city)
        {       
            try
            {

                List<CoordinatesModel> coordinatesModel = await GetCoordinates(city);

                var cityCoordinates = coordinatesModel.FirstOrDefault();

                var url = string.Format("/data/2.5/weather?lat={0}&lon={1}&appid={2}&units=metric", cityCoordinates?.lat, cityCoordinates?.lon,ApiKey);
                
                var result = new WeatherModel();
                var response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var stringResponse = await response.Content.ReadAsStringAsync();

                    result = JsonSerializer.Deserialize<WeatherModel>(stringResponse );
                }
                else
                {
                    throw new HttpRequestException(response.ReasonPhrase);
                }

                return result;
            }
            catch (Exception ex) 
            {
                throw;
            }
            
        }

        public async Task<FiveDayWeatherModel> GetFiveDayWeatherData(string city)
        {
            try
            {
                 
                var url = string.Format("/data/2.5/forecast?q={0}&units={1}&appid={2}", city, "metric", ApiKey);
                var result = new FiveDayWeatherModel();
                var response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var stringResponse = await response.Content.ReadAsStringAsync();
                    result = JsonSerializer.Deserialize<FiveDayWeatherModel>(stringResponse);
                }
                else
                {
                    throw new HttpRequestException(response.ReasonPhrase);
                }

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public async Task<List<CoordinatesModel>> GetCoordinates(string city)
        {
            try
            { 
                var url = string.Format("/geo/1.0/direct?q={0}&appid={1}", city, ApiKey); 
              
                var result = new List<CoordinatesModel>();
                var response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var stringResponse = await response.Content.ReadAsStringAsync();

                    result = JsonSerializer.Deserialize<List<CoordinatesModel>>(stringResponse,
                        new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
                }
                else
                {
                    throw new HttpRequestException(response.ReasonPhrase);
                }

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }

        }


    }



}
