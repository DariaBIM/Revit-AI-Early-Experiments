using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Json;
using RevitAPI_AI_Plugin.Helper;
using RevitAPI_AI_Plugin.Keyword;
using RevitAPI_AI_Plugin.Views;
using RevitAPI_AI_Plugin.WindowMain;
using RevitAPI_AI_Plugin.ModelProcessor;



namespace RevitAPI_AI_Plugin
{
    [Transaction(TransactionMode.Manual)]
    public class Main : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                Autodesk.Revit.DB.Document doc = commandData.Application.ActiveUIDocument?.Document;

                if (doc == null)
                {
                    TaskDialog.Show("Ошибка", "Документ не найден или закрыт.");
                    return Result.Failed;
                }

                string apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
                bool useOpenAI = !string.IsNullOrWhiteSpace(apiKey);

                if (RevitAPI_AI_Plugin.DockablePane.AIChatPaneProvider.PaneInstance != null)
                {
                    RevitAPI_AI_Plugin.DockablePane.AIChatPaneProvider.PaneInstance.Initialize(doc, useOpenAI);
                }

                Autodesk.Revit.UI.DockablePane pane = commandData.Application.GetDockablePane(RevitAPI_AI_Plugin.DockablePane.AIChatPaneConstants.PaneId);
                pane.Show();

                return Result.Succeeded;
            }
            catch (Autodesk.Revit.Exceptions.ArgumentException)
            {
                TaskDialog.Show("Ошибка", "Dockable Pane не зарегистрирована. Убедитесь, что Application-часть плагина загружена.");
                return Result.Failed;
            }
            catch (Exception ex)
            {
                // Обработка ошибок
                TaskDialog.Show("Ошибка", $"Произошла ошибка:\n{ex.Message}");
                return Result.Failed;
            }
        }
    }
}