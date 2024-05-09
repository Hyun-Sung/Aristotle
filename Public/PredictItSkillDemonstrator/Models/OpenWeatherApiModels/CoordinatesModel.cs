using System.Text.Json.Serialization;

namespace PredictItSkillDemonstrator.Models.OpenWeatherApiModels
{
    public class CoordinatesModel
    {
        public double Longitude { get; set; }

        public double Latitude { get; set; }

        public CoordinatesModel() { }
        public CoordinatesModel(double latitude, double longitude)
        {
            Longitude = longitude;
            Latitude = latitude;
        }

        public string ReturnCoordinatePairAsString()
        {
            return $"Longitude: {Longitude}, Latitude: {Latitude}";
        }
    }
}
