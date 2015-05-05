using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Windows.Input;
using BaseType;
using BaseType.Common;
using BaseType.Utils;
using ManagementGui.Config;
using ManagementGui.Utils;
using ManagementGui.ViewModel.Validation;
using Microsoft.Reporting.WinForms;

namespace ManagementGui.ViewModel.Report
{
    public class ConsolidateReportViewModel: ValidationViewModelBase
    {

        public ConsolidateReportViewModel()
        {
        }

        public ConsolidateReportViewModel(View.Report.ConsolidateReport reportTasks)
        {
            StatusList = typeof(StatusTask).GetEnumItems<int>();
            Status = StatusTask.Open;
            StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            StartDate = StartDate.AddMonths(-1);
            EndDate = DateTime.Now;
            _reportTasks = reportTasks;
            foreach (var enumItem in StatusList)
            {
                SelectedMember = SelectedMember+ enumItem.Code + ',';
            }
           SelectedMember= SelectedMember.RemoveEndString(",");
        }

        public List<EnumItem<int>> StatusList { get; set; }
        public StatusTask Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        private RelayCommand _showReportCommand;
        private readonly View.Report.ConsolidateReport _reportTasks;
        public ICommand ShowReportCommand
        {
            get { return _showReportCommand ?? (_showReportCommand = new RelayCommand(CreateReport)); }
        }

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

        public string DisplayMember
        {
            get { return "Description"; }
        }

        private async void CreateReport(object obj)
        {          
            try
            {
                var dbContext = DbHelper.GetDbProvider;
                var query = await (from user in dbContext.Users
                                   join member in dbContext.TaskMembers on user.Id equals member.IdUser
                                   join task in dbContext.Tasks on member.IdTask equals task.IdTask
                                   where task.Project == WorkEnviroment.CurrentProject.IdProject &&
                                   task.DateCreate >= StartDate && task.DateCreate < EndDate &&
                                   StatusCode.Contains((int)task.Status)
                                   group new{user,task} by user into g
                                   let gr=g.FirstOrDefault()
                                   let fio=gr.user.Surname + " " + gr.user.Name + " " + gr.user.MiddleName
                                   let sum=g.Sum(s=>gr.task.TaskRating)
                                   let count=g.Count()
                                       select new ConsolidateReportModelTemplate
                                           {
                                               FIO = fio,
                                               SumWeight = sum,
                                               CountTask = count
                                           }).ToListAsync();

                _reportTasks.TaskReportViewer.ProcessingMode = ProcessingMode.Local;
                _reportTasks.TaskReportViewer.LocalReport.ReportEmbeddedResource = "ManagementGui.View.Report.ConsolidateReportTemplate.rdlc"; 
                var dataSource = new ReportDataSource("DataSet1", query);
                _reportTasks.TaskReportViewer.LocalReport.DataSources.Clear();
                var reportname = string.Format("Консолидированный отчет по исполнителям c {0} по {1}",
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
