using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PredictItSkillDemonstrator.BusinessLayer
{
    public class WeatherHelper
    {
        //QUESTION #2 - Fill in this function
        /// <summary>
        /// Return the forecasts from forecastsForNextMonth ordered by date
        /// </summary>
        /// <param name="forecastsForNextMonth">A list of forecasts for the month</param>
        /// <param name="coldTempCutOffInFarenheit">The temperature below or equal to which is considered cold (in farenheit)</param>
        /// <returns></returns>
        public WeatherForecast[] GetColdForecasts(List<WeatherForecast> forecastsForNextMonth, int coldTempCutOffInFarenheit)
        {
            
        }
        //END QUESTION #2

        //QUESTION #3 - Create another function with the same name below which gets the cold forecasts but defines cold as below 50 degrees

        //END QUESTION #3

        //QUESTION #4 - Create a function which calls the OpenWeather API https://home.openweathermap.org/ and returns the weather description for our Provo office at 86N University Avenue
        public string OpenWeatherAPIKey = "31996e11ed59ae841d9145334a82f328";


        //END QUESTION #4
    }
}
