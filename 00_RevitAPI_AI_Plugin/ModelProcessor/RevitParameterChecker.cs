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
    public class RevitParameterChecker
    {
        private Document _doc;

        public RevitParameterChecker(Document doc)
        {
            _doc = doc ?? throw new ArgumentNullException(nameof(doc));
        }

        /// <summary>
        /// Проверить заполнение заданного параметра для всех элементов категории.
        /// </summary>
        public List<string> CheckParameterForCategory(BuiltInCategory category, string parameterName)
        {
            List<string> issues = new List<string>();

            var collector = new FilteredElementCollector(_doc)
                .OfCategory(category)
                .WhereElementIsNotElementType();

            foreach (var element in collector)
            {
                Parameter param = element.LookupParameter(parameterName);
                if (param == null || string.IsNullOrWhiteSpace(param.AsString() ?? param.AsValueString()))
                {
                    issues.Add($"Элемент ID: {element.Id.IntegerValue} - параметр '{parameterName}' не заполнен.");
                }
            }

            return issues;
        }
    }
}
