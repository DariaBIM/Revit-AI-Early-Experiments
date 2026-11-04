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
using RevitAPI_AI_Plugin.Models;
using RevitAPI_AI_Plugin.Services;
using Autodesk.Revit.DB;
using System.IO; // Конфликтов не вызывает



namespace RevitAPI_AI_Plugin.WindowMain
{
    /// <summary>
    /// Основной класс для обработки пользовательских запросов.
    /// </summary>
    public class ChatWindowMain
    {
        private readonly RevitModelProcessor _modelProcessor;
        private readonly AIRequestService _aiRequestService;
        private readonly bool _useOpenAI;

        /// <summary>
        /// Конструктор, принимающий документ Revit и режим работы.
        /// </summary>
        public ChatWindowMain(Autodesk.Revit.DB.Document doc, bool useOpenAI)
        {
            if (doc == null)
                throw new ArgumentNullException(nameof(doc), "Документ Revit не может быть null.");

            _modelProcessor = new RevitModelProcessor(doc); // Инициализация процессора
            _useOpenAI = useOpenAI;

            if (_useOpenAI)
            {
                _aiRequestService = new AIRequestService();
            }
        }

        /// <summary>
        /// Асинхронная обработка пользовательского ввода.
        /// </summary>
        public async Task<string> ProcessUserInput(string userInput)
        {
            var keywordProcessor = new KeywordProcessor();
            var command = keywordProcessor.ProcessKeyword(userInput);

            if (command != null)
            {
                return await HandleLocalCommand(command, userInput);
            }

            if (_useOpenAI)
            {
                AIResponseModel response = await _aiRequestService.SendAsync(userInput);
                if (response.IsSuccess)
                {
                    return response.AnswerText;
                }

                return response.ErrorMessage;
            }

            return "Команда не распознана. Попробуйте снова.";
        }

        private async Task<string> HandleLocalCommand(string command, string userInput)
        {
            switch (command)
            {
                case "ListCategories":
                    return await Task.Run(() =>
                    {
                        var categories = _modelProcessor.Categories.GetAllCategories();
                        return $"Категории в модели:\n{string.Join(", ", categories)}";
                    });

                case "ExportDWG":
                    string dwgPath = "C:\\Exports";
                    return await Task.Run(() => _modelProcessor.Export.ExportToDWG(dwgPath));

                case "ExportNWC":
                    string nwcPath = "C:\\Exports";
                    return await Task.Run(() => _modelProcessor.Export.ExportToNWC(nwcPath));

                case "CheckParameters":
                    BuiltInCategory category = BuiltInCategory.OST_Walls;
                    string parameterName = "Комментарии";

                    return await Task.Run(() =>
                    {
                        var issues = _modelProcessor.ParameterChecker.CheckParameterForCategory(category, parameterName);
                        return issues.Count > 0
                            ? $"Обнаружены проблемы с параметром '{parameterName}':\n{string.Join("\n", issues)}"
                            : $"Все элементы категории '{category}' имеют заполненный параметр '{parameterName}'.";
                    });

                case "ExportPDF":
                    return "Команда распознана, но пока не реализована.";

                case "GetElementById":
                    return "Команда распознана, но пока не реализована.";

                case "SetParameter":
                    return "Функция установки параметров ещё не реализована.";

                case "CreateSheet":
                    return "Функция создания листов ещё не реализована.";

                case "CheckCollisions":
                    return "Функция проверки коллизий ещё не реализована.";

                default:
                    return "Команда не распознана. Пожалуйста, попробуйте снова.";
            }
        }
    }
}

