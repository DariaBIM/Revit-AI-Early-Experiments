using System;

namespace RevitAPI_AI_Plugin.Models
{
    public class AIMessage
    {
        public string Role { get; set; }

        public string Content { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
