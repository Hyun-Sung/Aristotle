using PredictItSkillDemonstrator.Configurations;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System;
using PredictItSkillDemonstrator.Models.OpenWeatherApiModels;
using Microsoft.Extensions.Logging;
using PredictItSkillDemonstrator.HelperFunctions;
using System.Web;
using System.Collections.Specialized;
using Microsoft.AspNetCore.WebUtilities;

namespace PredictItSkillDemonstrator.BusinessLayer
{
    public class WeatherHelper
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<WeatherHelper> _logger;
        private readonly OpenWeatherApiKeyHelperClass _openWeatherApiKeyHelperClass;
        private string _apiKey;

        public WeatherHelper( IHttpClientFactory httpClientFactory, ILogger<WeatherHelper> logger, OpenWeatherApiKeyHelperClass openWeatherApiKeyHelperClass)
        {
            _httpClient = httpClientFactory.CreateClient("OpenWeatherAPI");
            _logger = logger;
            _openWeatherApiKeyHelperClass = openWeatherApiKeyHelperClass;
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
        public async Task<string> GetCurrentWeatherDescriptionWithCoordinates(CoordinatesModel coordinates)
        {

            string apiKey = _openWeatherApiKeyHelperClass.GetApiKey();
            string weatherDescription = string.Empty;

            //base url already in startup file. Just need to add the query parameters

            //https://stackoverflow.com/questions/61842738/uri-builder-for-path-only
            //Sahar Shokrani's answer is the one I used to build the query parameters

            Dictionary<string, string> queryParameters = new Dictionary<string, string>
            {
                {"lat", coordinates.Latitude.ToString()},
                {"lon", coordinates.Longitude.ToString()},
                {"appid", apiKey }
            };
            
            string url = QueryHelpers.AddQueryString("", queryParameters);

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
                        weatherDescription = weatherPayload.Weather.FirstOrDefault().Description;

                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        _logger.LogError("Weather not found");
                        throw new HttpRequestException("Weather not found");
                    }
                    else if( response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        _logger.LogError("Unauthorized access to weather API");
                        throw new HttpRequestException("Unauthorized access to weather API");
                    }
                    else
                    {
                        _logger.LogError("Error getting weather data. Error message: " + response.RequestMessage);
                        throw new HttpRequestException("Error getting weather data");
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
