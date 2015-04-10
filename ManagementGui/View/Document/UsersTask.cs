using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseType;

namespace ManagementGui.View.Document
{
    public class UsersTaskViewModel
    {
        public List<UserTreeViewModel> Users { get; set; } 
        public UsersTaskViewModel(ICollection<TaskMembers> workGroup)
        {
            Users=new List<UserTreeViewModel>(); 
            foreach (TaskMembers members in workGroup)
            {
                Users.Add(new UserTreeViewModel(members.User,members.IdTask));    
            }
            
        }
    }
}
