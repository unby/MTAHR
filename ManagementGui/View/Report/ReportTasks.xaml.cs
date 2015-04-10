using System;
using System.Windows.Controls;
using BaseType.Report;
using ManagementGui.ViewModel.Report;
using Microsoft.Reporting.WinForms;

namespace ManagementGui.View.Report
{
    public partial class ReportTasks : UserControl
    {
        ReportTaskViewModel View { get; set; }
        public ReportTasks()
        {                    
            View=new ReportTaskViewModel(this);
            InitializeComponent();
            TaskReportViewer.Messages = new RussianReportViewerMessages();
            DataContext = View;
        }

        public ReportTasks(IReportViewerMessages languageMessages)
        {
            TaskReportViewer.Messages = languageMessages;
            View = new ReportTaskViewModel(this);
            InitializeComponent();
            DataContext = View;
        }
    }
}
