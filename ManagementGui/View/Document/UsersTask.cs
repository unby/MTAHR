using System.Collections.Generic;
using BaseType;

namespace ManagementGui.View.Document
{
    public class UsersTaskViewModel
    {
        public List<UserTreeViewModel> Users { get; set; } 
        public UsersTaskViewModel(IEnumerable<TaskMembers> workGroup)
        {
            Users=new List<UserTreeViewModel>(); 
            foreach (var members in workGroup)
                Users.Add(new UserTreeViewModel(members,members.IdTask));                          
        }
    }
}
