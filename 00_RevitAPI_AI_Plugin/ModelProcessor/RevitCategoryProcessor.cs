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
    public class RevitCategoryProcessor
    {
        private readonly Autodesk.Revit.DB.Document _doc;

        public RevitCategoryProcessor(Autodesk.Revit.DB.Document doc)
        {
            _doc = doc ?? throw new ArgumentNullException(nameof(doc));
        }

        /// <summary>
        /// Получает список всех категорий в документе.
        /// </summary>
        public List<string> GetAllCategories()
        {
            var categories = _doc.Settings.Categories;
            List<string> categoryNames = new List<string>();

            foreach (Category category in categories)
            {
                if (category != null && category.AllowsBoundParameters)
                {
                    categoryNames.Add(category.Name);
                }
            }

            return categoryNames;
        }
    }
}
