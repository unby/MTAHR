using System;
using System.Diagnostics;
using System.Windows;
using MahApps.Metro.Controls;
using ManagementGui.View;

namespace ManagementGui.Admin
{
    /// <summary>
    /// Логика взаимодействия для ProjectsWindow.xaml
    /// </summary>
    public partial class ProjectsWindow : MetroWindow
    {
        private AdminProjectsWindow view = null;
        public ProjectsWindow()
        {
            InitializeComponent();
            view=new AdminProjectsWindow();
            DataContext = view;
        }

        private void BtnAuthor_OnClick(object sender, RoutedEventArgs e)
        {
            var userWindow = new SearchUserWindow(SearchWindowMode.Admin|SearchWindowMode.AllBase);
            try
            {
                if (userWindow.ShowDialog() != true) return;
                if (view.Current == null)
                    view.Current = view.Projects[0];
                    
                view.Current.Author = userWindow.View.CurrentUser;
                // view.ContextData.Entry(view.Current.Author).State=EntityState.Modified;
                //view.ContextData.ProjectsWindow.Where(w => w.IdProject == view.Current.IdProject)
                //    .Update(u => new Project(){Author = view.Current.Author});
                //view.Current.DateUpdate = DateTime.Now;
                //view.ContextData.SaveChanges();
            }
            catch (Exception ex)
            {
                Debug.Print(ex.Message);
                MessageBox.Show(ex.StackTrace, ex.Message, MessageBoxButton.OK);
            }
           
        }
    }
}
