﻿using System;
using System.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;
using ManagementGui.Config;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.TeamFoundation.MVVM;

namespace ManagementGui.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>

    public class AuthenticationWindowsViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the AuthenticationWindowsViewModel class.
        /// </summary>

        public string Login
        {
            get
            {
                if (ConfigHelper.DesktopSettings.ConnectionSettings.IntegratedSecurity)
                    return ConfigHelper.DesktopSettings.ConnectionSettings.UserName;
                else return string.Format(@"{0}\{1}", Environment.UserDomainName, Environment.UserName); 
            }

            set
            {
                RaisePropertyChanged("Login");
                if (ConfigHelper.DesktopSettings.ConnectionSettings.IntegratedSecurity)
                    ConfigHelper.DesktopSettings.ConnectionSettings.UserName = value;
            }
        }

        public string ServerName
        {
            get { return ConfigHelper.DesktopSettings.ConnectionSettings.ServerName; }
            set
            {
                RaisePropertyChanged("ServerName");
                ConfigHelper.DesktopSettings.ConnectionSettings.ServerName = value;
            }
        }

        private bool _loginIs;

        public bool LoginIs
        {
            get { return _loginIs; }
            set
            {
                //Set(@"LoginIs", ref _loginIs, true);
                _loginIs = value;
            }
        }

        public AuthenticationUser Authentication
        {
            get
            {
                if (ConfigHelper.DesktopSettings.ConnectionSettings.IntegratedSecurity)
                {

                    RaisePropertyChanged("LoginIs");
                    LoginIs = true;
                    return AuthenticationUser.Integraten;
                }
                RaisePropertyChanged("LoginIs");
                LoginIs = false;
                return AuthenticationUser.Windows;
            }
            set
            {
                if (AuthenticationUser.Windows == value)
                {
                    ConfigHelper.DesktopSettings.ConnectionSettings.IntegratedSecurity = LoginIs = false;
                    Login =string.Format(@"{0}\{1}",Environment.UserDomainName,Environment.UserName);
                }
                else
                {
                    ConfigHelper.DesktopSettings.ConnectionSettings.IntegratedSecurity = LoginIs = true;
                    Login = ConfigHelper.DesktopSettings.ConnectionSettings.UserName;
                }
            }
        }

        private string _password;

        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        public AuthenticationWindowsViewModel()
        {
            string server = ConfigHelper.DesktopSettings.ConnectionSettings.ServerName;
        }
    }
}