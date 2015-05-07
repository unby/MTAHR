﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Migrations;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using BaseType;
using BaseType.Migrations;
using ManagementGui.Config;
using ManagementGui.Utils;
using ManagementGui.ViewModel;
using Task = BaseType.Task;

namespace ManagementGui.View.Document
{
    /// <summary>
    /// Логика взаимодействия для TaskDocument.xaml
    /// </summary>

    public enum TextProperty
    {
        Comment=0,
        Description=1,
        Result

    }

    public partial class TaskDocument : UserControl
    {
        private TaskDocumentViewModel View { get; set; }

        public TaskDocument(Task task)
        {
            this.Language = XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag);
            InitializeComponent();

            if (string.IsNullOrEmpty( task.NameTask))
            {
                EdProp.IsChecked = true;
                task.DateCreate = DateTime.Now;
            }

            View = new TaskDocumentViewModel(task);
            DataContext = View;
            var cbTypeSource = new[]
            {
                new ItemKeyValue{Id  =0,Value = "Описание задачи"}, 
                new ItemKeyValue{Id  =1,Value = "Комментарий"},
                new ItemKeyValue{Id  =2,Value = "Итоговый результат"},
            };
            CBTypeComment.ItemsSource = cbTypeSource;
            CBTypeComment.SelectedIndex = 0;
                //.Grid.TableStyle.Borders.Bottom = New GridBorder(GridBorderStyle.Solid, Color.SteelBlue,GridBorderWeight.Thin) Me.Grid.TableStyle.Borders.Right = New GridBorder(GridBorderStyle.Solid, Color.SteelBlue,GridBorderWeight.Thin)
        }

