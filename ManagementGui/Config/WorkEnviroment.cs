using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using BaseType;
using BaseType.Common;
using ManagementGui.Utils;
using ManagementGui.ViewModel.Menu;

namespace ManagementGui.Config
{
    public static class WorkEnviroment
    {
        public static Visibility ManageSystemMI
        {
            get {return IsModerator ? Visibility.Visible : Visibility.Collapsed;}
        }
     //   public static FileLoadService FileLoadService { get; set; } 
        public static ApplicationUser ApplicationUserSession { get; set; }
        public static Project CurrentProject { get; set; }
        public static List<Project> UserProjects { get; set; }
        public static bool IsModerator { get; set; }
        public static List<SQLServerRoleUser> SqlServerRoles { get; set; }
        public static DateTime StartDateTime
        {
            get { return DateTime.Now.AddYears(-60); }
        }
        public static DateTime NowDateTime
        {
            get { return DateTime.Now; }
        }
        public static DateTime NotivicationStartDateTime
        {
            get { return DateTime.Now.AddMinutes(3); }
        }

        public static DateTime DefaultDate { get {return new DateTime(1990,10,2);}}

        static WorkEnviroment()
        {
            ApplicationUserSession = new ApplicationUser();
            SqlServerRoles = new List<SQLServerRoleUser>();
            UserProjects = new List<Project>();
            NewFiles=new NotifyObservableCollection<NewFile>();
          //  FileLoadService = new FileLoadService(NewFiles);
        }

        private  static bool _isSetSession;
        public static bool IsSetSetSession { get { return ApplicationUserSession!=null; }}

        internal static bool SetSession()
        {
            try
            {
                var sid = SqlServerRoles[0].SID;               
                ApplicationUserSession = DbHelper.GetDbProvider.Users.Single(w => w.SID == sid);
                UserProjects = DbHelper.GetDbProvider.Projects.Where(w=>w.Author.Id==ApplicationUserSession.Id).ToList();
                if (UserProjects != null && UserProjects.Count > 0)
                {
                    Guid id;
                    if (Guid.TryParse(DesktopSettings.Default.SessionSettings.LastProject,out id))
                        CurrentProject = UserProjects.FirstOrDefault(w => w.IdProject == id) ?? UserProjects[0];
                    else CurrentProject = UserProjects[0];
                }
                _isSetSession = true;
                return _isSetSession;
            }
            catch (Exception ex)
            {
                Logger.MessageBoxException(ex);
                _isSetSession = false;
                return _isSetSession;
            }
        }

        public static DateTime GetSendDefaultTime 
        {
            get { return DateTime.Now.AddDays(1); }
        }

        public static NotifyObservableCollection<NewFile> NewFiles { get; set; }
    }
}
