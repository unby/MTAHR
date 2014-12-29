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
using BaseType;
using MahApps.Metro.Controls;
using ManagementGui.ViewModel;

namespace ManagementGui
{
    /// <summary>
    /// Логика взаимодействия для WindowSystemListUsers.xaml
    /// </summary>
    public partial class WindowSystemListUsers : MetroWindow
    {
        private SystemUsersViewModel model { get; set; }

        public WindowSystemListUsers()
        {
            model=new SystemUsersViewModel();
            Content = model;
            InitializeComponent();
        }

        private void MICreateDivision_OnClick(object sender, RoutedEventArgs e)
        {
            model.ProjectCurrent=new Project(){IdProject = Guid.NewGuid(),DateCreate = DateTime.Now,Name = "Новое подразделение"
                ,Comment = "Пустой комментарий",Purpose = "Проект без целей"};
            model.Projects.Add(model.ProjectCurrent);
        }
    }
}
