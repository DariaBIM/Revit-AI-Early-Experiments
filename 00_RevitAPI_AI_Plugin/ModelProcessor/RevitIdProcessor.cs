using Autodesk.Revit.DB;
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

namespace RevitAPI_AI_Plugin.ModelProcessor
{
    public class RevitIdProcessor
    {
        private readonly Document _doc;

        public RevitIdProcessor(Document doc)
        {
            _doc = doc ?? throw new ArgumentNullException(nameof(doc));
        }

        /// <summary>
        /// Получает элемент по его ID.
        /// </summary>
        public Element GetElementById(int id)
        {
            return _doc.GetElement(new ElementId(id));
        }

        /// <summary>
        /// Получает имя и тип элемента по его ID.
        /// </summary>
        public string GetElementInfoById(int id)
        {
            Element element = GetElementById(id);
            if (element == null)
            {
                return $"Элемент с ID {id} не найден.";
            }

            return $"Элемент: {element.Name}, Тип: {element.GetType().Name}";
        }
    }
}
