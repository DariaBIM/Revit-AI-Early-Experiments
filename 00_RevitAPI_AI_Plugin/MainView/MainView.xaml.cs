using System.Windows;
using Autodesk.Revit.DB;

namespace RevitAPI_AI_Plugin.MainView
{
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();
            DataContext = new MainViewViewModel();
        }

        public MainView(Document doc, bool useOpenAI)
        {
            InitializeComponent();
            DataContext = new MainViewViewModel(doc, useOpenAI);
        }
    }
}
