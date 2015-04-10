using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using BaseType;
using BaseType.Common;
using GalaSoft.MvvmLight;
using ManagementGui.Config;
using ManagementGui.Utils;
using Xceed.Wpf.Data;

namespace ManagementGui.ViewModel
{
    public class SearchUserViewModel : ViewModelBase
    {
        public SearchUserViewModel()
        {
            Users = new List<ApplicationUser>((from user in DbHelper.GetDbProvider.Users.Where(w => w.IsWork)
                join project in DbHelper.GetDbProvider.UserRoles on user.Id equals project.IdUser
                where project.IdProject==WorkEnviroment.CurrentProject.IdProject
                select user));
            IsWork = true;
            IsProject = true;
            SelectedUsers=new List<ApplicationUser>();
            IsVisibleProjectUser = Visibility.Visible;
        }
        public SearchUserViewModel(Visibility isVisibleProjectUser)
        {
            Users = new List<ApplicationUser>((from user in DbHelper.GetDbProvider.Users.Where(w => w.IsWork)
                join project in DbHelper.GetDbProvider.UserRoles on user.Id equals project.IdUser
                where project.IdProject==WorkEnviroment.CurrentProject.IdProject
                select user));
            IsWork = true;
            IsProject = false;
            SelectedUsers=new List<ApplicationUser>();
             IsVisibleProjectUser = isVisibleProjectUser;
        }
        #region PropertyBinding
        #region PropertyFiltrBinding

        public  Visibility IsVisibleProjectUser { get; private set; }
        public string FIOField { get; set; }
        private bool _isWork;
        private string _isProjectText;
        private string _isWorkText;
        private bool _isProject;

        public bool IsWork
        {
            get { return _isWork; }
            set
            {
                _isWork = value;
                if (_isWork)
                {
                    IsWorkText = "Работает";
                }
                else
                {
                    IsWorkText = "Не работает";
                }
                this.RaisePropertyChanged();
            }
        }

        public bool IsProject
        {
            get { return _isProject; }
            set
            {
                _isProject = value;
                _isWork = value;
                if (_isProject)
                {
                    IsProjectText = "В базе";
                }
                else
                {
                    IsProjectText = "В проекте";
                }
                this.RaisePropertyChanged();
            }
        }
        public string IsWorkText
        {
            get { return _isWorkText; }
            set
            {
                _isWorkText = value;
                this.RaisePropertyChanged();
            }
        }
        public string IsProjectText
        {
            get { return _isProjectText; }
            set
            {
                _isProjectText = value;
                this.RaisePropertyChanged();
            }
        }
        private bool? _isDialogClose;
        public bool? IsDialogClose
        {
            get { return _isDialogClose; }
            set
            {
                _isDialogClose = value;
                RaisePropertyChanged();
            }
        }
        #endregion
        #region ModelBinding
        private List<ApplicationUser> _users;
        private List<ApplicationUser> _selectedUsers;

        public List<ApplicationUser> SelectedUsers
        {
            get { return _selectedUsers; }
            set
            {
                _selectedUsers = value;
                this.RaisePropertyChanged();
            }
        }

        public List<ApplicationUser> Users
        {
            get { return _users; }
            set
            {
                _users = value;
                this.RaisePropertyChanged();
            }
        }
        #endregion
        #endregion

        #region CommandBinding

        private RelayCommand _seacrh;
        private RelayCommand _select;
        private RelayCommand _exit;
        public ICommand Search {
            get
            {
                if(_seacrh==null)
                    _seacrh=new RelayCommand(SearchUserInBase);
                return _seacrh;
            }
        }

        private async void SearchUserInBase(object obj)
        {
            if (!string.IsNullOrEmpty(FIOField))
            {
                string query = string.Format(@"SELECT *  FROM [AspNetUsers] as userapp where {1} {0} {2}", GetBool(), GetFio(), GetProject());
                Users =
                    new List<ApplicationUser>(
                        await
                            DbHelper.GetDbProvider.Database.SqlQuery<ApplicationUser>(query
                                )
                                .ToListAsync());
            }

        }

        private string GetProject()
        {
            string result = "";
            if (IsProject)
                result =
                    string.Format(@"and userapp.id in (select id from (select IdProject from [Projects] where IdProject='{0}') as projectApp 
join [members] as memberApp on memberApp.IdProject=projectApp.IdProject)", WorkEnviroment.CurrentProject.IdProject);
            return result;
        }

        private string GetFio()
        {
            string result = "";
            var names = FIOField.Split(new[] {'.',' ', ',', ';', '!', '@', '"', '\'', '+', '(', ')', '^', ':'}).Where(w=>w.Length>2);
            result += "(";
            var enumerable = names as string[] ?? names.ToArray();
            result = enumerable.Aggregate(result, (current, name) => current + string.Format(" Name='{0}' or", name));
            result = enumerable.Aggregate(result, (current, name) => current + string.Format(" MiddleName='{0}' or", name));
            result = enumerable.Aggregate(result, (current, name) => current + string.Format(" SurName='{0}' or", name));
           result= result.RemoveEndString("or");
            result += ")";
            return result;
        }

        private string GetBool()
        {
            string result = "";
            if (IsWork)
                result += "and iswork='true'";
            else
                result += "";
            return result;
        }

        public ICommand Select
        {
            get
            {
                if (_select == null)
                    _select = new RelayCommand(SelectUsers);
                return _select;
            }
        }

        private void SelectUsers(object obj)
        {
            IsDialogClose = true;
        }

        public ICommand Exit
        {
            get
            {
                if (_exit == null)
                    _exit = new RelayCommand(ExitWindow);
                return _exit;
            }
        }

        private void ExitWindow(object obj)
        {
            IsDialogClose = false;
        }

        #endregion
    }
}
