using System.Windows;
using BaseType;
using MahApps.Metro.Controls;
using ManagementGui.Admin;
using ManagementGui.ViewModel;

namespace ManagementGui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        private readonly AuthenticationWindows authenticationWindows;
        public MainWindow()
        {

            var model = new AuthenticationWindowsViewModel();
            authenticationWindows = new AuthenticationWindows { DataContext = model };
            if (authenticationWindows.ShowDialog() == true)
            {
                //Title = "~Вы начали работу~";
                InitializeComponent();
                ManageSystemMI.IsEnabled = authenticationWindows.IsModerator;
            }
            else
            {
                Application.Current.Shutdown();
            }
            // if(authenticationWindows.DialogResult==)
           // InitializeComponent();
            //this.Show();
            //Closing += (s, e) => ViewModelLocator.Cleanup();
        }

        private void MISistemUserList_OnClick(object sender, RoutedEventArgs e)
        {
            var projectWindow=new Projects();

            projectWindow.Show();
        }
    }
}