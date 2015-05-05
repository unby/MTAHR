using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using BaseType;
using BaseType.Common;
using GalaSoft.MvvmLight;
using ManagementGui.Config;
using ManagementGui.Utils;

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
        public string FioField { get; set; }
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
                RaisePropertyChanged();
            }
        }

        public bool IsProject
        {
            get { return _isProject; }
            set
            {
                _isProject = value;               
                RaisePropertyChanged();
            }
        }
        public string IsWorkText
        {
            get { return _isWorkText; }
            set
            {
                _isWorkText = value;
                RaisePropertyChanged();
            }
        }
        public string IsProjectText
        {
            get { return _isProjectText; }
            set
            {
                _isProjectText = value;
                RaisePropertyChanged();
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
                RaisePropertyChanged();
            }
        }

        public List<ApplicationUser> Users
        {
            get { return _users; }
            set
            {
                _users = value;
                RaisePropertyChanged();
            }
        }
        #endregion
        #endregion

        #region CommandBinding

        private RelayCommand _seacrh;
        private RelayCommand _select;
        private RelayCommand _exit;
        public ICommand Search {
            get { return _seacrh ?? (_seacrh = new RelayCommand(SearchUserInBase)); }
        }

        private async void SearchUserInBase(object obj)
        {
            if (string.IsNullOrEmpty(FioField)) return;
            var query = string.Format(@"SELECT *  FROM [AspNetUsers] as userapp where {1} {0} {2}", GetBool(), GetFio(), GetProject());
            Users =
                new List<ApplicationUser>(
                    await
                        DbHelper.GetDbProvider.Database.SqlQuery<ApplicationUser>(query
                            )
                            .ToListAsync());
        }

        private string GetProject()
        {
            var result = "";
            if (IsProject)
                result =
                    string.Format(@"and userapp.id in (select id from (select IdProject from [Projects] where IdProject='{0}') as projectApp 
join [members] as memberApp on memberApp.IdProject=projectApp.IdProject)", WorkEnviroment.CurrentProject.IdProject);
            return result;
        }

        private string GetFio()
        {
            var result = "";
            var names = FioField.Split(new[] {'.',' ', ',', ';', '!', '@', '"', '\'', '+', '(', ')', '^', ':'}).Where(w=>w.Length>2);
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
            var result = "";
            if (IsWork)
                result += "and iswork='true'";
            else
                result += "";
            return result;
        }

        public ICommand Select
        {
            get { return _select ?? (_select = new RelayCommand(SelectUsers)); }
        }

        private void SelectUsers(object obj)
        {
            IsDialogClose = true;
        }

        public ICommand Exit
        {
            get { return _exit ?? (_exit = new RelayCommand(ExitWindow)); }
        }

        private void ExitWindow(object obj)
        {
            IsDialogClose = false;
        }

        #endregion
    }
}
