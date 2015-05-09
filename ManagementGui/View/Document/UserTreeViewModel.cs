using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using BaseType;
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
        public TaskMembers MemberUser { get; set; }
        public Guid TaskId { get; set; }

        public UserTreeViewModel(TaskMembers memberUser, Guid taskId) : base(null, true)
        {
            this.MemberUser = memberUser;
            TaskId = taskId;
        }

        protected override void LoadChildren()
        {
            var temp =
                DbHelper.GetDbProvider.Notivications.Where(w => w.IdUserTo == MemberUser.User.Id && w.IdTask == TaskId);
            foreach (var item in temp )
                base.Children.Add(new NotivicationTreeViewModel(item, this));
        }

        public string Name
        {
            get { return MemberUser.User.UserName; }
        }

        public StatusParticipation Status
        {
            get { return MemberUser.Participation; }
            set
            {
                if(MemberUser.Participation==StatusParticipation.ToAccept)
                    return;
                if(value==StatusParticipation.ItIsAppointed||value==StatusParticipation.ToNotify)
                MemberUser.Participation = value;
            }
        }
        public string Description
        {
            get { return MemberUser.Comment; }
        }

        internal void AddUser(Notivication notivication)
        {
            base.Children.Add(new NotivicationTreeViewModel(notivication, this));
        }
    }
}
