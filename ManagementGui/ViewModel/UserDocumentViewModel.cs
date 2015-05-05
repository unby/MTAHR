using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Globalization;
using System.Linq;
using System.Data.Entity;
using System.Linq.Dynamic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using ManagementGui.Config;
using BaseType;
using ManagementGui.Utils;
using ManagementGui.ViewModel.Validation;
using RelayCommand = BaseType.Common.RelayCommand;


namespace ManagementGui.ViewModel
{
    public class UserViewTasks
    {
        public Guid IdTask { get; set; }
        public string NameTask { get; set; }
        public DateTime LastUpdate { get; set; }

        public StatusTask Status { get; set; }

        public string LastComment { get; set; }
    }

    public class UserDocumentViewModel : ValidationViewModelBase
    {
        private readonly ApplicationDbContext _context = DbHelper.GetDbProvider;
        private ApplicationUser _user;
        private List<UserViewTasks> _userTask;
        public List<UserViewTasks> UserTasks {
            get { return _userTask; }
            set
            {
                _userTask = value;
                RaisePropertyChanged("UserTasks");
            }
        }
        public UserViewTasks SelectedTask {
            get { return _selectedTask; }
            set
            {
                _selectedTask = value;
                RaisePropertyChanged("SelectedTask");
            }
        }
        public ApplicationUser User {
            get { return _user; }
            set { _user = value; }
    }

        public UserDocumentViewModel()
        {
        }

        public UserDocumentViewModel(ApplicationUser user)
        {
           
            User = user;
            RoleList = new List<Role> {Role.Distribution, Role.User};
            if(WorkEnviroment.IsModerator)RoleList.Add(Role.Admin);
            if (user.BirthDate == null)
                _birthDate = WorkEnviroment.DefaultDate;
            else
                _birthDate=(DateTime)user.BirthDate;
           
        }

        public List<Role> RoleList { get; set; }

        public ICommand ShowTask
        {
            get
            {
                if (_showTask == null)
                    _showTask = new RelayCommand(UpdateListTask);
                return _showTask;
            }
        }

        private async void UpdateListTask(object obj)
        {
            var date = new DateTime(DateTime.Today.Year, DateTime.Today.Month - 1, 1);
            UserTasks = new List<UserViewTasks>((
              from task in DbHelper.GetDbProvider.Tasks.Where(x => x.Project == WorkEnviroment.CurrentProject.IdProject)
                join users in DbHelper.GetDbProvider.TaskMembers.Where(w => w.IdUser == User.Id)
                    on task.IdTask equals users.IdTask
              where task.DateUpdate > date
                
            select new UserViewTasks()
            {
                LastComment = task.TaskComments.Max(s => s.DateMessage).ToString(),
                NameTask = task.NameTask,
                Status = task.Status,
                LastUpdate = task.DateUpdate,
                IdTask = task.IdTask
            }));
        }
        RelayCommand<UserViewTasks> _mouseDoubleClickCommand;

        public ICommand MouseDoubleClickTasksGridCommand
        {
            get {
                return _mouseDoubleClickCommand ??
                       (_mouseDoubleClickCommand = new RelayCommand<UserViewTasks>(OpenTask));
            }
        }

        private void OpenTask(UserViewTasks obj)
        {
            if(obj!=null&&obj.IdTask!=Guid.Empty)
                MainWindow.DelegateWindowOpenTask(obj.IdTask);
        }

        public ICommand CreateTask
        {
            get { return _createTask ?? (_createTask = new RelayCommand(NewTask)); }
        }

        private void NewTask(object obj)
        {
            MainWindow.DelegateWindowCreateTask(User);           
        }

        private RelayCommand _save;
        private RelayCommand _createTask;

        public ICommand Save {
            get { return _save ?? (_save = new RelayCommand(SaveUser)); }
        }

        private void SaveUser(object obj)
        {
            try
            {
                if (string.IsNullOrEmpty(User.PasswordHash))
                    User.PasswordHash = Guid.NewGuid().ToString();
                _context.Users.AddOrUpdate(User);
                _context.SaveChanges();
                MainWindow.View.Users.Add(User);
            }
            catch (Exception ex)
            {
                Logger.MessageBoxException(ex);
            }
            if (User.Members != null) return;
            try
            {
                var member = new Member {Role = Role.User, User = User};
                // member.IdUser = User.Id;
                if (WorkEnviroment.CurrentProject != null)
                    member.Project = WorkEnviroment.CurrentProject;
                else
                {
                    if (WorkEnviroment.CurrentProject != null)
                    {
                        WorkEnviroment.CurrentProject.Members = new ObservableCollection<Member> {member};
                    }
                    _context.SaveChanges();
                }
                _context.UserRoles.AddOrUpdate(member);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Logger.MessageBoxException(ex);
            }
        }

    
        #region Accesors
        
        [Required(ErrorMessage = @"Обязательно заполните фамилию сотрудника.")]
        public string Surname
        {
            get { return User.Surname; }
            set
            {
                User.Surname = value;
                this.RaisePropertyChanged("Surname");
            }
        }
        [Required(ErrorMessage = @"Обязательно заполните отчество сотрудника.")]
        public string MiddleName
        {
            get { return User.MiddleName; }
            set
            {
                User.MiddleName = value;
                RaisePropertyChanged("MiddleName");
            }

        }
        [Required(ErrorMessage = @"Обязательно заполните имя сотрудника.")]
        public string Name
        {
            get { return User.Name; }
            set
            {
                User.Name = value;
                RaisePropertyChanged("Name");
            }

        }

        [Required(ErrorMessage = @"Обязательно заполните почтовый адрес.")]
        public string Email
        {
            get { return User.Email; }
            set
            {
                User.Email = value;
                RaisePropertyChanged("Email");
            }
        }

        private DateTime _birthDate;
        private UserViewTasks _selectedTask;

        public DateTime BirthDate
        {
            get { return _birthDate; }
            set
            {
                _birthDate = value;
                User.BirthDate = _birthDate;
                RaisePropertyChanged("BirthDate");
            }
        }

        #endregion

        public RelayCommand _showTask { get; set; }
    }
}
