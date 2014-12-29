using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using BaseType.Common;
using BaseType.Utils;

namespace BaseType
{
    [Serializable]
    public class User : INotifyPropertyChanged,IMemento
    {
        public User()
        {
            ObjectHystory=new RoundStack<KeyValuePair<string, object>>(80);
            MementoFlag = true;
        }

        public override string ToString()
        {
            return string.Format("{0} {1} {2} {3}", Surname, Name, MiddleName, Post); 
        }

        private Guid _idUser;

        [Key]
        [Required]
        [Category("Служебная информация")]
        [DisplayName("Идентификатор")]
        [Description("Идентификатор пользователя в БД")]
        public Guid IdUser
        {
            get { return _idUser; }
            set { _idUser = value; }
        }

        private string _eMail;

        [Required(ErrorMessage = "Почтовый адрес не заполнен")]
        [StringLength(150, ErrorMessage = "Введите почтовый адрес сотрудника", MinimumLength = 5)]
        [RegularExpression(EntityValidate.MatchEmailPattern, ErrorMessage = "Вы должны вести eMail")]
        [Description("Основной почтовый электронный адрес")]
        [Category("Служебная информация")]
        [DisplayName("EMail")]
        public string Email
        {
            get { return _eMail; }
            set
            {
                if (MementoFlag)
                    Set("Email", value);
                _eMail = value;
                OnPropertyChanged("Email");
            }
        }

        private string _surname;

        [Required]
        [Category("Личная информация")]
        [DisplayName("Фамилия")]
        [Description("Фамилия")]
        public string Surname
        {
            get { return _surname; }
            set
            {
                if (MementoFlag)
                    Set("Surname", value);
                _surname = value;
                OnPropertyChanged("Surname");
            }
        }

        private string _name;

        [Required]
        [Category("Личная информация")]
        [Display(Name = "Имя")]
        [DisplayName("Имя")]
        [Description("Имя")]
        public string Name
        {
            get { return _name; }
            set
            {
                if (MementoFlag)
                    Set("Name", value);
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        private string _midleName;

        [Category("Личная информация")]
        [DisplayName("Отчество")]
        [Description("Отчество")]
        public string MiddleName
        {
            get { return _midleName; }
            set
            {
                if (MementoFlag)
                    Set("MiddleName", value);
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
                if (MementoFlag)
                    Set("BirthDate", value);
                _birthDate = value;
                OnPropertyChanged("BirthDate");
            }
        }

        private int _phoneNumber;

        [Category("Личная информация")]
        [DisplayName("Номер телефона")]
        [Description("Номер телефона")]
        public int PhoneNumber
        {
            get { return _phoneNumber; }
            set
            {
                if (MementoFlag)
                    Set("PhoneNumber", value);
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
                if (MementoFlag)
                    Set("IsWork", value);
                _isWork = value;
                OnPropertyChanged("IsWork");
            }
        }

        private string _comment;

        [Category("Служебная информация")]
        [DisplayName("Комментарий")]
        [Description("Комментарий")]
        public string Comment
        {
            get { return _comment; }
            set
            {
                if (MementoFlag)
                    Set("Comment", value);
                _comment = value;
                OnPropertyChanged("Comment");
            }
        }

        private Role _systemRole;

        [Category("Параметры информационной системы")]
        [DisplayName("Роль пользователя")]
        [Description("Роль пользователя")]
        public Role SystemRole
        {
            get { return _systemRole; }
            set
            {
                if (MementoFlag)
                    Set("SystemRole", value);
                _systemRole = value;
                OnPropertyChanged("SystemRole");
            }
        }

        private string _post;
        [Category("Служебная информация")]
        [DisplayName("Должность")]
        [Description("Должность")]
        public string Post
        {
            get { return _post; }
            set
            {
                if (MementoFlag)
                    Set("Post", value);
                _post = value;
                OnPropertyChanged("Post");
            }
        }

        private const string PasswordHash = "P@@Sw0rd";
        private const string SaltKey = "S@LT&KEY";
        private const string VIKey = "@1B2c3D4e5F6g7H8";

        public static string Encrypt(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            var keyBytes = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(256 / 8);
            var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.Zeros };
            var encryptor = symmetricKey.CreateEncryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));

            byte[] cipherTextBytes;

            using (var memoryStream = new MemoryStream())
            {
                using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                    cryptoStream.FlushFinalBlock();
                    cipherTextBytes = memoryStream.ToArray();
                    cryptoStream.Close();
                }
                memoryStream.Close();
            }
            return Convert.ToBase64String(cipherTextBytes);
        }
        public static string Decrypt(string encryptedText)
        {
            var cipherTextBytes = Convert.FromBase64String(encryptedText);
            var keyBytes = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(256 / 8);
            var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.None };

            var decryptor = symmetricKey.CreateDecryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));
            var memoryStream = new MemoryStream(cipherTextBytes);
            var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            var plainTextBytes = new byte[cipherTextBytes.Length];

            int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
            memoryStream.Close();
            cryptoStream.Close();
            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount).TrimEnd("\0".ToCharArray());
        }

        private string _password;

        [PasswordPropertyText(true)]
        public string Password {
            get { return Decrypt(_password); }
            set
            {
                _password = Encrypt(value);
                OnPropertyChanged("Password");
            }
        }
        [NotMapped]
        [Browsable(false)]
        public Operation Operation { get; set; }

        public string ModelDisplay
        {
            get
            {
                string completeName = string.Format("{0} {1} {2} {3}", Surname, Name, MiddleName, Post);
                return completeName;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                PropertyChanged(this, new PropertyChangedEventArgs("ModelDisplay"));
            }
        }
        #region IMemento
        [NotMapped]
        [Browsable(false)]
        public RoundStack<KeyValuePair<string, object>> ObjectHystory { get; set; }

        public void Set(string name, object value)
        {
            ObjectHystory.SetValue(new KeyValuePair<string, object>(name, value));
        }
        [NotMapped]
        [Browsable(false)]
        public bool MementoFlag { get; set; }

        public void RestorePropery(KeyValuePair<string, object> memento)
        {

                if (string.IsNullOrEmpty(memento.Key))
                    return;
                var myType = this.GetType();
                var pinfo = myType.GetProperty(memento.Key);
                pinfo.SetValue(this, memento.Value);
        }
        private RelayCommand _undoCommand;
        [NotMapped]
        [Browsable(false)]
        public RelayCommand Undo
        {
            get { return _undoCommand ?? (_undoCommand = new RelayCommand(UndoMethod)); }
        }

        public void UndoMethod(object obj)
        {
            MementoFlag = false;
            RestorePropery(ObjectHystory.Undo);
            MementoFlag = true;
        }

        private RelayCommand _rendoCommand;
        [NotMapped]
        [Browsable(false)]
        public RelayCommand Redo
        {
            get { return _rendoCommand ?? (_rendoCommand = new RelayCommand(RendoMethod)); }
        }

        public void RendoMethod(object obj)
        {
            RestorePropery(ObjectHystory.Redo);
        }
#endregion
    }
}
