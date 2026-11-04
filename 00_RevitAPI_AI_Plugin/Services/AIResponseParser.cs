using System;
using System.Text.Json;

namespace RevitAPI_AI_Plugin.Services
{
    public class AIResponseParser
    {
        public string ParseAnswer(string rawJson)
        {
            if (string.IsNullOrWhiteSpace(rawJson))
            {
                throw new ArgumentException("Raw JSON response is empty.", nameof(rawJson));
            }

            using (JsonDocument document = JsonDocument.Parse(rawJson))
            {
                JsonElement root = document.RootElement;
                JsonElement choices = root.GetProperty("choices");
                JsonElement firstChoice = choices[0];
                JsonElement message = firstChoice.GetProperty("message");
                JsonElement content = message.GetProperty("content");

                return content.ToString().Trim();
            }
        }
    }
}
