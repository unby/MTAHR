using System.Data.Entity;
using System.Windows;
using System.Windows.Input;
using BaseType;
using BaseType.Common;
using BaseType.Utils;
using ManagementGui.Config;
using ManagementGui.Utils;
using ManagementGui.ViewModel.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Reporting.WinForms;

namespace ManagementGui.ViewModel.Report
{
    public class ReportTaskViewModel : ValidationViewModelBase
    {

        public ReportTaskViewModel()
        {
        }

        public ReportTaskViewModel(View.Report.ReportTasks reportTasks)
        {
            StatusList = typeof (StatusTask).GetEnumItems<int>();
            Status=StatusTask.Open;
            StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            StartDate = StartDate.AddMonths(-1);
            EndDate = DateTime.Now;
            _reportTasks = reportTasks;
            SelectedStatusMember = "1";
        }
        //SelectedMemberPath
        private string _selectedStatusMember;
        public int[] StatusCode { get { return (SelectedStatusMember).Split(',').Select(int.Parse).ToArray(); } }
        public string SelectedStatusMember
        {
            get { return _selectedStatusMember; }
            set
            {
                _selectedStatusMember = value;
                RaisePropertyChanged();
            }
        }
        public List<EnumItem<int>> StatusList { get; set; }
        public StatusTask Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        private RelayCommand _showReportCommand;
        private readonly View.Report.ReportTasks _reportTasks;
        public ICommand ShowReportCommand
        {
            get { return _showReportCommand ?? (_showReportCommand = new RelayCommand(CreateReport)); }
        }
     
        public string DisplayStatusMember
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
                        join userMembers in dbContext.TaskMembers on task.IdTask equals userMembers.IdTask
                        join user in dbContext.Users on userMembers.IdUser equals user.Id
                        where
                            task.Project==WorkEnviroment.CurrentProject.IdProject&&
                            task.DateCreate >= StartDate && task.DateCreate < EndDate &&
                            StatusCode.Contains((int)task.Status)
                        select new ReportTaskModelTemplate{
                            Comment = task.Comment,
                            FIO =  user.Surname+" "+user.Name+" "+user.MiddleName,
                            Status = task.Status,
                            TaskCreate = task.DateCreate,
                            TaskUpdate = task.DateUpdate,
                            TaskName = task.NameTask
                        }).OrderByDescending(b=>b.TaskCreate).ToListAsync();

                _reportTasks.TaskReportViewer.ProcessingMode = ProcessingMode.Local;
                _reportTasks.TaskReportViewer.LocalReport.ReportEmbeddedResource = "ManagementGui.View.Report.ReportTasksTemplate.rdlc"; // .Reports if the report isin the Reports folder not in the root
                var dataSource = new ReportDataSource("DataSet1", query);
                _reportTasks.TaskReportViewer.LocalReport.DataSources.Clear();
                var reportname = string.Format("Отчет поставленных задач за период c {0} по {1}",
                    StartDate.ToString("dd.MM.yyyy"), EndDate.ToString("dd.MM.yyyy"));
                _reportTasks.TaskReportViewer.LocalReport.DisplayName = reportname;
                _reportTasks.TaskReportViewer.LocalReport.SetParameters(new ReportParameter("HeaderReport", reportname));
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
