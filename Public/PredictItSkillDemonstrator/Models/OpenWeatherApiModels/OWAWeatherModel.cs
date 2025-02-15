﻿using System.Text.Json;
using System.Text.Json.Serialization;

namespace PredictItSkillDemonstrator.Models.OpenWeatherApiModels
{
    public class OWAWeatherModel
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("main")]
        public string Main { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("icon")]
        public string Icon { get; set; }
    }
}
