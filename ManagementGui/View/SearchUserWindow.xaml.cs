using System.Windows.Controls;
using MahApps.Metro.Controls;
using ManagementGui.ViewModel;

namespace ManagementGui.View
{
    /// <summary>
    /// Логика взаимодействия для SearchUserWindow.xaml
    /// </summary>
    public partial class SearchUserWindow : MetroWindow
    {
        public SearchUserViewModel View;
        public SearchUserWindow()
        {
            View=new SearchUserViewModel();
            InitializeComponent();
            DataContext = View;
            UserSearchGrid.SelectionMode=DataGridSelectionMode.Extended;
        }
        public SearchUserWindow(DataGridSelectionMode mode)
        {
            View = new SearchUserViewModel();
            InitializeComponent();
            DataContext = View;
            UserSearchGrid.SelectionMode = mode;
        }
    }
}
