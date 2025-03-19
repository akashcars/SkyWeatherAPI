using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using SkyWeatherAPI.Models;
using SkyWeatherAPI.Models.Requests;
using SkyWeatherAPI.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SkyWeatherAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    { 

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IWeatherService _weatherService;

        private readonly IMemoryCache _cache;
        private int cacheTime = 10;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IWeatherService weatherService, IMemoryCache cache)
        {
            _logger = logger;
            _weatherService = weatherService;
            _cache = cache;
        }


        /// <summary>
        /// Get Current Weather By City Name
        /// </summary>
        /// <param name="city"></param>
        /// <returns>Current Weather of city</returns>
        [HttpGet("GetWeatherByCity")]
        public async Task<ActionResult<WeatherModel>> GetWeatherByCity(string city = "")
        {

            if (string.IsNullOrEmpty(city))
            {
                return BadRequest("City is required");
            }
          
            try
            {
                WeatherModel result;
                if (!_cache.TryGetValue(city.ToLower(), out result))
                { 
                    result = await _weatherService.GetWeatherData(city);

                    var cacheEntryOptions = new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(cacheTime)
                    };

                    // Save data in cache
                    _cache.Set(city.ToLower() , result);
                }  

                return Ok(result);

            }
            catch (Exception ex)
            {
                _logger.LogError("Error in getting Getting Weather By City");

                return NotFound();
            } 

        }

        /// <summary>
        /// Get Five Day Weather By City Name
        /// </summary>
        /// <param name="city"></param>
        /// <returns>Five day 3 Hourly weather forecast</returns>
        [HttpGet("GetFiveDayWeatherByCity")]
        public async Task<ActionResult<FiveDayWeatherModel>> GetFiveDayWeatherByCity(string city = "")
        {     

            if (string.IsNullOrEmpty(city))
            {
                return BadRequest("City is required");
            } 

            try
            {
                var result = await _weatherService.GetFiveDayWeatherData(city);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in getting FiveDayWeatherByCity");

                return NotFound();
            }

        }


        /// <summary>
        /// Set Default location for user  
        /// </summary>
        /// <param name="userLocationRequest"></param>
        /// <returns>Location name</returns>
        [HttpPost("SetDefaultLocation")]
        public ActionResult<string> SetDefaultLocation(UserLocationRequest userLocationRequest)
        {
            try
            {
                if (userLocationRequest != null && string.IsNullOrEmpty(userLocationRequest.Location))
                {
                    return BadRequest("City is required.Please enter city name");
                }

                //HttpContext.Session.SetString("DefaultLocation", userLocationRequest.Location);

                return Ok(userLocationRequest.Location);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in setting Location");
                return NotFound();
            }

        } 

    }
}
