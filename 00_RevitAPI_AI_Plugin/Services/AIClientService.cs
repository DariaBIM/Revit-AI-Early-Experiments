using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using RevitAPI_AI_Plugin.Models;

namespace RevitAPI_AI_Plugin.Services
{
    public class AIClientService
    {
        private readonly AISettingsService _settingsService;

        public AIClientService()
            : this(new AISettingsService())
        {
        }

        public AIClientService(AISettingsService settingsService)
        {
            _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
        }

        public async Task<string> SendRawAsync(string userText)
        {
            List<AIMessage> messages = new List<AIMessage>
            {
                new AIMessage
                {
                    Role = "user",
                    Content = userText
                }
            };

            return await SendRawAsync(messages);
        }

        public async Task<string> SendRawAsync(List<AIMessage> messages)
        {
            AISettingsModel settings = _settingsService.GetSettings();

            if (string.IsNullOrWhiteSpace(settings.ApiKey))
            {
                throw new InvalidOperationException("API key is missing in OPENAI_API_KEY.");
            }

            if (messages == null || messages.Count == 0)
            {
                throw new ArgumentException("Messages list is empty.", nameof(messages));
            }

            var requestBody = new
            {
                model = settings.Model,
                messages = messages.Select(m => new { role = m.Role, content = m.Content }).ToList(),
                max_tokens = 100,
                temperature = 0.7
            };

            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + settings.ApiKey);

                StringContent content = new StringContent(
                    JsonSerializer.Serialize(requestBody),
                    Encoding.UTF8,
                    "application/json");

                HttpResponseMessage response;
                string rawResponse;
                try
                {
                    response = await httpClient.PostAsync(settings.BaseUrl, content);
                    rawResponse = await response.Content.ReadAsStringAsync();
                }
                catch (TaskCanceledException)
                {
                    throw new InvalidOperationException("AI недоступен: превышено время ожидания ответа.");
                }

                if (!response.IsSuccessStatusCode)
                {
                    throw new InvalidOperationException(BuildUserFriendlyError(response.StatusCode, rawResponse));
                }

                return rawResponse;
            }
        }

        private static string BuildUserFriendlyError(HttpStatusCode statusCode, string rawResponse)
        {
            string lowerRaw = (rawResponse ?? string.Empty).ToLowerInvariant();

            if (statusCode == HttpStatusCode.Unauthorized)
            {
                return "AI недоступен: неверный или отсутствующий API key.";
            }

            if (statusCode == HttpStatusCode.RequestTimeout)
            {
                return "AI недоступен: превышено время ожидания ответа.";
            }

            if ((int)statusCode == 429 &&
                (lowerRaw.Contains("insufficient_quota") || lowerRaw.Contains("quota") || lowerRaw.Contains("billing")))
            {
                return "AI недоступен: нет баланса API или превышена квота. Проверьте Billing в OpenAI Platform.";
            }

            return "AI недоступен. Код ошибки: " + (int)statusCode + ".";
        }
    }
}
