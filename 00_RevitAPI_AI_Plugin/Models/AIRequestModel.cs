using System.Collections.Generic;

namespace RevitAPI_AI_Plugin.Models
{
    public class AIRequestModel
    {
        public string UserText { get; set; }

        public List<AIMessage> Messages { get; set; }
    }
}
