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
    public class RevitParameterProcessor
    {
        private Document _doc;

        public RevitParameterProcessor(Document doc)
        {
            _doc = doc ?? throw new ArgumentNullException(nameof(doc));
        }

        /// <summary>
        /// Получить значение параметра элемента по имени.
        /// </summary>
        public string GetParameterValue(Element element, string parameterName)
        {
            Parameter param = element.LookupParameter(parameterName);
            if (param == null)
            {
                return $"Параметр '{parameterName}' не найден.";
            }

            return param.AsString() ?? param.AsValueString() ?? "Пустое значение";
        }

        /// <summary>
        /// Установить значение параметра элемента.
        /// </summary>
        public string SetParameterValue(Element element, string parameterName, string value)
        {
            Parameter param = element.LookupParameter(parameterName);
            if (param == null)
            {
                return $"Параметр '{parameterName}' не найден.";
            }

            using (Transaction trans = new Transaction(_doc, "Установка значения параметра"))
            {
                trans.Start();
                if (param.StorageType == StorageType.String)
                {
                    param.Set(value);
                }
                else if (param.StorageType == StorageType.Double && double.TryParse(value, out double doubleValue))
                {
                    param.Set(doubleValue);
                }
                else if (param.StorageType == StorageType.Integer && int.TryParse(value, out int intValue))
                {
                    param.Set(intValue);
                }
                else
                {
                    trans.RollBack();
                    return $"Ошибка: Тип параметра '{parameterName}' не соответствует значению '{value}'.";
                }
                trans.Commit();
            }

            return $"Параметр '{parameterName}' успешно установлен на '{value}'.";
        }
    }
}
