namespace PredictItSkillDemonstrator.HelperFunctions
{
    /// <summary>
    /// this helper function updates the description of the weather forecast based on the temperature found in the Summaries array int he WeatherForecastController
    /// </summary>
    public static class FixTemperatureFieldsHelper
    {
        public static void CorrectDescription(WeatherForecast[] weatherForecasts)
        {
            foreach (WeatherForecast forecast in weatherForecasts)
            {
                if (forecast.TemperatureC < -10)
                {
                    forecast.Summary = "Freezing";
                }
                else if (forecast.TemperatureC < 0)
                {
                    forecast.Summary = "Bracing";
                }
                else if (forecast.TemperatureC < 10)
                {
                    forecast.Summary = "Chilly";
                }
                else if (forecast.TemperatureC < 20)
                {
                    forecast.Summary = "Cool";
                }
                else if (forecast.TemperatureC < 30)
                {
                    forecast.Summary = "Mild";
                }
                else if (forecast.TemperatureC < 40)
                {
                    forecast.Summary = "Warm";
                }
                else if (forecast.TemperatureC < 50)
                {
                    forecast.Summary = "Balmy";
                }
                else if (forecast.TemperatureC < 60)
                {
                    forecast.Summary = "Hot";
                }
                else if (forecast.TemperatureC < 70)
                {
                    forecast.Summary = "Sweltering";
                }
                else
                {
                    forecast.Summary = "Scorching";
                }
            }
        }


    }
}
