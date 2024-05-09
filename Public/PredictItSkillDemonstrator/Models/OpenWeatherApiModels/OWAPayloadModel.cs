using System.Text.Json.Serialization;

namespace PredictItSkillDemonstrator.Models.OpenWeatherApiModels
{
    //OWA means Open Weather Model

    /// <summary>
    /// The model for the JSON payload returned by the OpenWeather API
    /// </summary>
    public class OWAPayloadModel
    {
        [JsonPropertyName("coord")]
        public OWACoordinatesModel Coordinates { get; set; }

        [JsonPropertyName("weather")]
        public OWAWeatherModel Weather { get; set; }

    }
}
