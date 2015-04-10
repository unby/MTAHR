using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using BaseType.Common;
using ManagementGui.Config;
using ManagementGui.Properties;
using ManagementGui.Utils;
using ManagementGui.View.Document;
using BaseType;
using ManagementGui.ViewModel.Validation;

namespace ManagementGui.ViewModel
{
    public class TaskDocumentViewModel : ValidationViewModelBase
    {
        public ObservableCollection<TaskComment> Comments { get; set; }
        public ObservableCollection<UserTreeViewModel> Users { get; set; }

        public TaskDocumentViewModel()
        {
        }

        public TaskDocumentViewModel(Task task)
        {
            Task = task;
            Comments = new ObservableCollection<TaskComment>();
            Users = new ObservableCollection<UserTreeViewModel>();
            if (task.WorkGroup != null)
                foreach (var members in Task.WorkGroup)
                    Users.Add(new UserTreeViewModel(members.User, members.IdTask));

            if (task.WorkGroup != null && string.IsNullOrEmpty(task.NameTask))
            {
                if (task.Notivications == null)
                    task.Notivications = new ObservableCollection<Notivication>();
                task.Notivications.Add(new Notivication
                {
                    NotivicationStatus = NotivicationStatus.Declared,
                    DateCreate = DateTime.Now,
                    Description = "На вас назначена новая задача",
                    Task = task,
                    From = WorkEnviroment.ApplicationUserSession,
                    To = task.WorkGroup.First().User,
                    IdNotivication = Guid.NewGuid(),
                    IdTask = task.IdTask,
                    IdUserFrom = WorkEnviroment.ApplicationUserSession.Id,
                    IdUserTo = task.WorkGroup.First().User.Id,
                    TimeSend = DateTime.Now
                });}
        }

        public Task Task { get; set; }
        public StatusTask Status
        {
            get { return Task.Status; }
            set { Task.Status = value; }
        }
        [Range(1, 100, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "TaskDocumentViewModel_DayNotivication_Error_Message")]
        public int DayNotivication { get; set; }

        public string MessageSend {
            get { return _messageSend; }
            set { _messageSend = value; }
        }

        public ICommand Save
        {
            get
            {
                if (_save == null)
                    _save = new RelayCommand(SaveTask);
                return _save;
            }
        }

        private async void SaveTask(object obj)
        {
            try
            {
                if (BaseType.Utils.EntityValidate.CostumValidator(Task))
                {

                    SaveModel();
                    await DbHelper.GetDbProvider.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Logger.MessageBoxException(ex);
            }
        }

        private RelayCommand _setNotivicationAll;
        private RelayCommand _save;

        public ICommand SetNotivicationAll
        {
            get
            {
                if(_setNotivicationAll==null)
                    _setNotivicationAll = new RelayCommand(SetNotivicationAllUserInTask);
                return _setNotivicationAll;
            }
        }
        
        private void SetNotivicationAllUserInTask(object obj)
        {
            if (DayNotivication > 0)
            {
                var date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 9, 0, 0);
                date = date.AddDays(DayNotivication);
                foreach (var member in Task.WorkGroup)
                {
                    Task.Notivications.Add(new Notivication
                    {
                        NotivicationStatus = NotivicationStatus.Declared,
                        DateCreate = date,
                        Description =
                            string.Format("По задаче '{0}', требуется предоставить результат к {1}", Task.NameTask,
                                Task.DateFinish.ToShortDateString()),
                        Task = Task,
                        From = WorkEnviroment.ApplicationUserSession,
                        To = member.User,
                        IdNotivication = Guid.NewGuid(),
                        IdTask = Task.IdTask,
                        IdUserFrom = WorkEnviroment.ApplicationUserSession.Id,
                        IdUserTo = member.User.Id,
                    });
                }
            }
            else
            {
                MessageBox.Show("Укажите через солько дней будет отправлено уведомление", "Внимание",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public void SaveModel()
        {
            Task.DateUpdate = DateTime.Now;
            DbHelper.GetDbProvider.Tasks.AddOrUpdate(Task);
        }

        public string _messageSend { get; set; }
    }
}
