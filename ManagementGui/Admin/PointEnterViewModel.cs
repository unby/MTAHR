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

        public ObservableCollection<PointEnter> PointEnters
        {
            get { return _pointEnters; }
            set { _pointEnters = value;RaisePropertyChanged(); }
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
                string.Format("select logins.ProviderKey, logins.LoginProvider, users.UserName ,users.Id " +
                              "from AspNetUserLogins as logins join  AspNetUsers as users on users.Id=logins.UserId " +
                              "where logins.LoginProvider='windows_nt_logon' or logins.LoginProvider='sqlserver_logon' " +
                              "union select convert(nvarchar(200),db.sid,2) as ProviderKey,CAST(" +
                              "CASE WHEN serverdb.isntuser = 1 THEN 'windows_nt_logon' ELSE 'sqlserver_logon' " +
                              "END AS nvarchar) as LoginProvider , serverdb.loginname as UserName,null as id " +
                              "from dbo.sysusers as db,master.dbo.syslogins as serverdb ,AspNetUserLogins as applogins " +
                              "where db.sid=serverdb.sid and serverdb.denylogin=0 and applogins.ProviderKey " +
                              "!= convert(nvarchar(200),db.sid,2) order by id")).ToListAsync();
            PointEnters = new ObservableCollection<PointEnter>(temp);
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
        private ObservableCollection<PointEnter> _pointEnters;
        private ObservableCollection<ApplicationUserLogin> _userLogins;

        public ICommand AcceptCommand
        {
            get { return _accept ?? (_accept = new RelayCommand(Accept)); }
        }

        public ICommand ClearLogins
        {
            get { return _clearLogins ?? (_clearLogins = new RelayCommand(LoginClear)); }
        }

        private async void LoginClear()
        {
            try
            {
                if (Current == null) return;
                DbHelper.GetDbProvider.Logins.Remove(Current);
                await DbHelper.GetDbProvider.SaveChangesAsync();
                UserLogins.Remove(Current);
                User.Logins.Remove(Current);
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

        [BaseType.Utils.NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}