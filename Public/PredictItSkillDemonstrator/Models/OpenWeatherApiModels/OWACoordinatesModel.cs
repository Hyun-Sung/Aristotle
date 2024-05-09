using System.Reflection.Metadata.Ecma335;
using System.Text.Json.Serialization;

namespace PredictItSkillDemonstrator.Models.OpenWeatherApiModels
{
    /// <summary>
    /// open weather api model for the coordinates field
    /// </summary>
    public class OWACoordinatesModel
    {
        [JsonPropertyName("lon")]
        public double Longitude { get; set; }

        [JsonPropertyName("lat")]
        public double Latitude { get; set; }

        [JsonPropertyName("weather")]
        public OWAWeatherModel[] Weather { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        public OWACoordinatesModel() { }
        public OWACoordinatesModel(double longitude, double latitude)
        {
            Longitude = longitude;
            Latitude = latitude;
        }

        private string ReturnLonLat()
        {
            return $"Longitude: {Longitude}, Latitude: {Latitude}";
        }
    }
}
