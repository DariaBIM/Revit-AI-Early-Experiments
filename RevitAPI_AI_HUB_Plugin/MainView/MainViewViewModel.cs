using RevitAPI_AI_HUB_Plugin.Models;
using RevitAPI_AI_HUB_Plugin.Services;
using ZimovetsPluginLibrary.Utils;

namespace RevitAPI_AI_HUB_Plugin.MainView
{
    public class MainViewViewModel : ViewModelBase
    {
        private readonly AIHubService _aiHubService;

        private string _userInput;
        private string _result;

        public MainViewViewModel()
        {
            _aiHubService = new AIHubService();

            AnalyzeCommand = new RelayCommand(_ =>
            {
                var request = new HubRequestModel
                {
                    UserInput = UserInput
                };

                var response = _aiHubService.Analyze(request);
                Result = response != null ? response.Message : string.Empty;
            });
        }

        public string UserInput
        {
            get => _userInput;
            set
            {
                if (_userInput == value)
                {
                    return;
                }

                _userInput = value;
                OnPropertyChanged(nameof(UserInput));
            }
        }

        public string Result
        {
            get => _result;
            set
            {
                if (_result == value)
                {
                    return;
                }

                _result = value;
                OnPropertyChanged(nameof(Result));
            }
        }

        public RelayCommand AnalyzeCommand { get; }
    }
}
