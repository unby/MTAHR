using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using BaseType.Common;
using System.Windows.Input;
using BaseType;
using BaseType.Report;
using GalaSoft.MvvmLight;
using ManagementGui.Config;
using ManagementGui.Utils;
using ManagementGui.View.Report;
using ManagementGui.ViewModel.Report;
using Xceed.Wpf.AvalonDock.Layout;

namespace ManagementGui.ViewModel
{
    public partial class MainViewModel : ViewModelBase
    {

        NotifyObservableCollection<ApplicationUser> _users;
        public NotifyObservableCollection<ApplicationUser> Users { get { return _users; }
            set
            {
                _users = value;
                this.RaisePropertyChanged();
            }
        }

        public MainViewModel()
        {
            if (WorkEnviroment.UserProjects != null&&WorkEnviroment.UserProjects.Count>0)
            {
                Update(null);
                SetListReport();
                SetInfo();
            }
        }

        private RelayCommand _update;
        private ObservableCollection<ReportItem> _listReport;

        public ICommand UpdateModel {
            get { return _update ?? (_update = new RelayCommand(Update)); }
        }

        public ObservableCollection<ReportItem> ListReport
        {
            get { return _listReport; }
            private set
            {
                _listReport = value;
                this.RaisePropertyChanged();
            }

        }

        private List<CurrentInfo> _currentInfoList;

        public List<CurrentInfo> CurrentInfoList
        {
            get
            {
                return _currentInfoList;
            }
            set
            {
                _currentInfoList = value;
                this.RaisePropertyChanged();
            }
        }

        public async void SetInfo()
        {
            CurrentInfoList=new List<CurrentInfo>();
            var infoCommon = await GetCommonInfoList();
            if (infoCommon != null)
                CurrentInfoList.Add(infoCommon);
            else
                return;
            
            for (int i = 0; i < 4; i++)
            {
                var item =await GetInfoListForMonth(i);
                if(item!=null)
                    CurrentInfoList.Add(item);
            }
        }

        private async Task<CurrentInfo> GetInfoListForMonth(int i)
        {
            var startDate=new DateTime(DateTime.Now.Year,DateTime.Now.Month-i,1);
            var endDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month - i+1, 1);
            var result = await
               (from task in DbHelper.GetDbProvider.Tasks
                where task.Project == WorkEnviroment.CurrentProject.IdProject
                        &&task.DateCreate>=startDate&&task.DateCreate<endDate
                group task by  task.IdTask 
                    into g
                    let all = g.DefaultIfEmpty()
                    let common = g.Count()
                    let open = g.Count(c => c.Status == StatusTask.Open)
                    let closed = g.Count(c => c.Status == StatusTask.Complete)
                    select new CurrentInfo
                    {
                        Closed = closed,
                        Opens = open,
                        Common = common,                    
                    }).FirstOrDefaultAsync();
            if(result!=null)
             result.Name = "За " + startDate.ToString("MMMM", CultureInfo.CurrentCulture) + " " + startDate.Year;
            return result;
        }

        private async Task<CurrentInfo> GetCommonInfoList()
        {
            var result = await 
                (from task in
                    DbHelper.GetDbProvider.Tasks
                 where task.Project == WorkEnviroment.CurrentProject.IdProject
                    group task by task.IdTask
                    into g
                    let all = g.DefaultIfEmpty()
                    let common = g.Count()
                    let open = g.Count(c => c.Status == StatusTask.Open)
                    let closed = g.Count(c => c.Status == StatusTask.Complete)
                    select new CurrentInfo
                    {
                        Closed = closed,
                        Opens = open,
                        Common = common,
                        Name = "За все время."
                    }).FirstOrDefaultAsync();
            return result;


        }

        public void SetListReport()
        {
            ListReport = new ObservableCollection<ReportItem>
            {
                new ReportItem {Description = "", Name = "Общий отчет по задачам", Type = typeof (ReportTasks)},
                new ReportItem {Description = "", Name = "Консолидированный отчет о поставленных задачах", Type = typeof (ObjectivesReport)},
                new ReportItem {Description = "", Name = "Отчет по задач", Type = typeof (WeightOfTasksReport)},
                new ReportItem {Description = "", Name = "Отчет по задачам пользователей", Type = typeof (UserTaskReport)},
                new ReportItem {Description = "", Name = "Консолидированный отчет по исполнителям", Type = typeof (ConsolidateReport)}
            };
        }

        public void Update(object obj)
        {
            try
            {
                if (WorkEnviroment.IsSetSetSession)
                {
                    if(WorkEnviroment.CurrentProject!=null)
                    Users =
                        new NotifyObservableCollection<ApplicationUser>(
                            (   from user in DbHelper.GetDbProvider.Users
                                join member in DbHelper.GetDbProvider.UserRoles on user.Id equals member.IdUser
                                where member.IdProject == WorkEnviroment.CurrentProject.IdProject
                                select user).ToList()
                            );
                }
            }
            catch (Exception ex)
            {       
                Logger.MessageBoxException(ex);
            }
        }
    }
}