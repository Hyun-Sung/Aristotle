using System.Text.Json.Serialization;

namespace PredictItSkillDemonstrator.Models
{
    public class OpenWeatherApiModel
    {
        [JsonPropertyName("coord")]
        public double Latitude { get; set; }


    }
}
