using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Autodesk.Revit.DB;
using RevitAPI_AI_Plugin.MainView;

namespace RevitAPI_AI_Plugin.DockablePane
{
    public partial class AIChatPane : UserControl
    {
        private INotifyPropertyChanged _currentNotifySource;

        public AIChatPane()
        {
            InitializeComponent();
            DataContextChanged += OnDataContextChanged;
            DataContext = new MainViewViewModel();
        }

        public AIChatPane(Document doc, bool useOpenAI)
        {
            InitializeComponent();
            DataContextChanged += OnDataContextChanged;
            DataContext = new MainViewViewModel(doc, useOpenAI);
        }

        public void Initialize(Document doc, bool useOpenAI)
        {
            DataContext = new MainViewViewModel(doc, useOpenAI);
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            INotifyPropertyChanged oldNotifySource = e.OldValue as INotifyPropertyChanged;
            if (oldNotifySource != null)
            {
                oldNotifySource.PropertyChanged -= OnViewModelPropertyChanged;
            }

            _currentNotifySource = e.NewValue as INotifyPropertyChanged;
            if (_currentNotifySource != null)
            {
                _currentNotifySource.PropertyChanged += OnViewModelPropertyChanged;
            }
        }

        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e != null && e.PropertyName == "ChatText")
            {
                ChatScrollViewer.ScrollToEnd();
            }
        }

        private void UserInputTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e == null || e.Key != Key.Enter)
            {
                return;
            }

            if ((Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift)
            {
                return;
            }

            e.Handled = true;

            MainViewViewModel viewModel = DataContext as MainViewViewModel;
            if (viewModel == null || viewModel.SendCommand == null)
            {
                return;
            }

            if (viewModel.SendCommand.CanExecute(null))
            {
                viewModel.SendCommand.Execute(null);
            }
        }
    }
}
