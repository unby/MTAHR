using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseType;
using BaseType.Common;
using BaseType.Utils;
using ManagementGui.MainWindowView.Model;

namespace ManagementGui.MainWindowView.ViewModel
{
    public class UsersViewModel
    {
        public ObservableCollection<TVUser> Users { get; set; }
        private NotifyObservableCollection<ApplicationUser> _users;
        public UsersViewModel(NotifyObservableCollection<ApplicationUser> users)
        {
            _users = users;
            _users.ItemChanged=ItemChanged;
            _users.NotifyItemUpdate += _users_NotifyItemUpdate;
            Users=new ObservableCollection<TVUser>();
        }

        void _users_NotifyItemUpdate(object sender, NotifyItemUpdate e)
        {
            if (e.PropertyName == "Name" || e.PropertyName == "Surname" || e.PropertyName == "MiddleName")
            {
                ApplicationUser updItem = e.Item as ApplicationUser;
                if (updItem != null)
                {
                    foreach (var tvUser in Users.Where(w => w.UserId == updItem.Id))
                    {
                        tvUser.Name = updItem.UserShortName();
                    }
                }
            }
        }

        private void ItemChanged()
        {
        }

        void _users_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var item in e.NewItems)
                {
                    var addItem = item as ApplicationUser;
                    if (addItem != null)
                    {
                       Users.Add(new TVUser(){Description = addItem.UserShortNameAndPost(),UserId = addItem.Id,Name = addItem.UserShortName()});
                    }
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var item in e.OldItems)
                {
                    var removeItem = item as ApplicationUser;
                    if (removeItem != null)
                    {
                        Users.Remove(Users.FirstOrDefault(w => w.UserId == removeItem.Id));
                    }
                }
            }
        }
    }
}
