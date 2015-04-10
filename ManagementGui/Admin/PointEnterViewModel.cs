using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using BaseType;
using BaseType.Security;
using BaseType.Utils;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ManagementGui.Utils;

namespace ManagementGui.Admin
{
    public class PointEnterViewModel : ViewModelBase{
        public List<UserAndPointEnter> UserAndPointEnters { get; set; }
        private UserAndPointEnter _selectedPointEnter;

        public UserAndPointEnter SelectedPointEnter
        {
            get { return _selectedPointEnter; }
            set
            {
                _selectedPointEnter = value;
                this.RaisePropertyChanged();
            }
        }

        private readonly ApplicationDbContext _dataContextData = DbHelper.GetDbProvider;
        public ApplicationUser ApplicationUserCurrent { get; set; }
        private bool? _isDialogClose;
        public bool? IsDialogClose {
            get { return _isDialogClose; }
            set
            {
                _isDialogClose = value;
                RaisePropertyChanged();
            }
        }

        public PointEnterViewModel()
        {
           UpdateUserAndPointEnters();
        }
        private void UpdateUserAndPointEnters()
        {
            UserAndPointEnters =
                _dataContextData.Users.Where(w => w.LoginName != null && w.SID != null)
                    .Select(s => new UserAndPointEnter {SID = s.SID,LoginName = s.LoginName,UserName = s.Name,UserMidleName = s.MiddleName,UserSurname = s.Surname,UserId = s.Id})
                    .ToList();
           
            var temp =  _dataContextData.Database.SqlQuery<FreePointEnter>(
                    string.Format(
                        @"select db.sid, serverdb.isntuser, db.issqluser, serverdb.loginname " +
                          @"from dbo.sysusers as db,master.dbo.syslogins as serverdb " +
                            @"where db.sid=serverdb.sid and serverdb.denylogin=0"));
            foreach (FreePointEnter tPointEnter in temp)
            {
                if (!UserAndPointEnters.Any(x => x.SID.ConvertByteToStringSid().Equals(tPointEnter.sid.ConvertByteToStringSid())))
                    UserAndPointEnters.Add(new UserAndPointEnter()
                    {
                        LoginName = tPointEnter.loginname,
                        UserName = "",
                        UserId = Guid.Empty,
                        SID = tPointEnter.sid,
                        AuthorizationType = tPointEnter.issqluser == 1 ? Authorization.MsSqlServer : Authorization.WindowsAd
                    });
            }
        }

        
        #region CRUD
        #endregion

        public ICommand CancelCommand
        {
        get { return new RelayCommand(() => IsDialogClose = false); }
        }

        private RelayCommand _cleart;

        public ICommand ClearCommand
        {
            get
            {
                if (_cleart == null)
                    _cleart = new RelayCommand(Clear);
                return _cleart;
            }
        }

        private void Clear()
        {
            if (SelectedPointEnter != null && !string.IsNullOrEmpty(SelectedPointEnter.LoginName) &&
                !string.IsNullOrEmpty(SelectedPointEnter.Sid))
            {
                SelectedPointEnter.LoginName = null;
                SelectedPointEnter.SID = null;
            }
            IsDialogClose = true;
        }

        private RelayCommand _accept;

        public ICommand AcceptCommand
        {
            get
            {
                if (_accept == null)
                    _accept = new RelayCommand(Accept);
                return _accept;
            }      
        }

        private void Accept()
        {
            IsDialogClose = true;         
        }
    }

    public class UserAndPointEnter
    {
        public override string ToString()
        {
            return GetName+" " +LoginName;
        }

        public string GetName { get { return string.Format("{0} {1} {2}", UserSurname, UserName, UserMidleName); } }
        public string UserSurname { get; set; }
        public string UserMidleName { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        [Browsable(false)]
        public byte[] SID { get; set; }
        public string LoginName { get; set; }
        public Authorization AuthorizationType { get; set; }
        public string Sid
        {
            get { return SID.ConvertByteToStringSid(); }
        }

    }

    public struct FreePointEnter
    {
        [Browsable(false)]
        public byte[] sid { get; set; }
        public string loginname { get; set; }
        public int issqluser { get; set; }
        public int isntuser { get; set; }

        public override string ToString()
        {
            return string.Format("{0} ({1})", loginname, issqluser == 1 ? "Учетная запись SQL Server" : "Учетная запись Windows NT");
        }
    }
}
/*  UserAndPointEnters = new List<UserAndPointEnter>(from pEnter in _dataContextData.PointEnters
                                                              join User in _dataContextData.Users on pEnter equals User.PointEnterUser into gj
                                                              from obj in gj.DefaultIfEmpty()
                                                              where (obj.PointEnterUser.AuthorizationType & (Authorization.WindowsAd | Authorization.MsSqlServer)) != 0
                                                              select new UserAndPointEnter
                                                              {
                                                                  LoginName = obj.PointEnterUser.LoginName,
                                                                  UserName = obj.Name,
                                                                  UserSurname = obj.Surname,
                                                                  UserMidleName = obj.MiddleName,
                                                                  Id = obj.Id,
                                                                  SID = obj.PointEnterUser.SID,
                                                                  AuthorizationType = obj.PointEnterUser.AuthorizationType
                                                              });*/