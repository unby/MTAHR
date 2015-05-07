using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using BaseType;
using BaseType.Utils;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ManagementGui.Utils;

namespace ManagementGui.Admin
{
    public class SearchUserViewModel : ViewModelBase
    {
        public ApplicationDbContext Context = DbHelper.GetDbProvider;
        public SearchUserViewModel()
        {
            Users = new ObservableCollection<ApplicationUser>(Context.Users.Take(25));
            Users.CollectionChanged +=Users_CollectionChanged;
        }

        private async void Users_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            
            Context.Users.AddOrUpdate(Users[e.NewStartingIndex]);
           // await Context.SaveChangesAsync();
        }

        public SearchUserViewModel(ApplicationUser headApplicationUser)
        {
            List<ApplicationUser> list = null;
            HeadApplicationUser = headApplicationUser;
            //if (IsWork)
            //    list = Context.Projects.Where(x => x.Author.Id == headApplicationUser.Id).
            //        Select(y => new List<Member>(y.UsersProject.Users).Where(v => v.Role == Role.User)).
            //        SelectMany(enumirable => enumirable.Where(d => d.User.IsWork == IsWork).Select(b => b.User) ?? null)
            //        .ToList();
            //else
            //    list = Context.Projects.Where(x => x.Author.Id == headApplicationUser.Id).
            //        Select(y => new List<Member>(y.UsersProject.Users).Where(v => v.Role == Role.User)).
            //        SelectMany(enumirable => enumirable.Select(b => b.User) ?? null).ToList();
            Users = new ObservableCollection<ApplicationUser>(list);
        }

        private RelayCommand _setEnterPoint;

        public ICommand SetEnterPoint
        {
            get
            {
                if (_setEnterPoint == null)
                {
                    _setEnterPoint=new RelayCommand(GetEnterPoint);
                }
                return _setEnterPoint;
            }
        }

        private void GetEnterPoint()
        {
           
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
                List<ApplicationUser> list = null;
                //if (headApplicationUser != null)
                //    if (IsWork)
                //        list = Context.Projects.Where(x => x.Author.Id == headApplicationUser.Id).
                //            Select(y => new List<Member>(y.UsersProject.Users).Where(v => v.Role == Role.User)).
                //            SelectMany(
                //                enumirable =>
                //                    enumirable.Where(
                //                        d =>
                //                            d.User.IsWork == IsWork &&
                //                            (Words.EqualsWord(d.User.Name) || Words.EqualsWord(d.User.Surname)))
                //                        .Select(b => b.User) ?? null).ToList();
                //    else
                //        list = Context.Projects.Where(x => x.Author.Id == headApplicationUser.Id).
                //            Select(y => new List<Member>(y.UsersProject.Users).Where(v => v.Role == Role.User)).
                //            SelectMany(
                //                enumirable =>
                //                    enumirable.Where(
                //                        d => (Words.EqualsWord(d.User.Name) || Words.EqualsWord(d.User.Surname)))
                //                        .Select(b => b.User) ?? null).ToList();
                
                    if (IsWork)
                        list =
                            Context.Users.Where(
                                x => x.IsWork == true && (Words.EqualsWord(x.Name) || Words.EqualsWord(x.Surname)))
                                .ToList();
                    else
                        list =
                            Context.Users.Where(x => (Words.EqualsWord(x.Name) || Words.EqualsWord(x.Surname)))
                                .ToList();
                

                Users = new ObservableCollection<ApplicationUser>(list);
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
            Current = new ApplicationUser()
            {
                IsWork = true,
                Name = "Имя",
                Email = "test@t1twr.ru",
                MiddleName = "Отчество",
                Surname = "Фамилия",
                Id = Guid.NewGuid(),
                Comment = "Новый пользователь"
            };
            Users.Add(Current);
            
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
                if (Current == null) return;
                if (EntityValidate.CostumValidator(Current))
                {
                    if (string.IsNullOrEmpty(Current.PasswordHash))
                    {
                        using (
                            var manager = new ApplicationUserManager(new ApplicationUserStore(DbHelper.GetDbProvider)))
                        {
                            await manager.CreateAsync(Current, Current.Id.ToByteArray().GetMd5());
                        }
                    }
                    else
                    {
                        Context.Users.AddOrUpdate(Current);
                        await Context.SaveChangesAsync();
                    }
                   
                }
                else
                    MessageBox.Show("Корректно заполните инфорамцию о пользователе", "Информация о пользователе",
                        MessageBoxButton.OK, MessageBoxImage.Stop);
            }
            catch (Exception ex)
            {
                Logger.MessageBoxException(ex);
            }
        }

        #endregion

        #region template

        #endregion

        private string[] Words
        {
            get { return SearchString.Split(' '); }
        }

        private ApplicationUser HeadApplicationUser { get; set; }
        public string SearchString { get; set; }

        public bool IsWork = true;
        public ObservableCollection<ApplicationUser> Users { get; set; }

        private ApplicationUser _applicationUser;
        public ApplicationUser Current
        {
            get { return _applicationUser; }
            set
            {
                    _applicationUser = value;
                    RaisePropertyChanged();
            }
        }
    }
}