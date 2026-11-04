using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using RevitAPI_AI_Plugin.Helper;
using RevitAPI_AI_Plugin.Keyword;
using RevitAPI_AI_Plugin.Views;
using RevitAPI_AI_Plugin.WindowMain;

namespace RevitAPI_AI_Plugin.Helper
{
    public class OpenAIHelper
    {
        private readonly string _apiUrl = "https://api.openai.com/v1/chat/completions";
        private readonly string _apiKey;

        public OpenAIHelper()
        {
            _apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");

            if (string.IsNullOrEmpty(_apiKey))
            {
                throw new Exception("API ключ не найден. Убедитесь, что он добавлен в переменные окружения.");
            }
        }

        public async Task<string> GetChatGPTResponse(string prompt)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");

                var requestBody = new
                {
                    model = "gpt-3.5-turbo",
                    messages = new[] { new { role = "user", content = prompt } },
                    max_tokens = 100,
                    temperature = 0.7
                };

                var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync(_apiUrl, content);

                if (!response.IsSuccessStatusCode)
                {
                    string errorDetails = await response.Content.ReadAsStringAsync();
                    return $"Ошибка: {response.StatusCode}\nДетали: {errorDetails}";
                }

                var responseString = await response.Content.ReadAsStringAsync();
                var jsonResponse = JsonDocument.Parse(responseString);

                return jsonResponse.RootElement
                    .GetProperty("choices")[0]
                    .GetProperty("message")
                    .GetProperty("content")
                    .ToString()
                    .Trim();
            }
        }
    }
}
