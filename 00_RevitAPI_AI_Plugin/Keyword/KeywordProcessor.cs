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

namespace RevitAPI_AI_Plugin.Keyword
{
    public class KeywordProcessor
    {
        private readonly Dictionary<string, List<string>> _keywords;

        public KeywordProcessor()
        {
            // Список ключевых слов и их вариаций
            _keywords = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase)
            {
                // Категории
                { "ListCategories", new List<string> { "Список категорий", "Какие категории есть?", "List categories", "Liste der Kategorien", "¿Qué categorías hay?" } }

                // Временно отключенные команды (этап 19):
                // ExportDWG, ExportNWC, ExportPDF, GetElementById, CheckParameters,
                // SetParameter, CreateSheet, CheckCollisions.
                //
                // TODO: безопасные read-only команды для следующего этапа:
                // - GetProjectName
                // - GetActiveViewName
                // - GetRevitUserName
            };
        }

        /// <summary>
        /// Обрабатывает пользовательский ввод и возвращает идентификатор команды.
        /// </summary>
        public string ProcessKeyword(string userInput)
        {
            // Приводим ввод пользователя к нижнему регистру
            string lowerInput = userInput.ToLower();

            // Проверяем все ключевые слова и их вариации
            foreach (var command in _keywords)
            {
                if (command.Value.Any(variation => lowerInput.Contains(variation.ToLower())))
                {
                    return command.Key;
                }
            }

            // Если ключевое слово не найдено
            return null;
        }
    }
}
