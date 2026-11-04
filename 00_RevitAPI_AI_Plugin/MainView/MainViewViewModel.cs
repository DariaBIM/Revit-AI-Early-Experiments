using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Autodesk.Revit.DB;
using RevitAPI_AI_Plugin.Keyword;
using RevitAPI_AI_Plugin.ModelProcessor;
using RevitAPI_AI_Plugin.Models;
using RevitAPI_AI_Plugin.Services;
using ZimovetsPluginLibrary.AI.Commands;
using ZimovetsPluginLibrary.AI.Contracts;
using ZimovetsPluginLibrary.AI.Models;
using ZimovetsPluginLibrary.AI.Services;
using ZimovetsPluginLibrary.Utils;

namespace RevitAPI_AI_Plugin.MainView
{
    public class MainViewViewModel : INotifyPropertyChanged
    {
        private readonly AIRequestService _aiRequestService;
        private readonly List<AIMessage> _messages;
        private readonly AICommandRouter _commandRouter;
        private readonly RevitModelProcessor _modelProcessor;
        private readonly Document _doc;
        private readonly bool _useOpenAI;
        private readonly string _userName;
        private string _userInput;
        private string _chatText;
        private bool _isBusy;

        public MainViewViewModel()
            : this(null, true)
        {
        }

        public MainViewViewModel(Document doc, bool useOpenAI)
        {
            _doc = doc;
            _useOpenAI = useOpenAI;
            _userName = _doc != null ? _doc.Application.Username : "User";
            _aiRequestService = new AIRequestService();
            if (_doc != null)
            {
                _modelProcessor = new RevitModelProcessor(_doc);
            }
            _messages = new List<AIMessage>();
            _chatText = string.Empty;
            _userInput = string.Empty;
            _isBusy = false;

            IList<IAICommand> commands = new List<IAICommand>();
            commands.Add(new ListCategoriesCommand());
            _commandRouter = new AICommandRouter(commands);

            SendCommand = new RelayCommand(
                async _ => await ExecuteSendAsync(),
                _ => !IsBusy && !string.IsNullOrWhiteSpace(UserInput));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public string UserInput
        {
            get { return _userInput; }
            set
            {
                if (_userInput != value)
                {
                    _userInput = value;
                    OnPropertyChanged(nameof(UserInput));
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }

        public string ChatText
        {
            get { return _chatText; }
            private set
            {
                if (_chatText != value)
                {
                    _chatText = value;
                    OnPropertyChanged(nameof(ChatText));
                }
            }
        }

        public bool IsBusy
        {
            get { return _isBusy; }
            private set
            {
                if (_isBusy != value)
                {
                    _isBusy = value;
                    OnPropertyChanged(nameof(IsBusy));
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }

        public ICommand SendCommand { get; private set; }

        public async Task ExecuteSendAsync()
        {
            if (IsBusy)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(UserInput))
            {
                return;
            }

            IsBusy = true;

            try
            {
                string currentUserInput = UserInput.Trim();

                _messages.Add(new AIMessage
                {
                    Role = "user",
                    Content = currentUserInput,
                    CreatedAt = DateTime.Now
                });

                string assistantResponse = await ProcessUserInputAsync(currentUserInput);
                _messages.Add(new AIMessage
                {
                    Role = "assistant",
                    Content = assistantResponse,
                    CreatedAt = DateTime.Now
                });

                ChatText = BuildChatText();
                UserInput = string.Empty;
            }
            finally
            {
                IsBusy = false;
            }
        }

        private string BuildChatText()
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < _messages.Count; i++)
            {
                AIMessage message = _messages[i];
                string prefix = string.Equals(message.Role, "assistant", StringComparison.OrdinalIgnoreCase) ? "AI" : _userName;
                DateTime createdAt = message.CreatedAt == default(DateTime) ? DateTime.Now : message.CreatedAt;
                sb.AppendLine(prefix + " — " + createdAt.ToString("dd.MM.yyyy HH:mm") + ":");
                sb.AppendLine(message.Content ?? string.Empty);
                if (i < _messages.Count - 1)
                {
                    sb.AppendLine();
                }
            }

            return sb.ToString();
        }

        private async Task<string> ProcessUserInputAsync(string userInput)
        {
            AICommandResult commandResult = _commandRouter.TryExecute(_doc, userInput);
            if (commandResult.IsSuccess)
            {
                return commandResult.Message;
            }

            if (!string.Equals(commandResult.ErrorMessage, "Команда не распознана.", StringComparison.Ordinal))
            {
                return commandResult.ErrorMessage;
            }

            KeywordProcessor keywordProcessor = new KeywordProcessor();
            string command = keywordProcessor.ProcessKeyword(userInput);

            if (command != null)
            {
                return await HandleLocalCommand(command, userInput);
            }

            if (_useOpenAI)
            {
                AIResponseModel response = await _aiRequestService.SendAsync(_messages);
                if (response.IsSuccess)
                {
                    return response.AnswerText;
                }

                return response.ErrorMessage;
            }

            return "Я могу выполнять только встроенные команды.\n\n" +
                   "Для этой задачи нужен:\n" +
                   "— либо отдельный Revit-плагин\n" +
                   "— либо подключение AI (OPENAI_API_KEY)";
        }

        private async Task<string> HandleLocalCommand(string command, string userInput)
        {
            if (_modelProcessor == null)
            {
                return "Документ Revit не найден или закрыт.";
            }

            switch (command)
            {
                case "ListCategories":
                    return await Task.Run(() =>
                    {
                        List<string> categories = _modelProcessor.Categories
                            .GetAllCategories()
                            .OrderBy(c => c, StringComparer.OrdinalIgnoreCase)
                            .ToList();

                        StringBuilder categoriesText = new StringBuilder();
                        categoriesText.AppendLine("Категории в модели:");
                        categoriesText.AppendLine();

                        for (int i = 0; i < categories.Count; i++)
                        {
                            categoriesText.AppendLine("- " + categories[i]);
                        }

                        return categoriesText.ToString().TrimEnd();
                    });

                case "ExportDWG":
                    return await Task.Run(() => _modelProcessor.Export.ExportToDWG("C:\\Exports"));

                case "ExportNWC":
                    return await Task.Run(() => _modelProcessor.Export.ExportToNWC("C:\\Exports"));

                case "CheckParameters":
                    BuiltInCategory category = BuiltInCategory.OST_Walls;
                    string parameterName = "Комментарии";
                    return await Task.Run(() =>
                    {
                        List<string> issues = _modelProcessor.ParameterChecker.CheckParameterForCategory(category, parameterName);
                        return issues.Count > 0
                            ? "Обнаружены проблемы с параметром '" + parameterName + "':\n" + string.Join("\n", issues)
                            : "Все элементы категории '" + category + "' имеют заполненный параметр '" + parameterName + "'.";
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

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
