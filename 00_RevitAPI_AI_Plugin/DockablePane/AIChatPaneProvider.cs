using Autodesk.Revit.UI;

namespace RevitAPI_AI_Plugin.DockablePane
{
    public class AIChatPaneProvider : IDockablePaneProvider
    {
        private readonly AIChatPane _chatPane;

        public static AIChatPane PaneInstance { get; private set; }

        public AIChatPaneProvider()
        {
            _chatPane = new AIChatPane();
        }

        public AIChatPaneProvider(AIChatPane chatPane)
        {
            _chatPane = chatPane ?? new AIChatPane();
        }

        public void SetupDockablePane(DockablePaneProviderData data)
        {
            PaneInstance = _chatPane;
            data.FrameworkElement = _chatPane;
            data.InitialState = new DockablePaneState();
        }
    }
}
