using System;
using System.Collections.ObjectModel;
using System.Data.Entity.Migrations;
using System.Windows;
using System.Windows.Input;
using BaseType;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ManagementGui.Utils;

namespace ManagementGui.Admin
{
    public class AdminProjectsWindow : ViewModelBase
    {
        public readonly ApplicationDbContext ContextData = DbHelper.GetDbProvider;
        /// <summary>
        /// Initializes a new instance of the AdminProjectsWindow class.
        /// </summary>
        public AdminProjectsWindow()
        {
            Projects = new ObservableCollection<Project>(ContextData.Projects);
        }

        private Project _current;
        public Project Current {
            get
            {
                if (_current != null)
                    return _current;
                return null;
            }
            set
            {
                _current = value;
                // Метод из базового класса для запуска события NotifyPropertyChanged
                RaisePropertyChanged("Current");
            }
        }
        public ObservableCollection<Project> Projects { get; set; }

        private RelayCommand _createProjectCommand;
        
        

        public ICommand CreateProject
        {
            get
            {
                if (_createProjectCommand == null)
                {
                    _createProjectCommand = new RelayCommand(Create);
                }
                return _createProjectCommand;
            }
        }

        private void Create()
        {
            Project project=new Project(){IdProject = Guid.NewGuid(),
            Name = "Новый проект",TypeProject = TypeProject.Divison,
            DateCreate = DateTime.Now};
            Current = project;
            Projects.Add(project);
            ContextData.Projects.Add(Current);
        }
        private RelayCommand _deleteProject;
        public ICommand DeleteProject
        {
            get
            {
                if (_deleteProject == null)
                {
                    _deleteProject = new RelayCommand(Delete);
                }
                return _deleteProject;
            }
        }

        private void Delete()
        {
            try
            {
                if (Current != null)
                    if (MessageBox.Show(
                        string.Format("Вы действительно хотите удалить подразделение {0}", Current.Name),
                        "Удаление проекта",
                        MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.OK)
                    {
                        Projects.Remove(Current);
                        ContextData.Projects.Remove(Current);
                        ContextData.SaveChanges();
                    }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    string.Format("Попытка удалить проект {0} завершилась ошибкой{2}{1}", Current.Name, ex.Message,
                        Environment.NewLine), "Удаление проекта", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private RelayCommand _saveCurrent;

        public ICommand SaveCurrent
        {
            get { return _saveCurrent ?? (_saveCurrent = new RelayCommand(Save)); }
        }

        private void Save()
        {
            try
            {
                Current.DateUpdate = DateTime.Now;
                ContextData.Projects.AddOrUpdate(Current);
                
                ContextData.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace, ex.Message, MessageBoxButton.OK);
            }

        }

      

      
    }
}