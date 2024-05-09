using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web.Resource;
using PredictItSkillDemonstrator.BusinessLayer;
using PredictItSkillDemonstrator.HelperFunctions;
using PredictItSkillDemonstrator.Models.OpenWeatherApiModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PredictItSkillDemonstrator.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly WeatherHelper _weatherHelper;

        // The Web API will only accept tokens 1) for users, and 2) having the "access_as_user" scope for this API
        static readonly string[] scopeRequiredByApi = new string[] { "access_as_user" };

        public WeatherForecastController(ILogger<WeatherForecastController> logger, WeatherHelper weatherHelper)
        {
            _logger = logger;
            _weatherHelper = weatherHelper;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            //HttpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);

            _logger.Log(LogLevel.Information, "WeatherForecastController GET called...");

            //QUESTION #1 - On the line below add a comment which describes in detail what is happening in the code below

            //  This code is making a list of 5 weather forecasts. 
            // It generates the date based on the current date then adds the index value to it
            // It generates a random number between -20 and 55 and uses that to set the temperature. 
            // The summary is done is a similar way. The summaries of weather are stored in an array of strings.
            // it takes the length of the array and then generates a random number between 0 and the length of the array.
            // whatever value is returned, it finds a random value using the index and the Summaries array. 
            // The value of the summary is set to the Summary property.
            // It returns the weather forecasts in an array of WeatherForecast objects.
            // generation of a list could have been done with a for loop, but it is done with a LINQ query instead which doesn't require expensive looping

            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();

            //I created a Helper function FixTemperatureFieldsHelper.CorrectDescription to fix the temperature fields in the WeatherForecast objects.

            //END QUESTION #1
        }
        // create a new endpoint that uses the Helper class to get ColdForecasts

        /// <summary>
        /// from a list of forecasts for 5 days , get the cold forecasts where the temperature is below 50 degrees
        /// </summary>
        /// <returns></returns>
        /// [Authorize]
        [HttpGet("coldforecasts")]
        public WeatherForecast[] GetColdForecasts()
        {
            _logger.Log(LogLevel.Information, "WeatherForecastController GetColdForecasts called...");
            //HttpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);

            var forecasts = Get();

            WeatherForecast[] coldTemperatures = _weatherHelper.GetColdForecasts(forecasts.ToList(), 50);
            FixTemperatureFieldsHelper.CorrectDescription(coldTemperatures);
            
            return coldTemperatures;

        }

        /// <summary>
        /// Get the current weather of provo
        /// </summary>
        /// <returns></returns>
        
        [HttpGet("provoweather")]
        public async Task<string> GetProvoWeather()
        {
            //HttpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);

            double provoLon = 40.234790;
            double provoLat = -111.658170;

            CoordinatesModel coordinates = new CoordinatesModel(provoLon, provoLat);

            return await GetCurrentWeather(coordinates);

        }

        private async Task<string> GetCurrentWeather(CoordinatesModel coor)
        {
            CoordinatesModel _coordinates = coor;

            _logger.Log(LogLevel.Information, "WeatherForecastController GetCurrentWeather called for " + coor.ReturnCoordinatePairAsString());

            return await _weatherHelper.GetCurrentWeatherDescriptionWithCoordinates(coor);
        }
    }
}
