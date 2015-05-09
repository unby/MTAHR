using System.Windows.Input;
using BaseType;
using MahApps.Metro.Controls;

namespace ManagementGui.Admin
{
    public partial class PointEnterWindows : MetroWindow
    {
        public PointEnterViewModel View { get; set; }
        public PointEnterWindows(ApplicationUser applicationUser)
        {
            InitializeComponent();
            View = new PointEnterViewModel(applicationUser);
            DataContext = View;
        }
    }
}
