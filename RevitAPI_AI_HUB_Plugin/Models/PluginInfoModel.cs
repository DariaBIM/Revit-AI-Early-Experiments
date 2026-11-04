using System.Collections.Generic;

namespace RevitAPI_AI_HUB_Plugin.Models
{
    public class PluginInfoModel
    {
        public string Name { get; set; }

        public string CommandFullClassName { get; set; }

        public string Description { get; set; }

        public List<string> Keywords { get; set; }
    }
}
