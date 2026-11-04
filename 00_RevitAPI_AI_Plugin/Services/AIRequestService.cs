using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RevitAPI_AI_Plugin.Models;

namespace RevitAPI_AI_Plugin.Services
{
    public class AIRequestService
    {
        private readonly AIClientService _clientService;
        private readonly AIResponseParser _responseParser;
        private readonly AISettingsService _settingsService;

        public AIRequestService()
            : this(new AIClientService(), new AIResponseParser(), new AISettingsService())
        {
        }

        public AIRequestService(AIClientService clientService, AIResponseParser responseParser, AISettingsService settingsService)
        {
            _clientService = clientService ?? throw new ArgumentNullException(nameof(clientService));
            _responseParser = responseParser ?? throw new ArgumentNullException(nameof(responseParser));
            _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
        }

        public async Task<AIResponseModel> SendAsync(string userText)
        {
            List<AIMessage> messages = new List<AIMessage>
            {
                new AIMessage
                {
                    Role = "user",
                    Content = userText
                }
            };

            return await SendAsync(messages);
        }

        public async Task<AIResponseModel> SendAsync(List<AIMessage> messages)
        {
            try
            {
                AISettingsModel settings = _settingsService.GetSettings();
                if (string.IsNullOrWhiteSpace(settings.ApiKey))
                {
                    return new AIResponseModel
                    {
                        IsSuccess = false,
                        AnswerText = null,
                        ErrorMessage = "OPENAI_API_KEY is not set."
                    };
                }

                string rawResponse = await _clientService.SendRawAsync(messages);
                string answerText = _responseParser.ParseAnswer(rawResponse);

                return new AIResponseModel
                {
                    IsSuccess = true,
                    AnswerText = answerText,
                    ErrorMessage = null
                };
            }
            catch (Exception ex)
            {
                return new AIResponseModel
                {
                    IsSuccess = false,
                    AnswerText = null,
                    ErrorMessage = ex.Message
                };
            }
        }
    }
}
