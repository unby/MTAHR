using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseType.Utils;

namespace BaseType
{

    public enum ProjectStatus
    {
        [Description("Открыт")]
        Open=4,
        [Description("Закрыт")]
        Close=0
    }

    public class Project:INotifyPropertyChanged
    {
        public Project()
        {
           // Author = new User();
           // UsersProject=new ObservableCollection<Member>();
           // Tasks=new List<Task>();
        }

        //public virtual ICollection<Member> UserRoles { get; set; }

        public Project(Project s)
        {
            IdProject = s.IdProject;
            Author=s.Author;
            Comment = s.Comment;
            Purpose = s.Purpose;
            Name = s.Name;
            DateCreate = s.DateCreate;
            DateUpdate = s.DateUpdate;
            Tasks = s.Tasks;
            TypeProject = s.TypeProject;
            

        }

        [Key]
        [DisplayName("Идентификатор")]
        [Description("Идентификатор сущности в БД")]
        public Guid IdProject { get; set; }

        private string _name;
        [DisplayName("Название")]
        [Description("Название организационной еденицы")]
        [StringLength(70)]
        [Index("UK_ProjectName",IsUnique = true)]
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }
        private string _comment;
        [DisplayName("Новость")]
        [Description("Общий комментарий")]
        [StringLength(90)]
        public string Comment
        {
            get { return _comment; }
            set
            {
                _comment = value;
                OnPropertyChanged("Comment");
            }
        }

        private string _purpose;// OnPropertyChanged("BirthDate");
        [DisplayName("Цели")]
        [Description("Цели")]
        [StringLength(600)]
        public string Purpose
        {
            get { return _purpose; }
            set
            {
                _purpose = value;
                OnPropertyChanged("Purpose");
            }
        }
        public virtual ICollection<Member> Members { get; set; }

        private ApplicationUser _author;
        private ProjectStatus _status=ProjectStatus.Open;
        [DisplayName("Статус проекта")]
        [Description("Статус проекта")]
        public ProjectStatus Status
        {
            get { return _status; }
            set
            {
                _status = value;
                OnPropertyChanged("Status");
            }
        }

        [DisplayName("Руководитель")]
        [Description("Руководитель")]
        public virtual ApplicationUser Author
        {
            get { return _author; }
            set
            {
                _author = value;
                OnPropertyChanged("Author");
            }
        }
        [DisplayName("Дата создания")]
        [Description("Дата создания подразделения")]
        public DateTime DateCreate { get; set; }
        [DisplayName("Дата редактирования")]
        [Description("Последние изменение")]
        public DateTime DateUpdate { get; set; }


        private TypeProject _typeProject;
        [DisplayName("Тип проекта")]
        [Description("Тип проекта")]
        public TypeProject TypeProject
        {
            get { return _typeProject; }
            set
            {
                _typeProject = value;
                OnPropertyChanged("TypeProject");
            }
        }

        public virtual ICollection<Task> Tasks { get; set; }

        public override string ToString()
        {
            return string.Format("{0}: {1} Руководитель {2}",TypeProject.GetDescription(),Name,Author);
        }

        public event PropertyChangedEventHandler PropertyChanged;


        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

}
