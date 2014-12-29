using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseType
{
    public class Project
    {
        [Key]
        [DisplayName("Идентификатор")]
        [Description("Идентификатор сущности в БД")]
        public Guid IdProject { get; set; }

        private string _name;
        [DisplayName("Название")]
        [Description("Название организационной еденицы")]
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
        public string Purpose
        {
            get { return _purpose; }
            set
            {
                _purpose = value;
                OnPropertyChanged("Purpose");
            }
        }
        public WorkGroup ProjectGroup { get; set; }

        private User _author;
        [DisplayName("Руководитель")]
        [Description("Руководитель")]
        public User Author
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

        public List<Task> Tasks { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        public string HeadMan {
            get
            {
                if (Author == null)
                    return "Руководитель не определен";
                return string.Format(Author.ToString());
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                PropertyChanged(this, new PropertyChangedEventArgs("ModelDisplay"));
            }
        }
    }

}
