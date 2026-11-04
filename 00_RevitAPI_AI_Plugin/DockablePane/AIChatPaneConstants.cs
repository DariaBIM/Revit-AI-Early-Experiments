using System;
using Autodesk.Revit.UI;

namespace RevitAPI_AI_Plugin.DockablePane
{
    public static class AIChatPaneConstants
    {
        public static readonly Guid PaneGuid = new Guid("EE362AF9-A11C-4D9D-B69B-826ABAB6F3A0");

        public static readonly DockablePaneId PaneId = new DockablePaneId(PaneGuid);

        public const string PaneTitle = "Zimovets AI Chat";
    }
}
