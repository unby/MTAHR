﻿using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Migrations;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using BaseType.Common;
using EntityFramework.Extensions;
using ManagementGui.Config;
using ManagementGui.Properties;
using ManagementGui.Utils;
using ManagementGui.View.Control;
using ManagementGui.View.Document;
using BaseType;
using ManagementGui.ViewModel.Menu;
using ManagementGui.ViewModel.Validation;
using NLog.Targets;
using MessageBox = System.Windows.MessageBox;

namespace ManagementGui.ViewModel
{
    public enum DescriptionsTask
    {
        [Description("Комментарий")]
        Comment,
        [Description("Итоговый результат")]
        Result,
        [Description("Описание")]
        Description
    }

    public class TaskDocumentViewModel : ValidationViewModelBase
    {
        public ObservableCollection<TaskComment> Comments { get; set; }
        public ObservableCollection<UserTreeViewModel> Users { get; set; }
        public ObservableCollection<WorkFile> Files { get; set; } 
        public TaskDocumentViewModel()
        {
            Files = new ObservableCollection<WorkFile>();
        }

        public TaskDocumentViewModel(Task task)
        {
         //   _selectedFiles.CollectionChanged += (sender, e) => UpdateSelectFiles();
            Task = task;
            Comments = new ObservableCollection<TaskComment>();
            Users = new ObservableCollection<UserTreeViewModel>();
            if (task.WorkGroup != null)
                foreach (var members in Task.WorkGroup)
                    Users.Add(new UserTreeViewModel(members, members.IdTask));
            if (task.Files != null)
            {
                Files = new ObservableCollection<WorkFile>();
                foreach (var file in task.Files)
                {
                    file.PropertyChanged+=file_Comment_PropertyChanged;
                    Files.Add(file);
                }
            }

            if (task.WorkGroup == null || !string.IsNullOrEmpty(task.NameTask)) return;
            if (task.Notivications == null)
            {
                task.Notivications = new ObservableCollection<Notivication>
                {
                    new Notivication
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
                    }
                };
            }
           
        }

