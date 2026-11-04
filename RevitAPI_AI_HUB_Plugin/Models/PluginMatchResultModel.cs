using System.Collections.Generic;

namespace RevitAPI_AI_HUB_Plugin.Models
{
    public class PluginMatchResultModel
    {
        public PluginInfoModel Plugin { get; set; }

        public int Score { get; set; }

        public List<string> MatchedKeywords { get; set; }
    }
}
