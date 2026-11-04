using RevitAPI_AI_HUB_Plugin.Models;

namespace RevitAPI_AI_HUB_Plugin.Services
{
    public class PluginLaunchService
    {
        public bool CanLaunch(PluginInfoModel plugin)
        {
            return plugin != null && !string.IsNullOrEmpty(plugin.CommandFullClassName);
        }
    }
}
