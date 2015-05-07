using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BaseType;
using BaseType.Report;
using BaseType.Utils;
using MahApps.Metro.Controls;
using ManagementGui.Config;
using ManagementGui.Utils;
using ManagementGui.View;
using ManagementGui.View.Current;
using ManagementGui.View.Document;
using ManagementGui.View.Menu;
using ManagementGui.ViewModel;
using Xceed.Wpf.AvalonDock.Layout;
using ThicknessConverter = Xceed.Wpf.DataGrid.Converters.ThicknessConverter;

namespace ManagementGui
{
    public delegate void DelegateOpenUser(ApplicationUser applicationUser);
    public delegate void DelegateUserCreateTask(ApplicationUser applicationUser);
    public delegate void DelegateTaskOpen(Guid idTask);

    public delegate void DelegateCloseDocument(List<Guid> godList);

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
      
        #region StaticObject
        private static DelegateCloseDocument RemoteCloseDocument { get; set; }
        private static DelegateUserCreateTask RemoteCreateTaskTask { get; set; }
        private static DelegateOpenUser RemoteOpenUser { get; set; }
        private static DelegateTaskOpen RemoteOpenTask { get; set; }
        public static MainViewModel View;

        public static void DelegateWindowCreateTask(ApplicationUser applicationUser)
        {
            RemoteCreateTaskTask(applicationUser);
        }

        public static void DelegateWindowOpenTask(Guid idTask)
        {
            RemoteOpenTask(idTask);
        }

        public static void DelegateWindowsOpenUser(ApplicationUser applicationUser)
        {
            RemoteOpenUser(applicationUser);
        }

        public static void DelegateCloseWindow(List<Guid> save)
        {
            RemoteCloseDocument(save);
        }
        #endregion

        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {

            try
            {
                RemoteCloseDocument = CloseDocument;
                RemoteOpenTask = OpenTask;
                RemoteCreateTaskTask = CreateTask;
                RemoteOpenUser = RemoteOpenUserCall;
                var model = new AuthenticationWindowViewModel();
                var authenticationWindows = new AuthenticationWindows {DataContext = model};

                if (authenticationWindows.ShowDialog() == true)
                {
                    InitializeComponent();
                    View = new MainViewModel();
                    DataContext = View;
                    Guid id;
                    if (!Guid.TryParse(DesktopSettings.Default.SessionSettings.LastProject, out id))
                        ShowMyProjects_OnClick(null, null);
                    ADManager.DocumentClosed += ADManager_DocumentClosed;
                }
                else { Application.Current.Shutdown(); }
            }
            catch (Exception ex)
            {
                Application.Current.Shutdown();
            }
        }

        public static Guid NewFilesId=Guid.NewGuid();
        public Dictionary<Guid, LayoutAnchorable> OpenAnchorables = new Dictionary<Guid, LayoutAnchorable>();

        public void AddLayoutAnchorable(Guid id,LayoutAnchorable layout)
        {
            var obj = ADManager.Layout.Descendents().OfType<LayoutAnchorablePane>().FirstOrDefault();
            if (obj != null) obj.Children.Add(layout);
        }

        public Dictionary<Guid, LayoutDocument> OpenDocuments = new Dictionary<Guid, LayoutDocument>();
        private void CloseDocument(List<Guid> godlist)
        {
            if (godlist == null)
                godlist = new List<Guid> {_myProjGuid};
            else
                godlist.Add(_myProjGuid);
            var x = OpenDocuments.Where(doc => !godlist.Contains(doc.Key)).Select(doc => doc.Value).ToList();
            foreach (var item in x)
            {
                item.Close();
            }
            OpenDocuments=new Dictionary<Guid, LayoutDocument>();
        }

        private void RemoteOpenUserCall(ApplicationUser item)
        {
            try
            {
                if (item == null) return;
                var doc = new LayoutDocument
                {
                    Title = item.UserShortName(),
                    Description = item.UserShortNameAndPost(),
                    Content = new UserDocument(item)
                };
                OpenOrActiveLayoutDocument(item.Id, doc);
            }
            catch (Exception ex)
            {
                Logger.MessageBoxException(ex);
            }
        }