        private static void file_Comment_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender == null) return;
            if (e.PropertyName == "Comment")
                DbHelper.GetDbProvider.WorkFiles.AddOrUpdate(sender as WorkFile);
        }

        public DescriptionsTask DescriptionsTask
        {
            get { return _descriptionsTask; }
            set { _descriptionsTask = value; RaisePropertyChanged();}
        }

        public string StringDescriptionTask
        {
            get
            {
                switch (DescriptionsTask)
                {
                    case DescriptionsTask.Comment:
                        return Task.Comment;
                    case DescriptionsTask.Description:
                        return Task.Description;
                    default:
                    case DescriptionsTask.Result:
                        return Task.Result;
                }
            }
            set
            {
                switch (DescriptionsTask)
                {
                    case DescriptionsTask.Comment:
                        Task.Comment = value;
                        break;
                    case DescriptionsTask.Description:
                        Task.Description = value;
                        break;
                    default:
                    case DescriptionsTask.Result:
                        Task.Result = value;
                        break;
                }
                RaisePropertyChanged();
            }
        }

        public Task Task { get; set; }
        public StatusTask Status
        {
            get { return Task.Status; }
            set
            {
                if(value==Task.Status)return;
                if (value == StatusTask.Complete)
                    Task.DateClose = DateTime.Now;
                Task.Status = value;
                RaisePropertyChanged();
            }
        }
        [Range(1, 100, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "TaskDocumentViewModel_DayNotivication_Error_Message")]
        public int DayNotivication { get; set; }

        public string MessageSend {
            get { return _messageSend; }
            set { _messageSend = value; }
        }

        public ICommand Save
        {
            get { return _save ?? (_save = new RelayCommand(SaveTask)); }
        }

        private async void SaveTask(object obj)
        {
            try
            {
                if (!BaseType.Utils.EntityValidate.CostumValidator(Task)) return;
                SaveModel();
                await DbHelper.GetDbProvider.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Logger.MessageBoxException(ex);
            }
        }

        private RelayCommand _setNotivicationAll;
        private RelayCommand _save;
        private RelayCommand _load;
        private RelayCommand _upLoadAll;
        private RelayCommand _upLoadSelected;

        public ICommand SetNotivicationAll
        {
            get {
                return _setNotivicationAll ?? (_setNotivicationAll = new RelayCommand(SetNotivicationAllUserInTask));
            }
        }

        public RelayCommand Load
        {
            get { return _load ?? (_load = new RelayCommand(LoadFile)); }
        }
        //UpLoadSelected
        public RelayCommand UpLoadSelected
        {
            get { return _upLoadSelected ?? (_upLoadSelected = new RelayCommand(UpLoadSelectedFile)); }
        }

        private void UpLoadSelectedFile(object obj)
        {
            using (var dialog = new FolderBrowserDialog {RootFolder = Environment.SpecialFolder.Desktop})
            {
                try
                {
                    if (DialogResult.OK != dialog.ShowDialog()) return;
                    foreach (var workFile in SelectedFiles)
                    {
                        workFile.UnloadingOfFileInFolder(dialog.SelectedPath,
                            ConfigHelper.DesktopSettings.ConnectionSettings.ToString());
                    }
                    Process.Start(dialog.SelectedPath);
                }
                catch (Exception ex)
                {
                    Logger.MessageBoxException(ex);
                }
            }
        }

        private async void LoadFile(object obj)
        {
            try
            {
            if (Task.DateUpdate == DateTime.MinValue)
            {
                MessageBox.Show("Перед добавлением файлов требуется сохранить задачу в базе данных", "Внимание",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (Task.Files == null)
                Task.Files = new ObservableCollection<WorkFile>();
            var fileDialog = new OpenFileDialog
            {
                SupportMultiDottedExtensions = true,
                CheckFileExists = true,
                Multiselect = true
            };
            fileDialog.ShowDialog();
            if (fileDialog.FileNames.Length <= 0) return;
            foreach (WorkFile wFile in from file in fileDialog.FileNames where File.Exists(file) select new WorkFile()
            {
                FileId = Guid.NewGuid(),
                Author = WorkEnviroment.ApplicationUserSession,
                FileInfo = new FileInfo(file),
                Catalog = this.Task,
            })
            {
                await
                    System.Threading.Tasks.Task.Run(
                        () => wFile.LoadToDb(ConfigHelper.DesktopSettings.ConnectionSettings.ToString()));
                if (Files == null)
                    Files = new ObservableCollection<WorkFile>();
                wFile.PropertyChanged += file_Comment_PropertyChanged;
                Files.Add(wFile);
                RaisePropertyChanged("Files");
            }

            }
            catch (Exception ex)
            {
                Logger.MessageBoxException(ex);
            }
        }

        public RelayCommand UpLoadAll
        {
            get { return _upLoadAll ?? (_upLoadAll = new RelayCommand(UpLoadAllfile)); }
        }

        private async void UpLoadAllfile(object obj)
        {
            using (var dialog = new FolderBrowserDialog {RootFolder = Environment.SpecialFolder.Desktop})
            {
                try
                {
                    if (DialogResult.OK != dialog.ShowDialog()) return;
                    foreach (var workFile in Files)
                    {
                        workFile.UnloadingOfFileInFolder(dialog.SelectedPath,
                            ConfigHelper.DesktopSettings.ConnectionSettings.ToString());
                    }
                    Process.Start(dialog.SelectedPath);
                }
                catch (Exception ex)
                {
                    Logger.MessageBoxException(ex);
                }
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
                    var newNot = new Notivication
                    {
                        NotivicationStatus = NotivicationStatus.Declared,
                        DateCreate = DateTime.Now,
                        TimeSend = date,
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
                    };
                    Task.Notivications.Add(newNot);
                    Users.First(f => f.MemberUser.IdUser == member.IdUser).AddUser(newNot);
                }
            }
            else
            {
                MessageBox.Show("Укажите через сколько дней будет отправлено уведомление", "Внимание",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public void SaveModel()
        {
            Task.DateUpdate = DateTime.Now;
            DbHelper.GetDbProvider.Tasks.AddOrUpdate(Task);
        }
        [Required(ErrorMessage = @"Обязательно заполните название задачи.")]
        public string NameTask
        {
            get { return Task.NameTask; }
            set
            {
                Task.NameTask = value;
                RaisePropertyChanged("NameTask");
            }
        }
        string _messageSend { get; set; }
     
        private ObservableCollection<WorkFile> _selectedFiles;
        private RelayCommand _deleteSelectedFiles;
        private DescriptionsTask _descriptionsTask;

        public ObservableCollection<WorkFile> SelectedFiles
        {
            get { return _selectedFiles ?? (_selectedFiles = new ObservableCollection<WorkFile>()); }
        }

        public ICommand DeleteSelectedFiles
        {
            get { return _deleteSelectedFiles ?? (_deleteSelectedFiles = new RelayCommand(RemoveSelectedFiles)); }
        }

        private void RemoveSelectedFiles(object obj)
        {
            try
            {
                if (MessageBox.Show("Вы действительно хотите удалить выделенные файлы из базы данных", "Удаление файлов",
                    MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.Yes) return;
                foreach (var file in SelectedFiles.ToArray())
                {
                    DbHelper.GetDbProvider.WorkFiles.Remove(file);
                    Files.Remove(file);
                    file.PropertyChanged -= file_Comment_PropertyChanged;                 
                }
                SaveTask(null);               
                SelectedFiles.Clear();
            }
            catch (Exception ex)
            {
                Logger.MessageBoxException(ex);
            }
        }
    }
}
