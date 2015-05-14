using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using BaseType;
using BaseType.Migrations;
using MahApps.Metro.Controls;
using ManagementGui.Config;
using ManagementGui.Utils;

namespace ManagementGui.View
{
    /// <summary>
    /// Логика взаимодействия для AuthenticationWindows.xaml
    /// </summary>
    public partial class AuthenticationWindows :MetroWindow
    {
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
                List<SQLServerRoleUser> userDbRole = await DbHelper.GetDbProvider.Database.SqlQuery<SQLServerRoleUser>(
                    string.Format("EXEC sp_helpuser '{0}';", NameTB.Text), new object[0]).ToListAsync(_token.Token);
                if (userDbRole != null)
                {
                    WorkEnviroment.IsModerator = userDbRole.Any(x => x.RoleName == "db_moderator");
                    WorkEnviroment.SqlServerRoles = userDbRole;
                    WorkEnviroment.SetSession();
                }
                DbHelper.GetDbProvider.AppJurnal.Add(jurnal);
                DbHelper.GetDbProvider.SaveChanges();
                ConfigHelper.DesktopSettings.SaveConnectionString();

                DialogResult = true;
            }
            catch (Exception ex)
            {
               // MessageBox.Show(ex.Message, "Ошибка подключения к БД", MessageBoxButton.OK, MessageBoxImage.Error);
                Logger.MessageBoxException(ex);
                PasswordPB.Password = string.Empty;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (_token != null)
                _token.Cancel();
            DialogResult = false;
        }


        private void PasswordPB_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) Button_Click(null, null);
        }

        private void AuthenticationWindows_OnLoaded(object sender, RoutedEventArgs e)
        {
            Database.SetInitializer<ApplicationDbContext>(new MigrateDatabaseToLatestVersion<ApplicationDbContext, Configuration>());
        }
    }
}
