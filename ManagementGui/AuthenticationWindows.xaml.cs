using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using BaseType;
using MahApps.Metro.Controls;
using ManagementGui.Config;
using ManagementGui.Utils;

namespace ManagementGui
{
    /// <summary>
    /// Логика взаимодействия для AuthenticationWindows.xaml
    /// </summary>
    public partial class AuthenticationWindows :MetroWindow
    {
        public bool IsModerator;
        private CancellationTokenSource _token;
        public AuthenticationWindows()
        {
            InitializeComponent();
          //  
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            _token = new CancellationTokenSource();
            try
            {
                ConfigHelper.DesktopSettings.ConnectionSettings.SetPasswordAndUser(NameTB.Text,PasswordPB.Password);
                DbHelper.Init(ConfigHelper.DesktopSettings.ConnectionSettings.ToString());
                string connectMessage = string.Format("Пользователь {0} (Client host name {1}) подключился к базе данных",
                ConfigHelper.DesktopSettings.ConnectionSettings.UserName,Dns.GetHostName());
                AppJurnal jurnal = new AppJurnal()
                {
                    DateEntry = DateTime.Now,
                    IdEntry = Guid.NewGuid(),
                    MessageCode = 1401,
                    Message = connectMessage,
                    MessageType = MessageType.App
                };
                List<SQLServerRoleUser> userDbRole = await DbHelper.Invoke.Database.SqlQuery<SQLServerRoleUser>(
                    string.Format("EXEC sp_helpuser '{0}';", NameTB.Text), new object[0]).ToListAsync(_token.Token);
                if (userDbRole != null)
                    IsModerator = userDbRole.Any(x => x.RoleName == "db_moderator");
                DbHelper.Invoke.AppJurnal.Add(jurnal);
                DbHelper.Invoke.SaveChanges();
                ConfigHelper.DesktopSettings.Save();

                DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка подключения к БД", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (_token != null)
                _token.Cancel();
            else
            {
                Application.Current.Shutdown();
            }
        }

        private void AuthenticationWindows1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            
        }
    }
}
