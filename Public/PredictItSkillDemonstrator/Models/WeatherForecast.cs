using System;

namespace PredictItSkillDemonstrator
{
    public class WeatherForecast
    {
        public WeatherForecast()
        {
        }

        //adding constructor here so when we create a new WeatherForecast from the OpenWeatherApiModel,
        //we can pass in the values
        public WeatherForecast(DateTime date, int temperatureC, string summary)
        {
            Date = date;
            TemperatureC = temperatureC;
            Summary = summary;
        }

        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string Summary { get; set; }
    }
}
