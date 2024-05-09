using PredictItSkillDemonstrator.Configurations;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System;
using PredictItSkillDemonstrator.Models.OpenWeatherApiModels;
using Microsoft.Extensions.Logging;

namespace PredictItSkillDemonstrator.BusinessLayer
{
    public class WeatherHelper
    {
        private readonly ApiKeyConfiguration _apiKeyConfiguration;
        private readonly string OpenWeatherBaseURL = "http://api.openweathermap.org/data/2.5/weather";
        private readonly HttpClient _httpClient;
        private readonly ILogger<WeatherHelper> _logger;

        public WeatherHelper(ApiKeyConfiguration apiKeyConfiguration, IHttpClientFactory httpClientFactory,
            ILogger<WeatherHelper> logger)
        {
            _httpClient = httpClientFactory.CreateClient("OpenWeatherAPI");
            _apiKeyConfiguration = apiKeyConfiguration;
            _logger = logger;

        }

        //QUESTION #2 - Fill in this function
        /// <summary>
        /// Return the forecasts from forecastsForNextMonth ordered by date
        /// </summary>
        /// <param name="forecastsForNextMonth">A list of forecasts for the month</param>
        /// <param name="coldTempCutOffInFarenheit">The temperature below or equal to which is considered cold (in farenheit)</param>
        /// <returns></returns>
        public WeatherForecast[] GetColdForecasts(List<WeatherForecast> forecastsForNextMonth, int coldTempCutOffInFarenheit)
        {
            List<WeatherForecast> coldForecasts = new List<WeatherForecast>();

            coldForecasts = forecastsForNextMonth.Where(f => f.TemperatureF <= coldTempCutOffInFarenheit).OrderBy(f => f.Date).ToList();

            return coldForecasts.ToArray();

        }
        //END QUESTION #2

        //QUESTION #3 - Create another function with the same name below which gets the cold forecasts but defines cold as below 50 degrees

        /// <summary>
        /// gets the cold forecasts but defines cold as below 50 degrees
        /// </summary>
        /// <param name="forecastsForNextMonth"></param>
        /// <returns></returns>
        public WeatherForecast[] GetColdForecasts(List<WeatherForecast> forecastsForNextMonth)
        {
            return GetColdForecasts(forecastsForNextMonth, 50);
        }

        //END QUESTION #3

        //QUESTION #4 - Create a function which calls the OpenWeather API https://home.openweathermap.org/ and returns the weather description for our Provo office at 86N University Avenue

        /// <summary>
        /// Calls the OpenWeather API and returns the weather description for lat 40.234790 long -111.658170
        /// </summary>
        /// <param name="lat"></param>
        /// <param name="lon"></param>
        /// <returns></returns>
        public async Task<string> GetCurrentWeatherDescriptionWithCoordinates(double lon, double lat)
        {
            double _lon = lon;
            double _lat = lat;
            string OpenWeatherAPIKey = _apiKeyConfiguration.OpenWeatherAPIKey;
            string weatherDescription = string.Empty;

            string url = new UriBuilder(OpenWeatherBaseURL)
            {
                Query = $"lat={_lat}&lon={_lon}&appid={OpenWeatherAPIKey}"
            }.ToString();

            try
            {
                //_httpClient has been configured with a base url and a retry policy that has exponential backoff implemented
                using (HttpClient client = _httpClient)
                {
                    HttpResponseMessage response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        string json = await response.Content.ReadAsStringAsync();
                        OWAPayloadModel weatherPayload = JsonSerializer.Deserialize<OWAPayloadModel>(json);
                        weatherDescription = weatherPayload.Weather.Description;

                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        _logger.LogError("Weather not found");
                    }
                    else if( response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        _logger.LogError("Unauthorized access to weather API");
                    }
                    else
                    {
                        _logger.LogError("Error getting weather data");
                    }
                    
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error getting weather data");
            }
            
            return weatherDescription;
        }

        //END QUESTION #4
    }
}
