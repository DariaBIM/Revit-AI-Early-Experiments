using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using RevitAPI_AI_Plugin.Helper;
using RevitAPI_AI_Plugin.Keyword;
using RevitAPI_AI_Plugin.Views;
using RevitAPI_AI_Plugin.WindowMain;
using RevitAPI_AI_Plugin.ModelProcessor;

namespace RevitAPI_AI_Plugin.ModelProcessor
{
    public class RevitSheetCreator
    {
        private Document _doc;

        public RevitSheetCreator(Document doc)
        {
            _doc = doc ?? throw new ArgumentNullException(nameof(doc));
        }

        /// <summary>
        /// Создаёт новый лист на основе заданного шаблона.
        /// </summary>
        public string CreateSheet(ViewSheet templateSheet, string sheetName, string sheetNumber)
        {
            using (Transaction trans = new Transaction(_doc, "Создание нового листа"))
            {
                trans.Start();

                ViewSheet newSheet = ViewSheet.Create(_doc, templateSheet.Id);
                newSheet.Name = sheetName;
                newSheet.SheetNumber = sheetNumber;

                trans.Commit();
            }

            return $"Лист '{sheetName}' (№{sheetNumber}) успешно создан.";
        }
    }
}
