using SkyWeatherAPI.Models;

namespace SkyWeatherAPI.Services.Tests
{
    [TestClass]
    public class WeatherServiceTests
    {

        [TestMethod]
        public async Task GetWeatherAsync_ReturnsValidData()
        {
           
            //HttpClient client = new HttpClient();
            //client.BaseAddress = new Uri(SkyConfig.Configuration.GetValue<string>("OpenweathermapBaseUrl"));
            //string apikey = SkyConfig.Configuration.GetValue<string>("OpenweathermapApiKey");

            var service = new WeatherService();

            // Act 
            var result = await service.GetFiveDayWeatherData("London");

            // Assert
            Assert.AreEqual("London", result.city.name ); //---  validating the city name
            Assert.IsInstanceOfType(result, typeof(FiveDayWeatherModel)); //--- validating the type of the result
            //Assert.IsInstanceOfType(result, typeof(WeatherModel));

        } 
    }
}
 