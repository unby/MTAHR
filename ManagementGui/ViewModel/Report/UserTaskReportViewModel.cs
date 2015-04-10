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
    public class UserTaskReportViewModel: ValidationViewModelBase
    {

        public UserTaskReportViewModel()
        {
        }

        public UserTaskReportViewModel(View.Report.UserTaskReport reportTasks)
        {
            StatusList = typeof(StatusTask).GetEnumItems<int>();
            Status = StatusTask.Open;
            StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            StartDate = StartDate.AddMonths(-1);
            EndDate = DateTime.Now;
            _reportTasks = reportTasks;
            UserList=new List<EnumItem<Guid>>(MainWindow.View.Users.Select(s=>new EnumItem<Guid>{Code = s.Id,Description=s.UserShortName()}));
            foreach (var enumItem in UserList)
            {
                SelectedUserMember = SelectedUserMember + enumItem.Code + ',';
            }
            SelectedUserMember = SelectedUserMember.RemoveEndString(",");
            foreach (var enumItem in StatusList)
            {
                SelectedStatusMember = SelectedStatusMember+ enumItem.Code + ',';
            }
           SelectedStatusMember= SelectedStatusMember.RemoveEndString(",");
        }
        

        private string _selectedUserMember;
        public Guid[] UserCode { get { return (SelectedUserMember).Split(',').Select(Guid.Parse).ToArray(); } }
        public string SelectedUserMember
        {
            get { return _selectedUserMember; }
            set
            {
                _selectedUserMember = value;
                RaisePropertyChanged();
            }
        }
        public List<EnumItem<Guid>> UserList { get; set; }
        public List<EnumItem<int>> StatusList { get; set; }
        public StatusTask Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        private RelayCommand _showReportCommand;
        private readonly View.Report.UserTaskReport _reportTasks;
        public ICommand ShowReportCommand
        {
            get { return _showReportCommand ?? (_showReportCommand = new RelayCommand(CreateReport)); }
        }

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

        public string DisplayStatusMember
        {
            get { return "Description"; }
        }

        private async void CreateReport(object obj)
        {          
            try
            {
                var dbContext = DbHelper.GetDbProvider;
                var query = await (from user in dbContext.Users.Where(w=>UserCode.Contains(w.Id))
                                   join member in dbContext.TaskMembers on user.Id equals member.IdUser
                                   join task in dbContext.Tasks.Where(w=>w.Project== WorkEnviroment.CurrentProject.IdProject) on member.IdTask equals task.IdTask
                                   where task.DateCreate >= StartDate && task.DateCreate < EndDate &&
                                   StatusCode.Contains((int)task.Status)
                                   group new{user,task} by user into g
                                   let gr=g.FirstOrDefault()
                                   select new UserTaskReportModelTemplate
                                           {
                                               FIO = gr.user.Surname + " " + gr.user.Name + " " + gr.user.MiddleName,
                                               NameTask = gr.task.NameTask,
                                               Weight = gr.task.TaskRating,
                                               Status = gr.task.Status
                                           }
                                           ).OrderBy(o=>o.FIO).ThenByDescending(o=>o.Weight).ToListAsync();
                _reportTasks.TaskReportViewer.ProcessingMode = ProcessingMode.Local;
                _reportTasks.TaskReportViewer.LocalReport.ReportEmbeddedResource = "ManagementGui.View.Report.UserTaskReport.rdlc"; 
                var dataSource = new ReportDataSource("DataSet1", query);
                _reportTasks.TaskReportViewer.LocalReport.DataSources.Clear();
                var reportname = string.Format("Задачи исполнителей c {0} по {1}",
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