        private void ADManager_DocumentClosed(object sender, Xceed.Wpf.AvalonDock.DocumentClosedEventArgs e)
        {
            lock (OpenDocuments)
            {
                OpenDocuments.Remove(OpenDocuments.FirstOrDefault(f => f.Value.Equals(e.Document)).Key);
            }
        }

        private void OpenTask(Guid idtask)
        {
            var task = DbHelper.GetDbProvider.Tasks.FirstOrDefault(f => f.IdTask == idtask);
            if (task == null) return;
            var doc = new LayoutDocument
            {
                Title = task.NameTask,
                Description = task.NameTask,
                Content = new TaskDocument(task)
            };
            OpenOrActiveLayoutDocument(idtask,doc);
        }

        private void NewUserCreateOnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var user = new ApplicationUser
                {
                    IsWork = true,
                    Id = Guid.NewGuid(),
                    Comment = "Новый пользователь",
                    
                };
                var doc = new LayoutDocument
                {
                    Title = "Новый пользователь",
                    Description = user.Comment,
                    Content = new UserDocument(user)
                };
                View.Users.Add(user);
                OpenOrActiveLayoutDocument(user.Id, doc);
            }
            catch (Exception ex)
            {
                Logger.MessageBoxException(ex);
            }
        }

        private void TreeViewDoubleClickOpenUser(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var item = TreeUsers.SelectedItem as ApplicationUser;
                if (item == null) return;
                var doc = new LayoutDocument
                {
                    Title = item.UserShortName(),
                    Description = item.UserShortNameAndPost(),
                    Content = new UserDocument(item)
                };
                OpenOrActiveLayoutDocument(item.Id, doc);
            }
            catch (Exception ex)
            {
                Logger.MessageBoxException(ex);
            }
        }

        private void OpenUser(object sender, RoutedEventArgs e)
        {
            try
            {
                var item = TreeUsers.SelectedItem as ApplicationUser;
                if (item == null) return;
                var doc = new LayoutDocument
                {
                    Title = item.UserShortName(),
                    Description = item.UserShortNameAndPost(),
                    Content = new UserDocument(item)
                };
                OpenOrActiveLayoutDocument(item.Id, doc);
            }
            catch (Exception ex)
            {
                Logger.MessageBoxException(ex);
            }
        }

        private void CreateTask(ApplicationUser applicationUser)
        {
            try
            {
                var task = new Task
                {
                    IdTask = Guid.NewGuid(),
                    DateCreate = DateTime.Now,
                    DateFinish = DateTime.Now.AddDays(3),
                    Project = WorkEnviroment.CurrentProject.IdProject,
                    Author = WorkEnviroment.ApplicationUserSession.Id,
                    Status = StatusTask.Open
                };
                if (applicationUser != null)
                {
                    task.WorkGroup = new ObservableCollection<TaskMembers>
                    {
                        new TaskMembers
                        {
                            Task = task,
                            User = applicationUser,
                            LevelNotivication = LevelNotivication.Normal,
                            TaskRole = TaskRoles.Participant,
                            IdUser = applicationUser.Id,
                            IdTask = task.IdTask
                        }
                    };
                }
                else
                {
                    task.WorkGroup = new ObservableCollection<TaskMembers>();
                }
                var doc = new LayoutDocument
                {
                    Title = "Новая задача",
                    Description = "Новая задача",
                    Content = new TaskDocument(task)
                };
                OpenOrActiveLayoutDocument(task.IdTask,doc);
            }
            catch (Exception ex)
            {
                Logger.MessageBoxException(ex);
            }
        }

        private void CreateTask(object sender, RoutedEventArgs e)
        {
            try
            {
                var item = TreeUsers.SelectedItem as ApplicationUser;
                if (item == null) return;
                var task = new Task
                {
                    IdTask = Guid.NewGuid(),
                    DateFinish = DateTime.Now.AddDays(3),
                    Project = WorkEnviroment.CurrentProject.IdProject
                };
                task.WorkGroup = new ObservableCollection<TaskMembers>
                {
                    new TaskMembers
                    {
                        Task = task,
                        User = item,
                        LevelNotivication = LevelNotivication.Normal,
                        TaskRole = TaskRoles.Participant,
                        IdUser = item.Id,
                        IdTask = task.IdTask
                    }
                };
                var doc = new LayoutDocument
                {
                    Title = "Новая задача",
                    Description = "Новая задача",
                    Content = new TaskDocument(task)
                };
                OpenOrActiveLayoutDocument(task.IdTask, doc);
            }
            catch (Exception ex)
            {
                Logger.MessageBoxException(ex);
            }
        }

        private void ImportPersonsOnXLS(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OpenOrActiveLayoutDocument(Guid id, LayoutDocument doc)
        {
            try
            {
                var obj = ADManager.Layout.Descendents().OfType<LayoutDocumentPane>().FirstOrDefault();
                if (obj == null) return;
                if (OpenDocuments.ContainsKey(id))
                {
                    var panel = OpenDocuments[id];
                    panel.IsActive = true;
                }
                else
                {
                    obj.Children.Add(doc);
                    OpenDocuments.Add(id, doc);
                    doc.IsActive = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка открытия документа!", MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
        }

        private void NewTaskOnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var task = new Task
                {
                    IdTask = Guid.NewGuid(),
                    WorkGroup = new ObservableCollection<TaskMembers>()
                };
                var doc = new LayoutDocument
                {
                    Title = "Новая задача",
                    Description = task.NameTask,
                    Content = new TaskDocument(task)
                };
                OpenOrActiveLayoutDocument(task.IdTask, doc);
            }
            catch (Exception ex)
            {
                Logger.MessageBoxException(ex);
            }
        }

        private readonly Guid _myProjGuid = Guid.NewGuid();

        private void ShowMyProjects_OnClick(object sender, RoutedEventArgs e)
        {
            var doc = new LayoutDocument
            {
                Title = "Мои проекты",
                Description = "Управление проектами",
                Content = new ProjectManage()
            };
            OpenOrActiveLayoutDocument(_myProjGuid, doc);
        }

        private void TreeViewDoubleClickOpenReport(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var item = ReportList.SelectedItem as ReportItem;
                if (item == null) return;
                var doc = new LayoutDocument
                {
                    Title = item.Name,
                    Description = item.Description,
                    Content = Activator.CreateInstance(item.Type)
                };
                OpenOrActiveLayoutDocument(item.Id, doc);
            }
            catch (Exception ex)
            {
                Logger.MessageBoxException(ex);
            }
        }

        private readonly Guid _currentesTaskId = Guid.NewGuid();

        private void Users_OnSelected(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Currentes == null || !Currentes.IsSelected) return;
                var doc = new LayoutDocument
                {
                    Title = "Задачи",
                    Description = string.Format("Задачи ({0})", WorkEnviroment.CurrentProject.Name),
                    Content = new CurrentTasks()
                };
                OpenOrActiveLayoutDocument(_currentesTaskId, doc);
            }
            catch (Exception ex)
            {
                Logger.MessageBoxException(ex);
            }
        }

        private void MISistemUserList_OnClick(object sender, RoutedEventArgs e)
        {
            
        }

        private void ShowNewFilesMenu_OnClick(object sender, RoutedEventArgs e)
        {
            var layoutAnchorable = new LayoutAnchorable { Title = "Новые файлы", Content = new NewFilesMenu() };

            AddLayoutAnchorable(NewFilesId, layoutAnchorable);
        }

        private async void AddUserFromListOnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var window = new View.SearchUserWindow(DataGridSelectionMode.Single);
                if (window.ShowDialog() != true) return;
                var selectedUser = window.View.SelectedUsers[0];
                if (WorkEnviroment.CurrentProject.Members.Any(a => a.IdUser == selectedUser.Id)) return;
                var member = new Member
                {
                    Project = WorkEnviroment.CurrentProject,
                    User = selectedUser,
                    Role = Role.User
                };
                WorkEnviroment.CurrentProject.Members.Add(member);
                DbHelper.GetDbProvider.UserRoles.Add(member);
                await DbHelper.GetDbProvider.SaveChangesAsync();
                View.Users.Add(selectedUser);

            }
            catch (Exception ex)
            {
                Logger.MessageBoxException(ex);
            }
        }
    }
}