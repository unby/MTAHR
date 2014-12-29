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
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using ManagementGui.ViewModel;

namespace ManagementGui.Admin
{
    /// <summary>
    /// Логика взаимодействия для Projects.xaml
    /// </summary>
    public partial class Projects : MetroWindow
    {
        private AdminProjectsWindow view = null;
        public Projects()
        {
            InitializeComponent();
            view=new AdminProjectsWindow();
            DataContext = view;
        }

        private void BtnAuthor_OnClick(object sender, RoutedEventArgs e)
        {
            var userWindow=new SearchUserWindow();

            if (userWindow.ShowDialog() == true)
                view.Current.Author = userWindow.UserViewModel.Current;
        }
    }
}
