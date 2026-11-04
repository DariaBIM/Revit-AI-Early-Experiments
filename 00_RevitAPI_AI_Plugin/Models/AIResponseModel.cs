namespace RevitAPI_AI_Plugin.Models
{
    public class AIResponseModel
    {
        public bool IsSuccess { get; set; }

        public string AnswerText { get; set; }

        public string ErrorMessage { get; set; }
    }
}
