using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

using RevitAPI_AI_Plugin.Helper;
using RevitAPI_AI_Plugin.Keyword;
using RevitAPI_AI_Plugin.Views;
using RevitAPI_AI_Plugin.WindowMain;
using RevitAPI_AI_Plugin.ModelProcessor;

namespace RevitAPI_AI_Plugin.ModelProcessor
{
    public class RevitExportProcessor
    {
        private Document _doc;

        public RevitExportProcessor(Document doc)
        {
            _doc = doc ?? throw new ArgumentNullException(nameof(doc));
        }

        /// <summary>
        /// Экспорт модели в формат DWG.
        /// </summary>
        public string ExportToDWG(string outputPath)
        {
            // Получаем все представления для экспорта
            var viewsToExport = new FilteredElementCollector(_doc)
                .OfClass(typeof(View))
                .Cast<View>()
                .Where(view => view.ViewType == ViewType.FloorPlan || view.ViewType == ViewType.ThreeD) // Пример фильтрации
                .Select(view => view.Id)
                .ToList();

            if (viewsToExport.Count == 0)
            {
                return "Нет доступных представлений для экспорта в DWG.";
            }

            DWGExportOptions options = new DWGExportOptions();

            try
            {
                using (Transaction trans = new Transaction(_doc, "Экспорт в DWG"))
                {
                    trans.Start();
                    _doc.Export(outputPath, "Model", viewsToExport, options);
                    trans.Commit();
                }

                return "Экспорт в DWG выполнен успешно.";
            }
            catch (Exception ex)
            {
                return $"Ошибка при экспорте в DWG: {ex.Message}";
            }
        }

        /// <summary>
        /// Экспорт модели в формат NWC.
        /// </summary>
        public string ExportToNWC(string outputPath)
        {
            NavisworksExportOptions options = new NavisworksExportOptions();

            try
            {
                using (Transaction trans = new Transaction(_doc, "Экспорт в NWC"))
                {
                    trans.Start();
                    _doc.Export(outputPath, "Model.nwc", options);
                    trans.Commit();
                }

                return "Экспорт в NWC выполнен успешно.";
            }
            catch (Exception ex)
            {
                return $"Ошибка при экспорте в NWC: {ex.Message}";
            }
        }

        /// <summary>
        /// Экспорт листов в формат PDF.
        /// </summary>
        public string ExportToPDF(string outputPath)
        {
            // Получаем все доступные листы
            var sheetsToExport = new FilteredElementCollector(_doc)
                .OfClass(typeof(ViewSheet))
                .Cast<ViewSheet>()
                .Where(sheet => !sheet.IsTemplate) // Исключаем шаблоны
                .Select(sheet => sheet.Id)
                .ToList();

            // Проверяем, есть ли листы для экспорта
            if (sheetsToExport.Count == 0)
            {
                return "Нет доступных листов для экспорта в PDF.";
            }

            using (Transaction trans = new Transaction(_doc, "Экспорт в PDF"))
            {
                trans.Start();

                PDFExportOptions options = new PDFExportOptions();
                bool result = _doc.Export(outputPath, sheetsToExport, options);

                trans.Commit();

                return result ? "Экспорт в PDF выполнен успешно." : "Ошибка при экспорте в PDF.";
            }
        }
    }
}
