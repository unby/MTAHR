using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using BaseType.Utils;
using ManagementGui.MainWindowView.ViewModel;
using ManagementGui.Utils;

namespace ManagementGui.View.Document
{
    public interface ITreeNodeNotivicationAndUser
    {
        string Name { get; }
        string Description { get; }
    }

    public class UserTreeViewModel : TreeViewItemViewModel, ITreeNodeNotivicationAndUser
    {
        public BaseType.ApplicationUser applicationUser { get; set; }
        public Guid TaskId { get; set; }

        public UserTreeViewModel(BaseType.ApplicationUser applicationUser, Guid taskId) : base(null, true)
        {
            this.applicationUser = applicationUser;
            TaskId = taskId;
        }

        protected override void LoadChildren()
        {
            var temp =
                DbHelper.GetDbProvider.Notivications.Where(w => w.IdUserTo == applicationUser.Id && w.IdTask == TaskId);
            foreach (var item in temp )
                base.Children.Add(new NotivicationTreeViewModel(item, this));
        }

        public string Name
        {
            get { return applicationUser.UserFullName(); }
        }

        public string Description
        {
            get { return applicationUser.Comment; }
        }

        internal void AddUser(BaseType.Notivication notivication)
        {
            base.Children.Add(new NotivicationTreeViewModel(notivication, this));
        }
    }
}
