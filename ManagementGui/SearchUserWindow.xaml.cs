﻿using System;
using System.Data.Entity.Validation;
using System.IO;
using System.Text;
using System.Windows;
using BaseType;
using BaseType.Utils;
using MahApps.Metro.Controls;
using ManagementGui.Utils;
using ManagementGui.ViewModel;
using Microsoft.Win32;

namespace ManagementGui
{
    /// <summary>
    /// Логика взаимодействия для SearchUserWindow.xaml
    /// </summary>
    public partial class SearchUserWindow : MetroWindow
    {
        private User HeadMan=null;
        public SearchUserViewModel UserViewModel = null;
        public SearchUserWindow()
        {
            InitializeComponent();
            UserViewModel = new SearchUserViewModel();
            DataContext = UserViewModel;
        }


        public SearchUserWindow(User user)
        {
            InitializeComponent();
            UserViewModel=new SearchUserViewModel(user);
            DataContext = UserViewModel;
        }
          
        private async void ImportUuser_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                int countImportContacts = 0;
                int countErrorContacts = 0;
           
                FileDialog dialog = new OpenFileDialog();
                dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                dialog.Filter = "special import file (*.csv)|*.csv";
                dialog.ShowDialog();
                if (!string.IsNullOrEmpty(dialog.FileName))
                {
                    var sr = new StreamReader(dialog.FileName, Encoding.UTF8);
                    string line;
                    string[] row;
                    while ((line = sr.ReadLine()) != null)
                    {

                        row = line.Split(',');
                        if (row.Length > 1)
                        {
                            if (!string.IsNullOrEmpty(row[0]) && !string.IsNullOrEmpty(row[2]) && EntityValidate.IsEmail(row[3]))
                            {
                                DbHelper.Invoke.Users.Add(new User()
                                {
                                    IdUser = Guid.NewGuid(),
                                    Name = row[0],
                                    MiddleName = row[1],
                                    Surname = row[2],
                                    Email = row[3]
                                });
                                countImportContacts++;
                                continue;
                            }
                            else
                            {
                                countErrorContacts++;
                                continue;
                            }
                        }
                        row = line.Split(';');
                        if (row.Length > 1)
                        {
                            if (!string.IsNullOrEmpty(row[0]) && !string.IsNullOrEmpty(row[2]) &&
                                EntityValidate.IsEmail(row[3]))
                            {
                                DbHelper.Invoke.Users.Add(new User()
                                {
                                    IdUser = Guid.NewGuid(),
                                    Name = row[0],
                                    MiddleName = row[1],
                                    Surname = row[2],
                                    Email = row[3]
                                });
                                countImportContacts++;
                                continue;
                            }
                            else
                            {
                                countErrorContacts++;
                                continue;
                            }
                        }
                    }
                    if (countImportContacts > 0 || countErrorContacts > 0)
                    {
                        await DbHelper.Invoke.SaveChangesAsync();
                        MessageBox.Show(
                            string.Format("{0}{1}{2}",
                                countImportContacts > 0
                                    ? string.Format("Импортировано контактов {0}",
                                        countImportContacts)
                                    : "", (countImportContacts > 0 && countErrorContacts > 0) ? Environment.NewLine : "",
                                (countImportContacts > 0 && countErrorContacts > 0)
                                    ? string.Format("Некорректных записей в файле {0}", countErrorContacts)
                                    : ""
                                ), "Импорт контактов завершен", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                        MessageBox.Show(string.Format("Файл {0} содержит не коректную схему данных", dialog.FileName),
                            "Ошибка импорта данных",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
            catch(DbEntityValidationException ex)
            {
                MessageBox.Show(string.Format("Ошибка импорта списка контактов: {0}", ex),
                 "Ошибка импорта данных", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Ошибка импорта списка контактов: {0}", ex.Message),
                    "Ошибка импорта данных", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
 
        private void BtnSelectUser_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}