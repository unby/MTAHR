using System.Windows.Input;
using BaseType;
using MahApps.Metro.Controls;

namespace ManagementGui.Admin
{
    public partial class PointEnterWindows : MetroWindow
    {
        private SearchUserWindow ViewWindow;
        private ApplicationUser _applicationUser;
        public PointEnterViewModel View { get; set; }
        public PointEnterWindows(ApplicationUser applicationUser)
        {
            InitializeComponent();
            View = new PointEnterViewModel();
            DataContext = View;
            _applicationUser = applicationUser;
        }

        public PointEnterWindows(SearchUserWindow searchUserWindow)
        {
            this.ViewWindow = searchUserWindow;
            InitializeComponent();
            View = new PointEnterViewModel();
            DataContext = View;
        }

        private void MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
           // GridDoubleClick.IsOpen = true;
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }
    }


}
