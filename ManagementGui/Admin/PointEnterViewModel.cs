using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BaseType;
using BaseType.Utils;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using JetBrains.Annotations;
using ManagementGui.Utils;

namespace ManagementGui.Admin
{
    public class PointEnterViewModel : ViewModelBase
    {
        public PointEnter CurrentPointEnter
        {
            get { return _currentPointEnter; }
            set
            {
                _currentPointEnter = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<PointEnter> FreePointEnters
        {
            get { return _freePointEnters; }
            set { _freePointEnters = value;RaisePropertyChanged(); }
        }

        public ApplicationUserLogin Current
        {
            get { return _current; }
            set
            {
                _current = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<ApplicationUserLogin> UserLogins
        {
            get { return _userLogins; }
            set { _userLogins = value;RaisePropertyChanged(); }
        }

        private readonly ApplicationDbContext _dataContextData = DbHelper.GetDbProvider;
        public ApplicationUser User { get; set; }
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

        public PointEnterViewModel()
        {
        }

        public PointEnterViewModel(ApplicationUser applicationUser)
        {
            User = applicationUser;
            UserLogins=new ObservableCollection<ApplicationUserLogin>(User.Logins);
            UpdateUserAndPointEnters();
        }

        private async void UpdateUserAndPointEnters()
        {
            try
            {
                var temp = await _dataContextData.Database.SqlQuery<PointEnter>(
                    string.Format("select convert(nvarchar(200),db.sid,2) as ProviderKey,CAST( "
                                 +"CASE WHEN serverdb.isntuser = 1 THEN 'windows_nt_logon' ELSE 'sqlserver_logon' "
                                 +"END AS nvarchar) as LoginProvider , serverdb.loginname as UserName,null as id "
                                 +"from dbo.sysusers as db,master.dbo.syslogins as serverdb "
                                 +"where db.sid=serverdb.sid and serverdb.denylogin=0 and "
                                 +"convert(nvarchar(200),db.sid,2) not in (" +
                                 "select ProviderKey from AspNetUserLogins where LoginProvider!='windows_nt_logon' " +
                                 "or LoginProvider!= 'sqlserver_logon') ")).ToListAsync();
                FreePointEnters = new ObservableCollection<PointEnter>(temp);

                var tempAll = await (from user in _dataContextData.Users
                    join pointEnter in _dataContextData.Logins on user.Id equals pointEnter.UserId
                    select new PointEnter()
                    {
                        Email = user.Email,
                        UserName = user.Surname + " " + user.Name + " " + user.MiddleName,
                        Id = user.Id,
                        LoginProvider = pointEnter.LoginProvider,
                        ProviderKey = pointEnter.ProviderKey
                    }).ToListAsync();


                AllEnterPoint = new List<PointEnter>(tempAll);
            }
            catch (Exception ex)
            {
                Logger.MessageBoxException(ex);
            }
        }

        public ICommand CancelCommand
        {
            get { return new RelayCommand(() => IsDialogClose = false); }
        }

        private RelayCommand _cleart;

        public ICommand ClearCommand
        {
            get { return _cleart ?? (_cleart = new RelayCommand(Clear)); }
        }

        private void Clear()
        {
           
            IsDialogClose = true;
        }

        private RelayCommand _accept;
        private PointEnter _currentPointEnter;
        private ApplicationUserLogin _current;
        private RelayCommand _clearLogins;
        private RelayCommand _addLogins;
        private ObservableCollection<PointEnter> _freePointEnters;
        private ObservableCollection<ApplicationUserLogin> _userLogins;
        private List<PointEnter> _allEnterPoint;
        private RelayCommand _refreshList;
        private ApplicationUserLogin _selectedUserLogin;

        public ICommand AcceptCommand
        {
            get { return _accept ?? (_accept = new RelayCommand(Accept)); }
        }

        public ApplicationUserLogin SelectedUserLogin
        {
            get { return _selectedUserLogin; }
            set { _selectedUserLogin = value; RaisePropertyChanged();}
        }

        public ICommand ClearLogins
        {
            get { return _clearLogins ?? (_clearLogins = new RelayCommand(LoginClear)); }
        }

        private async void LoginClear()
        {
            try
            {
                if (SelectedUserLogin == null) return;
                DbHelper.GetDbProvider.Logins.Remove(SelectedUserLogin);
                await DbHelper.GetDbProvider.SaveChangesAsync();
                UserLogins.Remove(SelectedUserLogin);
                User.Logins.Remove(SelectedUserLogin);
            }
            catch (Exception ex)
            {
                Logger.MessageBoxException(ex);
            }
        }

        public ICommand AddLogins
        {
            get { return _addLogins ?? (_addLogins = new RelayCommand(LoginAdd)); }
        }

        public string UserTitle
        {
            get { return string.Format("Точка входа: {0}", User.UserFullName()); }
        }

        public List<PointEnter> AllEnterPoint
        {
            get { return _allEnterPoint; }
            set
            {
                _allEnterPoint = value;
                RaisePropertyChanged();
            }
        }

        public ICommand RefreshList
        {
            get { return _refreshList ?? (_refreshList = new RelayCommand(UpdateUserAndPointEnters)); }
        }

        private async void LoginAdd()
        {
            try
            {
                if (CurrentPointEnter == null) return;
                if (CurrentPointEnter.Id == null)
                {
                    Current = new ApplicationUserLogin()
                    {
                        LoginProvider = CurrentPointEnter.LoginProvider,
                        ProviderKey = CurrentPointEnter.ProviderKey,
                        UserId = User.Id
                    };
                    DbHelper.GetDbProvider.Logins.Add(Current);
                    await DbHelper.GetDbProvider.SaveChangesAsync();
                    UserLogins.Add(Current);
                    User.Logins.Add(Current);
                }
                else
                {
                    var temp = DbHelper.GetDbProvider.Users.FirstOrDefault(f => f.Id == CurrentPointEnter.Id);
                    if (temp == null)
                    {
                        DbHelper.GetDbProvider.Logins.RemoveRange(await
                            DbHelper.GetDbProvider.Logins.Where(w => w.UserId == CurrentPointEnter.Id).ToListAsync());
                        Current = new ApplicationUserLogin()
                        {
                            LoginProvider = CurrentPointEnter.LoginProvider,
                            ProviderKey = CurrentPointEnter.ProviderKey,
                            UserId = User.Id
                        };
                        DbHelper.GetDbProvider.Logins.Add(Current);
                        await DbHelper.GetDbProvider.SaveChangesAsync();
                        UserLogins.Add(Current);
                        User.Logins.Add(Current);

                    }
                    else
                    {
                        if (MessageBox.Show(string.Format("Точка входа {0} занята пользователем {1}, выполнить смену",
                                CurrentPointEnter.ProviderKey,
                                temp.UserName), "Смена точек входа БД", MessageBoxButton.YesNo,
                            MessageBoxImage.Information) != MessageBoxResult.Yes) return;

                        DbHelper.GetDbProvider.Logins.Remove(await
                            DbHelper.GetDbProvider.Logins.FirstAsync(w => w.ProviderKey == CurrentPointEnter.ProviderKey));
                        Current = new ApplicationUserLogin()
                        {
                            LoginProvider = CurrentPointEnter.LoginProvider,
                            ProviderKey = CurrentPointEnter.ProviderKey,
                            UserId = User.Id
                        };
                        DbHelper.GetDbProvider.Logins.Add(Current);
                        await DbHelper.GetDbProvider.SaveChangesAsync();
                        UserLogins.Add(Current);
                        User.Logins.Add(Current);
                    }
                }
            }

            catch (Exception ex)
            {
                Logger.MessageBoxException(ex);
            }
        }

        private void Accept()
        {
            IsDialogClose = true;
        }
    }

    public class PointEnter : INotifyPropertyChanged
    {
        private string _providerKey;
        private Guid? _id;
        private string _loginProvider;
        private string _userName;
        private Uri _image;

        public Guid? Id
        {
            get { return _id; }
            set
            {
                _id = value; 
                OnPropertyChanged();
              
            }
        }
        public string Email { get; set; }
        public string ProviderKey
        {
            get { return _providerKey; }
            set
            {
                _providerKey = value; 
                OnPropertyChanged();
            }
        }

        public string LoginProvider
        {
            get { return _loginProvider; }
            set
            {
                _loginProvider = value;
                OnPropertyChanged();
            }
        }

        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                OnPropertyChanged();
            }
        }

        public bool IsFree
        {
            get
            {            
                return Id==null;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}