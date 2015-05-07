using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BaseType.Security;
using BaseType.Utils;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BaseType
{

    [Serializable]
    public class ApplicationUser : IdentityUser<Guid, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>, INotifyPropertyChanged
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser, Guid> manager)
        {
            // Обратите внимание, что authenticationType должен совпадать с типом, определенным в CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Здесь добавьте утверждения пользователя
            return userIdentity;
        }

        public virtual ICollection<Member> Members { get; set; }

        private string _loginName;
        [Category("Служебная информация")]
        [DisplayName("Имя точки входа")]
        [Description("Параметры авторизации пользователя в информационный системе")]
        [StringLength(50)]
        public string LoginName
        {
            get { return _loginName; }
            set
            {
                _loginName = value;
                OnPropertyChanged("LoginName");
                OnPropertyChanged("LoginNameAndSID");
            }
        }

        private byte[] _sid;
        [Browsable(false)]
        [Category("Служебная информация")]
        [DisplayName("Идентификатор точки входа")]
        [Description("Параметры авторизации пользователя в информационный системе")]
        [MaxLength(600)]
        public byte[] SID
        {
            get { return _sid; }
            set
            {
                _sid = value;
                OnPropertyChanged("LoginNameAndSID");
                OnPropertyChanged("SID");
            }
        }

        public ApplicationUser()
        {
            this.SecurityStamp = Guid.NewGuid().ToString();
        }

        public override string ToString()
        {
            return string.Format("{0} {1} {2} {3}", Surname, Name, MiddleName, Post);
        }
        [NotMapped]
        [Category("Служебная информация")]
        [DisplayName("Идентификатор точки входа")]
        [Description("Параметры авторизации пользователя в информационный системе")]
        public string LoginNameAndSID
        {
            get { return string.Format("{0} SID:{1}", LoginName, SID.ConvertByteToStringSid()); }
        }
        private Guid _id;

        [Key]
        [Required]
        [ReadOnly(true)]
        [Category("Служебная информация")]
        [DisplayName("Идентификатор")]
        [Description("Идентификатор пользователя в БД")]
        //    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override Guid Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _eMail;

        [Required(ErrorMessage = "Почтовый адрес не заполнен")]
        [StringLength(150, ErrorMessage = "Введите почтовый адрес сотрудника", MinimumLength = 5)]
        [RegularExpression(EntityValidate.MatchEmailPattern, ErrorMessage = "Вы должны вести eMail")]
        [Description("Основной почтовый электронный адрес")]
        [Category("Служебная информация")]
        [DisplayName("EMail")]
        [Index("UK_Email", IsUnique = true)]
        public override string Email
        {
            get { return _eMail; }
            set
            {
                _eMail = value;
                OnPropertyChanged("Email");
            }
        }

        private string _surname;

        [Required]
        [Category("Личная информация")]
        [DisplayName("Фамилия")]
        [Description("Фамилия")]
        [StringLength(30)]
        public string Surname
        {
            get { return _surname; }
            set
            {
                _surname = value;
                this.UserName = this.UserName();
                OnPropertyChanged("Surname");
            }
        }

        private string _name;
      

        [Required]
        [Category("Личная информация")]
        [Display(Name = "Имя")]
        [DisplayName("Имя")]
        [Description("Имя")]
        [StringLength(25)]
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                this.UserName = this.UserName();
                OnPropertyChanged("Name");
            }
        }

        private string _midleName;

        [Category("Личная информация")]
        [DisplayName("Отчество")]
        [Description("Отчество")]
        [StringLength(30)]
        public string MiddleName
        {
            get { return _midleName; }
            set
            {
                _midleName = value;
                OnPropertyChanged("MiddleName");
            }
        }


        private DateTime? _birthDate;

        [Category("Личная информация")]
        [DisplayName("День рождения сотрудника")]
        [Description("День рождения сотрудника")]
        public DateTime? BirthDate
        {
            get { return _birthDate; }
            set
            {
                _birthDate = value;
                OnPropertyChanged("BirthDate");
            }
        }

        private string _phoneNumber;

        [Category("Личная информация")]
        [DisplayName("Номер телефона")]
        [Description("Номер телефона")]
        [StringLength(20)]
        public override string PhoneNumber
        {
            get { return _phoneNumber; }
            set
            {
                _phoneNumber = value;
                OnPropertyChanged("PhoneNumber");
            }
        }

        private bool _isWork;

        [Category("Служебная информация")]
        [DisplayName("Трудоустроен")]
        [Description("Трудоустроен")]
        public bool IsWork
        {
            get { return _isWork; }
            set
            {
                _isWork = value;
                OnPropertyChanged("IsWork");
            }
        }

        private string _comment;

        [Category("Служебная информация")]
        [DisplayName("Комментарий")]
        [Description("Комментарий")]
        [StringLength(250)]
        public string Comment
        {
            get { return _comment; }
            set
            {
                _comment = value;
                OnPropertyChanged("Comment");
            }
        }

        public virtual ICollection<TaskMembers> UserTasks { get; set; }

        private string _post;
        [Category("Служебная информация")]
        [DisplayName("Должность")]
        [Description("Должность")]
        [StringLength(60)]
        public string Post
        {
            get { return _post; }
            set
            {
                _post = value;
                OnPropertyChanged("Post");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #region Method

        #endregion
    }


}
