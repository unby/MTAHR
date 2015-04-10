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
    /// Логика взаимодействия для ConsolidateReport.xaml
    /// </summary>
    public partial class ConsolidateReport : UserControl
    {
        ConsolidateReportViewModel View { get; set; }
        public ConsolidateReport()
        {
            View = new ConsolidateReportViewModel(this);
            InitializeComponent();
            TaskReportViewer.Messages = new RussianReportViewerMessages();
            DataContext = View;
        }

        public ConsolidateReport(IReportViewerMessages languageMessages)
        {
            TaskReportViewer.Messages = languageMessages;
            View = new ConsolidateReportViewModel(this);
            InitializeComponent();
            DataContext = View;
        }
    }
}
