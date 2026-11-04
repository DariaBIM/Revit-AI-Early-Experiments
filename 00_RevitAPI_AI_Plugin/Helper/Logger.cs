using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using RevitAPI_AI_Plugin.Helper;
using RevitAPI_AI_Plugin.Keyword;
using RevitAPI_AI_Plugin.Views;
using RevitAPI_AI_Plugin.WindowMain;


namespace RevitAPI_AI_Plugin.Helper
{
    public class Logger
    {
        private string _logFilePath;

        public Logger()
        {
            // Путь к файлу логов
            string logDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "RevitAPI_AI_Plugin");
            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }

            _logFilePath = Path.Combine(logDirectory, "plugin_logs.txt");
        }

        public void Log(string message)
        {
            string logMessage = $"{DateTime.Now}: {message}";
            File.AppendAllText(_logFilePath, logMessage + Environment.NewLine);
        }
    }
}
