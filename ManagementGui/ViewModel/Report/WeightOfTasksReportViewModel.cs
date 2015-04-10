using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using BaseType;
using BaseType.Common;
using ManagementGui.Config;
using ManagementGui.Utils;
using ManagementGui.ViewModel.Validation;
using Microsoft.Reporting.WinForms;

namespace ManagementGui.ViewModel.Report
{
    public class WeightOfTasksReportViewModel: ValidationViewModelBase
    {

        public WeightOfTasksReportViewModel()
        {
        }

        public WeightOfTasksReportViewModel(View.Report.WeightOfTasksReport reportTasks)
        {
            StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            StartDate = StartDate.AddMonths(-1);
            EndDate = DateTime.Now;
            _reportTasks = reportTasks;
            SelectedMember = "1";
        }
        //SelectedMemberPath
        private string _selectedMember;
        public int[] StatusCode { get { return (SelectedMember).Split(',').Select(int.Parse).ToArray(); } }
        public string SelectedMember
        {
            get { return _selectedMember; }
            set
            {
                _selectedMember = value;
                RaisePropertyChanged();
            }
        }
        public List<EnumItem<int>> StatusList { get; set; }
        public StatusTask Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        private RelayCommand _showReportCommand;
        private readonly View.Report.WeightOfTasksReport _reportTasks;
        public ICommand ShowReportCommand
        {
            get { return _showReportCommand ?? (_showReportCommand = new RelayCommand(CreateReport)); }
        }
     
        public string DisplayMember
        {
            get { return "Description"; }
        }

        private async void CreateReport(object obj)
        {
            if (StatusCode.Count() < 0)
                MessageBox.Show("Укажите статус", "Укажите статус", MessageBoxButton.OK, MessageBoxImage.Question);
            try
            {
                var dbContext = DbHelper.GetDbProvider;
                var query =
                    await (from task in dbContext.Tasks
                        where
                            task.Project == WorkEnviroment.CurrentProject.IdProject &&
                            task.DateCreate >= StartDate && task.DateCreate < EndDate
                        orderby task.Status
                        select new WeightOfTasksReportModelTemplate
                        {
                            TaskName = task.NameTask,
                            Weight = task.TaskRating,
                            Status = task.Status,
                        }).ToListAsync();

                _reportTasks.TaskReportViewer.ProcessingMode = ProcessingMode.Local;
                _reportTasks.TaskReportViewer.LocalReport.ReportEmbeddedResource =
                    "ManagementGui.View.Report.WeightOfTasksReportTemplate.rdlc";
                    
                var dataSource = new ReportDataSource("DataSet1", query);
                _reportTasks.TaskReportViewer.LocalReport.DataSources.Clear();
                var reportname = string.Format("Отчет поставленных задач за период c {0} по {1}",
                    StartDate.ToString("dd.MM.yyyy"), EndDate.ToString("dd.MM.yyyy"));
                _reportTasks.TaskReportViewer.LocalReport.DisplayName = reportname;
              //  _reportTasks.TaskReportViewer.LocalReport.SetParameters(new ReportParameter("HeaderReport", reportname));
                _reportTasks.TaskReportViewer.LocalReport.DataSources.Add(dataSource);

                _reportTasks.TaskReportViewer.RefreshReport();
            }
            catch (Exception ex)
            {
                Logger.MessageBoxException(ex);
            }
        }
    }
}

