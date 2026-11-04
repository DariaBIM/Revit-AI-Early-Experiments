using System.Collections.Generic;
using RevitAPI_AI_HUB_Plugin.Models;

namespace RevitAPI_AI_HUB_Plugin.Services
{
    public class PluginMatchService
    {
        public PluginMatchResultModel FindBestPlugin(string userInput, List<PluginInfoModel> plugins)
        {
            if (string.IsNullOrWhiteSpace(userInput) || plugins == null || plugins.Count == 0)
            {
                return null;
            }

            string userInputLower = userInput.ToLowerInvariant();
            PluginInfoModel bestPlugin = null;
            List<string> bestMatchedKeywords = null;
            int maxScore = 0;

            foreach (PluginInfoModel plugin in plugins)
            {
                List<string> matchedKeywords = new List<string>();
                if (plugin != null && plugin.Keywords != null)
                {
                    foreach (string keyword in plugin.Keywords)
                    {
                        if (string.IsNullOrEmpty(keyword))
                        {
                            continue;
                        }

                        if (userInputLower.Contains(keyword.ToLowerInvariant()))
                        {
                            matchedKeywords.Add(keyword);
                        }
                    }
                }

                int score = matchedKeywords.Count;
                if (score > maxScore)
                {
                    maxScore = score;
                    bestPlugin = plugin;
                    bestMatchedKeywords = matchedKeywords;
                }
            }

            if (maxScore == 0)
            {
                return null;
            }

            PluginMatchResultModel result = new PluginMatchResultModel();
            result.Plugin = bestPlugin;
            result.Score = maxScore;
            result.MatchedKeywords = bestMatchedKeywords;
            return result;
        }
    }
}
