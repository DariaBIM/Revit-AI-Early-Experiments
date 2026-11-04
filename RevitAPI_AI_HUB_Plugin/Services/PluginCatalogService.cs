using System.Collections.Generic;
using RevitAPI_AI_HUB_Plugin.Models;

namespace RevitAPI_AI_HUB_Plugin.Services
{
    public class PluginCatalogService
    {
        public List<PluginInfoModel> GetPlugins()
        {
            return new List<PluginInfoModel>
            {
                new PluginInfoModel
                {
                    Name = "RevitAPI_CSVParameterUpdater",
                    CommandFullClassName = "RevitAPI_CSVParameterUpdater.Main",
                    Description = "Updates Revit instance parameters from CSV by matching element identifiers.",
                    Keywords = new List<string>
                    {
                        "csv",
                        "excel",
                        "table",
                        "update parameters",
                        "parameter update",
                        "elementid",
                        "wi_id"
                    }
                },
                new PluginInfoModel
                {
                    Name = "RevitAPI_FamilyExporter",
                    CommandFullClassName = "RevitAPI_FamilyExporter.Main",
                    Description = "Exports family, type and instance parameters to TXT or Excel reports.",
                    Keywords = new List<string>
                    {
                        "export",
                        "family",
                        "parameters",
                        "excel",
                        "txt",
                        "report",
                        "elementid",
                        "экспорт",
                        "семейства"
                    }
                },
                new PluginInfoModel
                {
                    Name = "RevitAPI_ParameterChangeSync",
                    CommandFullClassName = "RevitAPI_ParameterChangeSync.Main",
                    Description = "Synchronizes parameters between two opened Revit models using ElementId or coordinate fallback.",
                    Keywords = new List<string>
                    {
                        "sync",
                        "synchronize",
                        "copy parameters",
                        "two models",
                        "model b",
                        "model a",
                        "coordinates",
                        "nested",
                        "синхронизировать",
                        "двумя моделями"
                    }
                },
                new PluginInfoModel
                {
                    Name = "RevitAPI_ParameterFormulaWriter",
                    CommandFullClassName = "RevitAPI_ParameterFormulaWriter.Main",
                    Description = "Writes Revit parameters using formulas, containers and parameter expressions.",
                    Keywords = new List<string>
                    {
                        "formula",
                        "write parameters",
                        "modplus",
                        "container",
                        "expression",
                        "nested",
                        "fill parameters",
                        "формул",
                        "заполнить"
                    }
                },
                new PluginInfoModel
                {
                    Name = "Parameter Standard Audit",
                    CommandFullClassName = null,
                    Description = "Checks whether required parameters exist and detects extra or incorrect parameters.",
                    Keywords = new List<string>
                    {
                        "audit",
                        "standard",
                        "check parameters",
                        "missing parameters",
                        "extra parameters",
                        "fop",
                        "shared parameters"
                    }
                }
            };
        }
    }
}
