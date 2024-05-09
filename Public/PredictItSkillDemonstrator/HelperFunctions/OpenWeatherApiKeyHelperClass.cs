using Microsoft.Extensions.Logging;
using System;


namespace PredictItSkillDemonstrator.HelperFunctions
{
    /// <summary>
    /// This helper class gets the apikey from the configuration file. It is cleaner than having to inject the Configuration file into every service that needs it.
    /// injecting the configuration file leaves some ambiguity but having the helper class for apis makes it clear that an apikey will be needed.
    /// </summary>
    public class OpenWeatherApiKeyHelperClass
    {
        private readonly Configurations.ApiKeyConfiguration _apiKeyConfiguration;

        public OpenWeatherApiKeyHelperClass(Configurations.ApiKeyConfiguration apiKeyConfiguration)
        {
            _apiKeyConfiguration = apiKeyConfiguration;
        }
        public string GetApiKey()
        {
            string apiKey = _apiKeyConfiguration.OpenWeatherAPIKey;
            
            return apiKey;
        }
    }
}

