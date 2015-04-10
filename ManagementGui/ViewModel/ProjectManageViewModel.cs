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
                this.RaisePropertyChanged();
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
            get
            {
                if (_update == null)
                    _update = new RelayCommand(UpdateDB);
                return _update;
            }
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
            get
            {
                if (_editMyProfile == null)
                    _editMyProfile = new RelayCommand(EditMyProfileUser);
                return _editMyProfile;
            }
        }

        private void EditMyProfileUser(object obj)
        {
           MainWindow.DelegateWindowsOpenUser(WorkEnviroment.ApplicationUserSession);
        }
        //SetDefaultProject
        private RelayCommand _setDefaultProject;

        public ICommand SetDefaultProject
        {
            get
            {
                if (_setDefaultProject == null)
                    _setDefaultProject = new RelayCommand(SetDefaultProjectConfig);
                return _setDefaultProject;
            }
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
            get
            {
                if (_reloadProject == null)
                    _reloadProject = new RelayCommand(ReloadProjectSession);
                return _reloadProject;
            }
        }

        private void ReloadProjectSession(object obj)
        {
            try
            {
                if (WorkEnviroment.UserProjects != null && WorkEnviroment.UserProjects.Count > 0)
                {
                    var result =
                        MessageBox.Show("Вы действительно хотите загрузить проект?,все открытые документы будут закрыты",
                            "Загрузка проектов", MessageBoxButton.YesNo, MessageBoxImage.Information);
                    if (result == MessageBoxResult.Yes)
                    {
                        WorkEnviroment.CurrentProject = CurrentProject;
                        MainWindow.DelegateCloseWindow(null);
                        MainWindow.View.Update(null);
                        MainWindow.View.SetInfo();
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
