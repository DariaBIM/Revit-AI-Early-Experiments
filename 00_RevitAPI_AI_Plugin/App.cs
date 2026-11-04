using Autodesk.Revit.UI;
using RevitAPI_AI_Plugin.DockablePane;

namespace RevitAPI_AI_Plugin
{
    public class App : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication application)
        {
            AIChatPaneProvider provider = new AIChatPaneProvider();
            application.RegisterDockablePane(
                AIChatPaneConstants.PaneId,
                AIChatPaneConstants.PaneTitle,
                provider);

            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }
    }
}
