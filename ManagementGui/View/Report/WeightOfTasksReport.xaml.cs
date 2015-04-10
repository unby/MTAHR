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
using System.Windows.Navigation;
using System.Windows.Shapes;
using ManagementGui.ViewModel.Report;
using Microsoft.Reporting.WinForms;

namespace ManagementGui.View.Report
{
    /// <summary>
    /// Логика взаимодействия для WeightOfTasksReport.xaml
    /// </summary>
    public partial class WeightOfTasksReport : UserControl
    {
        WeightOfTasksReportViewModel View { get; set; }
        public WeightOfTasksReport()
        {
            View = new WeightOfTasksReportViewModel(this);
            InitializeComponent();
            TaskReportViewer.Messages = new RussianReportViewerMessages();
            DataContext = View;
        }

        public WeightOfTasksReport(IReportViewerMessages languageMessages)
        {
            TaskReportViewer.Messages = languageMessages;
            View = new WeightOfTasksReportViewModel(this);
            InitializeComponent();
            DataContext = View;
        }
    }
}
