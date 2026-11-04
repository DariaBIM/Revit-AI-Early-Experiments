using System.Collections.Generic;
using RevitAPI_AI_HUB_Plugin.Models;

namespace RevitAPI_AI_HUB_Plugin.Services
{
    public class AIHubService
    {
        private readonly PluginCatalogService _pluginCatalogService;
        private readonly PluginMatchService _pluginMatchService;

        public AIHubService()
        {
            _pluginCatalogService = new PluginCatalogService();
            _pluginMatchService = new PluginMatchService();
        }

        public HubResponseModel Analyze(HubRequestModel request)
        {
            string input = request != null ? request.UserInput : null;

            List<PluginInfoModel> plugins = _pluginCatalogService.GetPlugins();
            PluginMatchResultModel matchResult = _pluginMatchService.FindBestPlugin(input, plugins);

            if (matchResult != null && matchResult.Plugin != null)
            {
                string matchedList = string.Empty;
                if (matchResult.MatchedKeywords != null)
                {
                    for (int i = 0; i < matchResult.MatchedKeywords.Count; i++)
                    {
                        if (i > 0)
                        {
                            matchedList += ", ";
                        }

                        matchedList += matchResult.MatchedKeywords[i];
                    }
                }

                return new HubResponseModel
                {
                    Message =
                        "Рекомендованный плагин:\n" + matchResult.Plugin.Name + "\n\n" +
                        "Описание:\n" + matchResult.Plugin.Description + "\n\n" +
                        "Почему выбран:\n" + "совпали ключевые слова: " + matchedList + "\n\n" +
                        "Запрос:\n" + (input ?? string.Empty)
                };
            }

            return new HubResponseModel
            {
                Message =
                    "Я пока не нашёл подходящий плагин.\n\n" +
                    "Доступных плагинов в HUB: " + plugins.Count + "\n" +
                    "Запрос: " + (input ?? string.Empty)
            };
        }
    }
}
