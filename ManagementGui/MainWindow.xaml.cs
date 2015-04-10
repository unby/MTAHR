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
using ManagementGui.Admin;
using ManagementGui.Config;
using ManagementGui.Utils;
using ManagementGui.View;
using ManagementGui.View.Current;
using ManagementGui.View.Document;
using ManagementGui.ViewModel;
using Xceed.Wpf.AvalonDock.Layout;

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
        public Dictionary<Guid,LayoutDocument> OpenDocuments=new Dictionary<Guid, LayoutDocument>();
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

        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            RemoteCloseDocument = CloseDocument;
            RemoteOpenTask = OpenTask;
            RemoteCreateTaskTask = CreateTask;
            RemoteOpenUser = RemoteOpenUserCall;
            var model = new AuthenticationWindowViewModel();
            var authenticationWindows = new AuthenticationWindows { DataContext = model };
            if (authenticationWindows.ShowDialog() == true)
            {
                InitializeComponent();
                View = new MainViewModel();
                DataContext = View;
            }
            else
            {
                try
                {
                    Application.Current.Shutdown();
                }
                catch(Exception ex)
                {
                    Logger.WriteException("MainWindow Constructor", ex);
                }
            }
            Guid id;
            if(!Guid.TryParse(DesktopSettings.Default.SessionSettings.LastProject,out id))
                ShowMyProjects_OnClick(null, null);
            ADManager.DocumentClosed+=ADManager_DocumentClosed;
        }

        private void CloseDocument(List<Guid> godlist)
        {
            if(godlist==null)
                godlist=new List<Guid>(){_myProjGuid};
            else
                godlist.Add(_myProjGuid);
            foreach (var doc in OpenDocuments.Where(doc => !godlist.Contains(doc.Key)))
            {               
                doc.Value.Close();
            }
        }

        private void RemoteOpenUserCall(ApplicationUser item)
        {
            try
            {
                var obj = ADManager.Layout.Descendents().OfType<LayoutDocumentPane>().FirstOrDefault();
                if (obj != null)
                {
                    if (item != null)
                    {
                        if (!OpenDocuments.ContainsKey(item.Id))
                        {
                            var doc = new LayoutDocument()
                            {
                                Title = item.UserShortName(),
                                Description = item.UserShortNameAndPost()
                            };
                            doc.Content = new UserDocument(item);
                            obj.Children.Add(doc);
                            OpenDocuments.Add(item.Id, doc);
                            doc.IsActive = true;
                        }
                        else
                        {
                            LayoutDocument panel = OpenDocuments[item.Id];
                            panel.IsActive = true;
                        }
                    }
                }
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
                OpenDocuments.Remove(OpenDocuments.FirstOrDefault(f => f.Value == e.Document).Key);
            }
        }

        private void OpenTask(Guid idtask)
        {
            if (OpenDocuments.ContainsKey(idtask))
            {
                LayoutDocument panel = OpenDocuments[idtask];
                panel.IsActive = true;
            }
            else
            {
                var obj = ADManager.Layout.Descendents().OfType<LayoutDocumentPane>().FirstOrDefault();
                if (obj != null)
                {
                    Task task = DbHelper.GetDbProvider.Tasks.FirstOrDefault(f => f.IdTask == idtask);
                    if (task != null)
                    {
                        var doc = new LayoutDocument()
                        {
                            Title = "Новый пользователь",
                            Description = "Новый пользователь"
                        };
                        doc.Content = new TaskDocument(task);
                        obj.Children.Add(doc);
                        doc.IsActive = true;
                    }
                }
            }
        }

        private void MISistemUserList_OnClick(object sender, RoutedEventArgs e)
        {
            var projectWindow=new ProjectsWindow();
            projectWindow.ShowDialog();
        }

        private void NewUserCreateOnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var obj = ADManager.Layout.Descendents().OfType<LayoutDocumentPane>().FirstOrDefault();
                if (obj != null)
                {
                    var userRole = new ApplicationUser()
                    {
                        IsWork = true,
                        Id = Guid.NewGuid(),
                        Comment = "Новый пользователь"
                    };
                    var doc = new LayoutDocument() {Title = "Новый пользователь", Description = "Новый пользователь"};
                    doc.Content = new UserDocument(userRole);
                    View.Users.Add(userRole);
                    obj.Children.Add(doc);
                    doc.IsActive = true;
                }
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
                var obj = ADManager.Layout.Descendents().OfType<LayoutDocumentPane>().FirstOrDefault();
                if (obj != null)
                {
                    var item = TreeUsers.SelectedItem as ApplicationUser;
                    if (item != null)
                    {
                        if (!OpenDocuments.ContainsKey(item.Id))
                        {
                            var doc = new LayoutDocument
                            {
                                Title = item.UserShortName(),
                                Description = item.UserShortNameAndPost(),
                                Content = new UserDocument(item)
                            };
                            obj.Children.Add(doc);
                            OpenDocuments.Add(item.Id, doc);
                            doc.IsActive = true;
                        }
                        else
                        {
                            LayoutDocument panel = OpenDocuments[item.Id];
                            panel.IsActive = true;
                        }
                    }
                }

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
                var obj = ADManager.Layout.Descendents().OfType<LayoutDocumentPane>().FirstOrDefault();
                if (obj != null)
                {
                    var item = TreeUsers.SelectedItem as ApplicationUser;
                    if (item != null)
                    {
                        if (!OpenDocuments.ContainsKey(item.Id))
                        {
                           var doc = new LayoutDocument()
                            {
                                Title = item.UserShortName(),
                                Description = item.UserShortNameAndPost()
                            };
                           doc.Content = new UserDocument(item);
                            obj.Children.Add(doc);
                            OpenDocuments.Add(item.Id, doc);
                            doc.IsActive = true;
                        }
                        else
                        {
                            LayoutDocument panel = OpenDocuments[item.Id];
                            panel.IsActive = true;
                        }
                    }
                }
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
                var obj = ADManager.Layout.Descendents().OfType<LayoutDocumentPane>().FirstOrDefault();
                if (obj != null)
                {

                        var task = new Task()
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
                            new TaskMembers()
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
                    var doc = new LayoutDocument()
                        {
                            Title = "Новая задача",
                            Description = "Новая задача"
                        };
                        doc.Content = new TaskDocument(task);
                        obj.Children.Add(doc);
                        doc.IsActive = true;
                    }               
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
                var obj = ADManager.Layout.Descendents().OfType<LayoutDocumentPane>().FirstOrDefault();
                if (obj != null)
                {
                    var item = TreeUsers.SelectedItem as ApplicationUser;
                    if (item != null)
                    {
                            var task = new Task()
                            {
                                IdTask = Guid.NewGuid(),
                                DateFinish = DateTime.Now.AddDays(3),
                                Project = WorkEnviroment.CurrentProject.IdProject
                            };
                            task.WorkGroup = new ObservableCollection<TaskMembers>
                            {
                                new TaskMembers()
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
                                Title = task.NameTask.CutString(0, 16),
                                Description = task.NameTask,
                                Content = new TaskDocument(task)
                            };
                        obj.Children.Add(doc);
                            OpenDocuments.Add(item.Id, doc);
                            doc.IsActive = true;
                        
                    }
                }
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

        private void NewTaskOnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var obj = ADManager.Layout.Descendents().OfType<LayoutDocumentPane>().FirstOrDefault();
                if (obj != null)
                {
                        var task = new Task
                        {
                            IdTask = Guid.NewGuid(),
                            WorkGroup = new ObservableCollection<TaskMembers>()
                        };
                        var doc = new LayoutDocument
                        {
                            Title = task.NameTask.CutString(0, 16),
                            Description = task.NameTask,
                            Content = new TaskDocument(task)
                        };
                        obj.Children.Add(doc);
                        OpenDocuments.Add(task.IdTask, doc);
                        doc.IsActive = true;                   
                }
            }
            catch (Exception ex)
            {
                Logger.MessageBoxException(ex);
            }
        }

        private void ManageSystem_OnClick(object sender, RoutedEventArgs e)
        {
            var adminWindow = new SystemAdminWindow();
            adminWindow.ShowDialog();
        }

        readonly Guid _myProjGuid = Guid.NewGuid();
        private void ShowMyProjects_OnClick(object sender, RoutedEventArgs e)
        {
            
            var obj = ADManager.Layout.Descendents().OfType<LayoutDocumentPane>().FirstOrDefault();
            if (obj != null)
            {
                if (!OpenDocuments.ContainsKey(_myProjGuid))
                {
                    var doc = new LayoutDocument()
                    {
                        Title = "Мои проекты",
                        Description = "Управление проектами"
                    };
                    doc.Content = new ProjectManage();
                    obj.Children.Add(doc);
                    OpenDocuments.Add(_myProjGuid, doc);
                    doc.IsActive = true;
                }
                else
                {
                    LayoutDocument panel = OpenDocuments[_myProjGuid];
                    panel.IsActive = true;
                }
            }
        }

        private void TreeViewDoubleClickOpenReport(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var obj = ADManager.Layout.Descendents().OfType<LayoutDocumentPane>().FirstOrDefault();
                if (obj != null)
                {
                    var item = ReportList.SelectedItem as ReportItem;
                    if (item != null)
                    {
                        if (!OpenDocuments.ContainsKey(item.Id))
                        {
                            var doc = new LayoutDocument()
                            {
                                Title = item.Name,
                                Description = item.Description
                            };
                            doc.Content = Activator.CreateInstance(item.Type);
                            obj.Children.Add(doc);
                            OpenDocuments.Add(item.Id, doc);
                            doc.IsActive = true;
                        }
                        else
                        {
                            LayoutDocument panel = OpenDocuments[item.Id];
                            panel.IsActive = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.MessageBoxException(ex);
            }
        }
        readonly Guid CurrentesTaskId = Guid.NewGuid();
        private void Users_OnSelected(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Currentes != null && Currentes.IsSelected)
                {
                    var obj = ADManager.Layout.Descendents().OfType<LayoutDocumentPane>().FirstOrDefault();
                    if (obj != null)
                    {
                        if (!OpenDocuments.ContainsKey(CurrentesTaskId))
                        {
                            var doc = new LayoutDocument
                            {
                                Title = "Задачи",
                                Description = string.Format("Задачи ({0})", WorkEnviroment.CurrentProject.Name),
                                Content = new CurrentTasks()
                            };
                            obj.Children.Add(doc);
                            OpenDocuments.Add(CurrentesTaskId, doc);
                            doc.IsActive = true;
                        }
                        else
                        {
                            LayoutDocument panel = OpenDocuments[CurrentesTaskId];
                            panel.IsActive = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.MessageBoxException(ex);
            }
        }

        private void Users_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (Currentes!=null&&Currentes.IsSelected)
                {
                    var obj = ADManager.Layout.Descendents().OfType<LayoutDocumentPane>().FirstOrDefault();
                    if (obj != null)
                    {
                        if (!OpenDocuments.ContainsKey(CurrentesTaskId))
                        {
                            var doc = new LayoutDocument
                            {
                                Title = "Задачи",
                                Description = string.Format("Задачи ({0})", WorkEnviroment.CurrentProject.Name),
                                Content = new CurrentTasks()
                            };
                            obj.Children.Add(doc);
                            OpenDocuments.Add(CurrentesTaskId, doc);
                            doc.IsActive = true;
                        }
                        else
                        {
                            LayoutDocument panel = OpenDocuments[CurrentesTaskId];
                            panel.IsActive = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.MessageBoxException(ex);
            }
        }     
    }
}