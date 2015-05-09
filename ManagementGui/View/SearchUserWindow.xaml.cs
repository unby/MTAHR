using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using BaseType;
using MahApps.Metro.Controls;
using ManagementGui.Utils;
using ManagementGui.ViewModel;

namespace ManagementGui.View
{
    [Flags]
    public enum SearchWindowMode
    {
        Admin =0,
        AllBase=1,
        MultiSearch=2,
        Import=4,
        OnlyProject=8
    }
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
            View.IsAdminModeVisibility = Visibility.Hidden;
        }
        public SearchUserWindow(SearchWindowMode mode)
        {
            View = new SearchUserViewModel();
            InitializeComponent();
            DataContext = View;
            SetWindowMode(mode);
            IsWorkcb.IsChecked = true;
        }

        private void SetWindowMode(SearchWindowMode mode)
        {
            if (mode.HasFlag(SearchWindowMode.MultiSearch))
            {
                UserSearchGrid.SelectionMode=DataGridSelectionMode.Extended;
            }
            if (mode.HasFlag(SearchWindowMode.Admin))
            {
                ManageWPanel.Visibility = Visibility.Visible;
                UserPropertyGrid.IsEnabled = true;
                PropertyViewer.IsEnabled = true;
            }
            if (mode.HasFlag(SearchWindowMode.Import))
            {
                ManageWPanel.Visibility = Visibility.Visible;
                UserPropertyGrid.IsEnabled = true;
                PropertyViewer.IsEnabled = true;
            }          
            if (mode.HasFlag(SearchWindowMode.AllBase))
            {
                Allrb.Visibility = Visibility.Visible;
                Allrb.IsChecked = true;
            }
            if (mode.HasFlag(SearchWindowMode.OnlyProject))
            {
                Projectrb.Visibility = Visibility.Visible;
                Projectrb.IsChecked = true;
            }
        }

        private void BtnPointEnter_OnClick(object sender, RoutedEventArgs e)
        {
           // throw new System.NotImplementedException();
        }
    }
}