        public int PreheviosVariant = -1;
        private void CBTypeComment_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (CBTypeComment.SelectedIndex)
            {
                case 1:
                    UpdateViewModel();
                    HtmlEditor.Text = View.Task.Comment;
                    PreheviosVariant = CBTypeComment.SelectedIndex;
                    return;
                case 2:
                    UpdateViewModel();
                    HtmlEditor.Text = View.Task.Result;
                    PreheviosVariant = CBTypeComment.SelectedIndex;
                    return;
                default:
                    UpdateViewModel();
                    HtmlEditor.Text = View.Task.Description;
                    PreheviosVariant = CBTypeComment.SelectedIndex;
                    return;
            }
        }

        private void UpdateViewModel()
        {
            switch (PreheviosVariant)
            {
                case 0:
                    View.Task.Description = HtmlEditor.Text;
                    return;
                case 1:
                    View.Task.Comment = HtmlEditor.Text;
                    return;
                case 2:
                    View.Task.Result = HtmlEditor.Text;
                    return;
                default:
                    return;
            }
        }

        private void AddNotivication(object sender, RoutedEventArgs e)
        {
            var userViewModel = UsersAndNotifications.SelectedItem as UserTreeViewModel;
            try
            {
                if (userViewModel == null) return;
                var notivication=new Notivication()
                {
                    From = WorkEnviroment.ApplicationUserSession,
                    IdNotivication = Guid.NewGuid(),
                    To = userViewModel.MemberUser.User,
                    IdUserTo = userViewModel.MemberUser.IdUser,
                    IdUserFrom =  WorkEnviroment.ApplicationUserSession.Id,
                    Task = View.Task,
                    IdTask = View.Task.IdTask,
                    DateCreate = DateTime.Now.AddDays(2),
                    TimeSend = WorkEnviroment.GetSendDefaultTime
                };
                var window=new NotivicationWindow(notivication);
                if (window.ShowDialog() != true) return;
                if(View.Task.Notivications!=null)
                    View.Task.Notivications.Add(window.ViewModel.Notivication);
                else
                {
                    View.Task.Notivications=new List<Notivication> {window.ViewModel.Notivication};
                }
                userViewModel.AddUser(window.ViewModel.Notivication);
                View.SaveModel();
                View.Task.Author = WorkEnviroment.ApplicationUserSession.Id;
                       
                DbHelper.GetDbProvider.SaveChanges();
            }
            catch (Exception ex)
            {
                Logger.MessageBoxException(ex);
            }
        }

        private async void RemoveNotivication(object sender, RoutedEventArgs e)
        {
            var notivicationViewModel = UsersAndNotifications.SelectedItem as NotivicationTreeViewModel;
            try
            {
                if (notivicationViewModel == null) return;
                notivicationViewModel.DeleteElementInParent(notivicationViewModel);
                if (View.Task.Notivications == null) return;
                DbHelper.GetDbProvider.Notivications.Remove(notivicationViewModel.Notivication);
                await DbHelper.GetDbProvider.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Logger.MessageBoxException(ex);
            }
        }

        private void AddUser(object sender, RoutedEventArgs e)
        {
            try
            {
                var userWindow = new SearchUserWindow();
                if (userWindow.ShowDialog() != true) return;
                var newList = new List<ApplicationUser>();
                foreach (var user in userWindow.UserSearchGrid.SelectedItems)
                {
                    var temp = user as ApplicationUser;
                    if (temp == null) continue;
                    if (View.Users.Any(viewUser => temp.Id.Equals(viewUser.MemberUser.IdUser)))
                    {
                        temp = null;
                    }
                    if (temp != null)
                        newList.Add(temp);
                }
                if (newList.Count <= 0) return;
                foreach (var user in newList)
                {
                    var member = new TaskMembers()
                    {
                        Task = View.Task,
                        IdTask = View.Task.IdTask,
                        IdUser = user.Id,
                        LevelNotivication = LevelNotivication.Normal,
                        TaskRole = TaskRoles.Participant,
                        User = user
                    };
                    View.Users.Add(new UserTreeViewModel(member, View.Task.IdTask));
                    View.Task.WorkGroup.Add(member);
                    View.Task.Notivications.Add(new Notivication
                    {
                        NotivicationStatus = NotivicationStatus.Declared,
                        DateCreate = DateTime.Now,
                        Description = "На вас назначена новая задача",
                        Task = View.Task,
                        From = WorkEnviroment.ApplicationUserSession,
                        To = user,
                        IdNotivication = Guid.NewGuid(),
                        IdTask = View.Task.IdTask,
                        IdUserFrom = WorkEnviroment.ApplicationUserSession.Id,
                        IdUserTo = user.Id,
                        TimeSend = DateTime.Now
                    });
                }
            }
            catch (Exception ex)
            {
                Logger.MessageBoxException(ex);
            }
        }

        private async void EditNotivication(object sender, RoutedEventArgs e)
        {
            var notivicationTreeViewModel = UsersAndNotifications.SelectedItem as NotivicationTreeViewModel;
            try
            {
                if (notivicationTreeViewModel == null) return;
                var window = new NotivicationWindow(notivicationTreeViewModel.Notivication);
                if (window.ShowDialog() != true) return;
                if (View.Task.Notivications != null)
                    View.Task.Notivications.Add(window.ViewModel.Notivication);
                else
                {
                    View.Task.Notivications = new List<Notivication> {window.ViewModel.Notivication};
                }
                View.SaveModel();
                DbHelper.GetDbProvider.Notivications.AddOrUpdate(notivicationTreeViewModel.Notivication);
                View.SaveModel();
                DbHelper.GetDbProvider.SaveChanges();
            }
            catch (Exception ex)
            {
                Logger.MessageBoxException(ex);
            }
        }

        private async void RemoveUser(object sender, RoutedEventArgs e)
        {
            try
            {

            var userViewModel = UsersAndNotifications.SelectedItem as UserTreeViewModel;
                if (userViewModel == null) return;
                foreach (NotivicationTreeViewModel item in userViewModel.Children)
                {
                    DbHelper.GetDbProvider.Notivications.Remove(item.Notivication);
                }
                View.Task.WorkGroup.Remove(
                    View.Task.WorkGroup.FirstOrDefault(f => f.IdUser == userViewModel.MemberUser.IdUser));
                await DbHelper.GetDbProvider.SaveChangesAsync();
                View.Users.Remove(userViewModel);
            }
            catch (Exception ex)
            {
                Logger.MessageBoxException(ex);
            }
        }

        private async void MessageSave(object sender, RoutedEventArgs e)
        {
            UpdateViewModel();
            var comment=new TaskComment()
            {
                TaskCommentId = Guid.NewGuid(),
                Author = WorkEnviroment.ApplicationUserSession,
                Task = View.Task,
                DateMessage = DateTime.Now,
                Message = View.MessageSend
            };
            DbHelper.GetDbProvider.TaskComments.Add(comment);
            View.Comments.Add(comment);
            await DbHelper.GetDbProvider.SaveChangesAsync();
        }

        private bool _isMessageExpandetOpens;
        private void MessageExpandetOpen(object sender, RoutedEventArgs e)
        {
            if (_isMessageExpandetOpens) return;
            //View.Comments=new ObservableCollection<TaskComment>(View.Task.TaskComments.OrderBy(ob=>ob.DateMessage));
            if(View.Task.TaskComments==null)
                View.Task.TaskComments=new ObservableCollection<TaskComment>();
            foreach (var item in View.Task.TaskComments.OrderByDescending(ob => ob.DateMessage))
            {
                View.Comments.Add(item);
            }

            _isMessageExpandetOpens = true;
        }
    }
}