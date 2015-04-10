using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using BaseType;
using BaseType.Common;
using BaseType.Utils;
using GalaSoft.MvvmLight.Command;
using ManagementGui.Config;
using ManagementGui.Utils;
using ManagementGui.ViewModel.Report;
using ManagementGui.ViewModel.Validation;
using RelayCommand = BaseType.Common.RelayCommand;

namespace ManagementGui.ViewModel
{
    public class CurrentTasksViewModel: ValidationViewModelBase
    {

        public CurrentTasksViewModel()
        {
            StatusList = typeof (StatusTask).GetEnumItems<int>();
            Status = StatusTask.Open;
            StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            StartDate = StartDate.AddMonths(-1);
            EndDate = DateTime.Now;
            SelectedStatusMember = "1";
            TaskList = DbHelper.GetDbProvider.Tasks.
                        Where(x => x.Project == WorkEnviroment.CurrentProject.IdProject && x.Status == StatusTask.Open).OrderBy(s=>s.NameTask).ThenByDescending(t=>t.TaskRating).ToList();
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
        //CreateNewTask
        private RelayCommand _сreateNewTask;
        public ICommand CreateNewTask
        {
            get { return _сreateNewTask ?? (_сreateNewTask = new RelayCommand(CreateTask)); }
        }

        private void CreateTask(object obj)
        {
            MainWindow.DelegateWindowCreateTask(null);
        }  


        private RelayCommand _showReportCommand;
        /// <summary>
        /// Сформировать новый список задач
        /// </summary>
        public ICommand ShowReportCommand
        {
            get { return _showReportCommand ?? (_showReportCommand = new RelayCommand(CreateReport)); }
        }    
        public string DisplayStatusMember
        {
            get { return "Description"; }
        }

        RelayCommand<UserViewTasks> _mouseDoubleClickCommand;
        public ICommand MouseDoubleClickTasksGridCommand
        {
            get
            {
                if (_mouseDoubleClickCommand == null)
                {
                    _mouseDoubleClickCommand = new RelayCommand<UserViewTasks>(OpenTask);
                }
                return _mouseDoubleClickCommand;
            }
        }
        private void OpenTask(UserViewTasks obj)
        {
            if (obj != null && obj.IdTask != Guid.Empty)
                MainWindow.DelegateWindowOpenTask(obj.IdTask);
        }

        private List<Task> _taskList;
        public List<Task> TaskList
        {
            get { return _taskList; }
            set
            {
                _taskList = value;
                RaisePropertyChanged();
            }
        }

        private Task _selectedTask;
        public Task SelectedTask
        {
            get { return _selectedTask; }
            set
            {
                _selectedTask = value;
                RaisePropertyChanged();
            }
        }

        private async void CreateReport(object obj)
        {

            if (StatusCode.Count() < 0)
                MessageBox.Show("Укажите статус", "Укажите статус", MessageBoxButton.OK, MessageBoxImage.Question);
            try
            {
                TaskList =await DbHelper.GetDbProvider.Tasks.
                      Where(x => x.Project == WorkEnviroment.CurrentProject.IdProject && x.Status == StatusTask.Open
                          && x.DateCreate >= StartDate && x.DateCreate < EndDate && StatusCode.Contains((int)x.Status)
                      ).OrderBy(s => s.NameTask).ThenByDescending(t => t.TaskRating).ToListAsync();                                
            }
            catch (Exception ex)
            {
                Logger.MessageBoxException(ex);
            }
        }
    }
}
