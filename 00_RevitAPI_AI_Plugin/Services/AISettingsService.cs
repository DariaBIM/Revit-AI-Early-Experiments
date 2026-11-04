using System;
using RevitAPI_AI_Plugin.Models;

namespace RevitAPI_AI_Plugin.Services
{
    public class AISettingsService
    {
        public AISettingsModel GetSettings()
        {
            return new AISettingsModel
            {
                ApiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY"),
                BaseUrl = "https://api.openai.com/v1/chat/completions",
                Model = "gpt-3.5-turbo"
            };
        }
    }
}
