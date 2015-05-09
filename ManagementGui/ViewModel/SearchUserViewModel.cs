using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using BaseType;
using BaseType.Common;
using BaseType.Utils;
using GalaSoft.MvvmLight;
using ManagementGui.Admin;
using ManagementGui.Config;
using ManagementGui.Utils;

namespace ManagementGui.ViewModel
{
    public class SearchUserViewModel : ViewModelBase
    {
        public SearchUserViewModel()
        {
            Users = new ObservableCollection<ApplicationUser>();
            SelectedUsers = new ObservableCollection<ApplicationUser>();    
        }
        public SearchUserViewModel(bool isAdminMode)
        {
            Users = new ObservableCollection<ApplicationUser>();
            SelectedUsers = new ObservableCollection<ApplicationUser>();
        }
        #region PropertyBinding
        #region PropertyFiltrBinding

        private Visibility _isAdminModeVisibility;

        public Visibility IsAdminModeVisibility
        {
            get { return _isAdminModeVisibility; }
            set { _isAdminModeVisibility = value; RaisePropertyChanged();}
        }
        public string FioField { get; set; }
        private bool _isWork;
        public bool IsWork
        {
            get { return _isWork; }
            set
            {
                _isWork = value;
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
        private ObservableCollection<ApplicationUser> _users;
        private ObservableCollection<ApplicationUser> _selectedUsers;

        public ObservableCollection<ApplicationUser> SelectedUsers
        {
            get { return _selectedUsers; }
            set
            {
                _selectedUsers = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<ApplicationUser> Users
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
        private ApplicationUser _currentUser;

        public ICommand Search {
            get { return _seacrh ?? (_seacrh = new RelayCommand(SearchUserInBase)); }
        }

        private async void SearchUserInBase(object obj)
        {         
            var query = from user in DbHelper.GetDbProvider.Users select user;
            if (!string.IsNullOrEmpty(SureNameFind))
                query = query.Where(c => c.Surname.Contains(SureNameFind));
            if (!string.IsNullOrEmpty(NameFind))
                query = query.Where(c => c.Name.Contains(NameFind));
            if (!string.IsNullOrEmpty(EMailFind))
                query = query.Where(c => c.Email.Contains(EMailFind));
            if (IsWork)
                query = query.Where(w => w.IsWork);
            var temp = await query.ToListAsync();
            if(IsAllBase)
                Users = new ObservableCollection<ApplicationUser>(temp);
            else
            {
                Users = new ObservableCollection<ApplicationUser>();
                foreach (var user in temp.Where(user => WorkEnviroment.CurrentProject.Members.Any(a=>a.IdUser==user.Id)))
                {
                    Users.Add(user);
                }
            }
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
        #region SearchFild
        private string _sureNameFind;
        public string SureNameFind
        {
            get { return _sureNameFind; }
            set
            {
                _sureNameFind = value;
                RaisePropertyChanged();
            }
        }

        private string _nameFind;
        public string NameFind
        {
            get { return _nameFind; }
            set
            {
                _nameFind = value;
                RaisePropertyChanged();
            }
        }

        private string _eMailFind;
        public string EMailFind
        {
            get { return _eMailFind; }
            set
            {
                _eMailFind = value;
                RaisePropertyChanged();
            }
        }

        private bool _isAllBase;
        public bool IsAllBase
        {
            get { return _isAllBase; }
            set
            {
                _isAllBase = value;
                RaisePropertyChanged();
            }
        }

        private bool _isAdminMode;
        private RelayCommand _resetPassword;
        private RelayCommand _setLoginDb;
        private RelayCommand _saveUser;
        private RelayCommand _createUser;

        public bool IsAdminMode
        {
            get { return _isAdminMode; }
            set
            {
                _isAdminMode = value;
                RaisePropertyChanged();
            }
        }
        #endregion
        public ApplicationUser CurrentUser
        {
            get { return _currentUser; }
            set
            {
                _currentUser = value; 
                RaisePropertyChanged();
            }
        }

        public ICommand ResetPassword
        {
            get { return _resetPassword ?? (_resetPassword = new RelayCommand(PasswordReset)); }
        }

        private async void PasswordReset(object obj)
        {
            try
            {
                if (string.IsNullOrEmpty(CurrentUser.PasswordHash))
                {
                    var manager = new ApplicationUserManager(new ApplicationUserStore(DbHelper.GetDbProvider));
                    await manager.ResetPasswordAsync(CurrentUser.Id, "", "1qazXSW@");
                }
            }
            catch (Exception ex)
            {
                Logger.MessageBoxException(ex);
            }
        }

        public ICommand SetLoginDb
        {
            get { return _setLoginDb ?? (_setLoginDb = new RelayCommand(LoginDbSet)); }
        }

        public ICommand CreateUser
        {
            get { return _createUser ?? (_createUser = new RelayCommand(UserCreate)); }
        }

        private void UserCreate(object obj)
        {
            CurrentUser=new ApplicationUser()
            {
                Id=Guid.NewGuid(),
                BirthDate = WorkEnviroment.DefaultDate,
                IsWork = true
            };
        }

        public ICommand SaveUser
        {
            get { return _saveUser ?? (_saveUser = new RelayCommand(UserSave)); ; }
        }

        private async void UserSave(object obj)
        {
            bool isSave = false;
            try
            {
                if (string.IsNullOrEmpty(CurrentUser.PasswordHash))
                {
                    var manager = new ApplicationUserManager(new ApplicationUserStore(DbHelper.GetDbProvider));
                    await manager.CreateAsync(CurrentUser, CurrentUser.Id.ToByteArray().GetMd5());
                    isSave = true;
                }
                else
                {
                    DbHelper.GetDbProvider.Users.AddOrUpdate(CurrentUser);
                    await DbHelper.GetDbProvider.SaveChangesAsync();
                }
                
                
            }
            catch (Exception ex)
            {
                Logger.MessageBoxException(ex);
            }
            finally
            {
                if (isSave)
                    if(Users!=null)
                    Users.Add(CurrentUser);
            }
        }

        private void LoginDbSet(object obj)
        {
            var windows=new PointEnterWindows(CurrentUser);
            windows.ShowDialog();
        }

        private void ExitWindow(object obj)
        {
            IsDialogClose = false;
        }

        #endregion
    }
}
