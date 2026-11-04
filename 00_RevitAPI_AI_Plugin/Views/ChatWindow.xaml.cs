using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Autodesk.Revit.DB;
using System.Net.Http;
using System.Text.Json;
using RevitAPI_AI_Plugin.Helper;
using RevitAPI_AI_Plugin.Keyword;
using RevitAPI_AI_Plugin.Views;
using RevitAPI_AI_Plugin.WindowMain;
using RevitAPI_AI_Plugin.ModelProcessor;


namespace RevitAPI_AI_Plugin.Views
{
    /// <summary>
    /// Главное окно чата.
    /// </summary>
    public partial class ChatWindow : Window
    {
        private ChatWindowMain _chatWindowMain;

        /// <summary>
        /// Конструктор окна.
        /// </summary>
        public ChatWindow(Autodesk.Revit.DB.Document doc, bool useOpenAI)
        {
            InitializeComponent();

            // Инициализация основного обработчика
            _chatWindowMain = new ChatWindowMain(doc, useOpenAI);
            string mode = useOpenAI ? "режиме OpenAI" : "локальном режиме";
            ChatBox.Text = $"Привет! Я подключён к модели {doc.Title} и работаю в {mode}. Чем могу помочь?\n";
        }

        private async void SendButton_Click(object sender, RoutedEventArgs e)
        {
            string userInput = InputBox.Text;
            if (!string.IsNullOrEmpty(userInput))
            {
                ChatBox.Text += $"Вы: {userInput}\n";
                InputBox.Clear();

                string response = await _chatWindowMain.ProcessUserInput(userInput);
                ChatBox.Text += $"Я: {response}\n";
            }
        }
    }
}
