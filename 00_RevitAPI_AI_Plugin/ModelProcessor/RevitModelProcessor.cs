using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RevitAPI_AI_Plugin.Helper;
using RevitAPI_AI_Plugin.Keyword;
using RevitAPI_AI_Plugin.Views;
using RevitAPI_AI_Plugin.WindowMain;
using RevitAPI_AI_Plugin.ModelProcessor;
using Autodesk.Revit.DB;

namespace RevitAPI_AI_Plugin.ModelProcessor
{
    /// <summary>
    /// Фасад для работы с различными процессорами модели Revit.
    /// </summary>
    public class RevitModelProcessor
    {
        private RevitCategoryProcessor _categoryProcessor;
        private RevitIdProcessor _idProcessor;
        private RevitParameterProcessor _parameterProcessor;
        private RevitParameterChecker _parameterChecker;
        private RevitExportProcessor _exportProcessor;
        private RevitSheetCreator _sheetCreator;

        public RevitModelProcessor(Autodesk.Revit.DB.Document doc)
        {
            if (doc == null)
                throw new ArgumentNullException(nameof(doc), "Документ Revit не может быть null.");

            // Инициализация процессоров
            _categoryProcessor = new RevitCategoryProcessor(doc);
            _idProcessor = new RevitIdProcessor(doc);
            _parameterProcessor = new RevitParameterProcessor(doc);
            _parameterChecker = new RevitParameterChecker(doc);
            _exportProcessor = new RevitExportProcessor(doc);
            _sheetCreator = new RevitSheetCreator(doc);
        }

        // Свойства для доступа к каждому процессору
        public RevitCategoryProcessor Categories => _categoryProcessor;
        public RevitIdProcessor ElementsById => _idProcessor;
        public RevitParameterProcessor Parameters => _parameterProcessor;
        public RevitParameterChecker ParameterChecker => _parameterChecker;
        public RevitExportProcessor Export => _exportProcessor;
        public RevitSheetCreator Sheets => _sheetCreator;
    }
}
