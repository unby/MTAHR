using System;
using System.Windows;
using System.Windows.Input;
using BaseType;
using BaseType.Common;
using ManagementGui.Config;
using ManagementGui.Utils;
using ManagementGui.ViewModel.Validation;

namespace ManagementGui.ViewModel
{
    public class ProjectManageViewModel : ValidationViewModelBase
    {
        private Project _currentProject;
        public Project CurrentProject
        {
            get { return _currentProject; }
            set
            {
                _currentProject = value;
                RaisePropertyChanged();
            }
        }

        public ProjectManageViewModel()
        {
            CurrentProject = WorkEnviroment.CurrentProject;
        }

        ///Update
        private RelayCommand _update;

        public ICommand Update
        {
            get { return _update ?? (_update = new RelayCommand(UpdateDB)); }
        }
        private async void UpdateDB(object obj)
        {
            try
            {
                await DbHelper.GetDbProvider.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Logger.MessageBoxException(ex);
            }
        }
        //EditMyProfile
        private RelayCommand _editMyProfile;

        public ICommand EditMyProfile
        {
            get { return _editMyProfile ?? (_editMyProfile = new RelayCommand(EditMyProfileUser)); }
        }

        private void EditMyProfileUser(object obj)
        {
           MainWindow.DelegateWindowsOpenUser(WorkEnviroment.ApplicationUserSession);
        }
        //SetDefaultProject
        private RelayCommand _setDefaultProject;

        public ICommand SetDefaultProject
        {
            get { return _setDefaultProject ?? (_setDefaultProject = new RelayCommand(SetDefaultProjectConfig)); }
        }

        private void SetDefaultProjectConfig(object obj)
        {
            try
            {
               DesktopSettings.Default.SaveSession(WorkEnviroment.CurrentProject.IdProject);
            }
            catch (Exception ex)
            {
                Logger.MessageBoxException(ex);
            }
        }

     
        //ReloadProject
        private RelayCommand _reloadProject;

        public ICommand ReloadProject
        {
            get { return _reloadProject ?? (_reloadProject = new RelayCommand(ReloadProjectSession)); }
        }

        private void ReloadProjectSession(object obj)
        {
            try
            {
                if (WorkEnviroment.UserProjects == null || WorkEnviroment.UserProjects.Count <= 0) return;
                var result =
                    MessageBox.Show("Вы действительно хотите загрузить проект?,все открытые документы будут закрыты",
                        "Загрузка проектов", MessageBoxButton.YesNo, MessageBoxImage.Information);
                if (result != MessageBoxResult.Yes) return;
                WorkEnviroment.CurrentProject = CurrentProject;
                MainWindow.DelegateCloseWindow(null);
                MainWindow.View.Update(null);
                MainWindow.View.SetInfo();
            }
            catch (Exception ex)
            {
                Logger.MessageBoxException(ex);
            }
        }
    }
}
