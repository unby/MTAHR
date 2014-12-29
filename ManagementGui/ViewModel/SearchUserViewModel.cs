using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using BaseType;
using BaseType.Utils;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ManagementGui.Utils;

namespace ManagementGui.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class SearchUserViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the SearchUserViewModel class.
        /// </summary>
        public SearchUserViewModel()
        {
            Users = new ObservableCollection<User>(DbHelper.Invoke.Users.Take(25));
        }

        public SearchUserViewModel(User headUser)
        {
            List<User> list = null;
            HeadUser = headUser;
            if (IsWork)
                list = DbHelper.Invoke.Projects.Where(x => x.Author.IdUser == HeadUser.IdUser).
                    Select(y => new List<UserRole>(y.ProjectGroup.Users).Where(v => v.Role == Role.User)).
                    SelectMany(enumirable => enumirable.Where(d => d.User.IsWork == IsWork).Select(b => b.User) ?? null)
                    .ToList();
            else
                list = DbHelper.Invoke.Projects.Where(x => x.Author.IdUser == HeadUser.IdUser).
                    Select(y => new List<UserRole>(y.ProjectGroup.Users).Where(v => v.Role == Role.User)).
                    SelectMany(enumirable => enumirable.Select(b => b.User) ?? null).ToList();

            Users = new ObservableCollection<User>(list);
        }

        #region Search

        private RelayCommand _searchUserCommand;

        public ICommand ISearchUser
        {
            get
            {
                if (_searchUserCommand == null)
                {
                    _searchUserCommand = new RelayCommand(SearchUser);
                }
                return _searchUserCommand;
            }
        }

        public void SearchUser()
        {
            if (string.IsNullOrEmpty(SearchString))
                return;
            else
            {
                List<User> list = null;
                if (HeadUser != null)
                    if (IsWork)
                        list = DbHelper.Invoke.Projects.Where(x => x.Author.IdUser == HeadUser.IdUser).
                            Select(y => new List<UserRole>(y.ProjectGroup.Users).Where(v => v.Role == Role.User)).
                            SelectMany(
                                enumirable =>
                                    enumirable.Where(
                                        d =>
                                            d.User.IsWork == IsWork &&
                                            (Words.EqualsWord(d.User.Name) || Words.EqualsWord(d.User.Surname)))
                                        .Select(b => b.User) ?? null).ToList();
                    else
                        list = DbHelper.Invoke.Projects.Where(x => x.Author.IdUser == HeadUser.IdUser).
                            Select(y => new List<UserRole>(y.ProjectGroup.Users).Where(v => v.Role == Role.User)).
                            SelectMany(
                                enumirable =>
                                    enumirable.Where(
                                        d => (Words.EqualsWord(d.User.Name) || Words.EqualsWord(d.User.Surname)))
                                        .Select(b => b.User) ?? null).ToList();
                else
                {
                    if (IsWork)
                        list =
                            DbHelper.Invoke.Users.Where(
                                x => x.IsWork == true && (Words.EqualsWord(x.Name) || Words.EqualsWord(x.Surname)))
                                .ToList();
                    else
                        list =
                            DbHelper.Invoke.Users.Where(x => (Words.EqualsWord(x.Name) || Words.EqualsWord(x.Surname)))
                                .ToList();
                }

                Users = new ObservableCollection<User>(list);
            }
        }

        #endregion

        #region CreateUser 

        private RelayCommand _createUserCommand;

        public ICommand ICreateUser
        {
            get
            {
                if (_createUserCommand == null)
                {
                    _createUserCommand = new RelayCommand(CreateUser);
                }
                return _createUserCommand;
            }
        }

        private void CreateUser()
        {
            Current = new User()
            {
                IsWork = true,
                Name = "Имя",
                Email = "test@t1twr.ru",
                MiddleName = "Отчество",
                Surname = "Фамилия",
                IdUser = Guid.NewGuid(),
                Comment = "Новый пользователь",
                Operation = Operation.Create,
                Password = "1234"
            };
           Current.ObjectHystory.ClearMemento();
        }

        #endregion

        #region SaveUser

        private RelayCommand _saveUserCommand;

        public ICommand ISaveUser
        {
            get
            {
                if (_searchUserCommand == null)
                {
                    _searchUserCommand = new RelayCommand(SaveUser);
                }
                return _searchUserCommand;
            }
        }

        private async void SaveUser()
        {
            try
            {
                if (EntityValidate.CostumValidator(Current))
                {
                    Users.Add(Current);
                    DbHelper.Invoke.Users.Add(Current);
                    await DbHelper.Invoke.SaveChangesAsync();
                }
                else
                    MessageBox.Show("Корректно заполните инфорамцию о пользователе", "Информация о пользователе",
                        MessageBoxButton.OK, MessageBoxImage.Stop);
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Данные не сохранены:{1} {0}",ex.Message,Environment.NewLine), "Информация о пользователе, ошибка выполнения операции", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        #endregion

        #region template

        #endregion


        private string[] Words
        {
            get { return SearchString.Split(' '); }
        }

        private User HeadUser { get; set; }
        public string SearchString { get; set; }
        public bool IsWork = true;
        public ObservableCollection<User> Users { get; set; }

        private User _user;
        public User Current
        {
            get { return _user; }
            set
            {
                    _user = value;
                    RaisePropertyChanged("Current");
            }
        }
    }
}