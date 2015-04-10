using BaseType;
using MahApps.Metro.Controls;
using ManagementGui.ViewModel;
using Xceed.Wpf.Toolkit;

namespace ManagementGui.View
{
    /// <summary>
    /// Логика взаимодействия для NotivicationWindow.xaml
    /// </summary>
    public partial class NotivicationWindow : MetroWindow
    {
        public NotivicationWindow()
        {
            InitializeComponent();
        }

        public NotivicationViewModel ViewModel;
        public NotivicationWindow(Notivication notivication)
        {
            ViewModel = new NotivicationViewModel(notivication);
            InitializeComponent();
           // RichTextBox.TextFormatter = new XamlFormatter(); 
            DataContext = ViewModel;
        }
    }
}
