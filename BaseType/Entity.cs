using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseType
{
    [Serializable]
    public class WorkGroup
    {
        [Key]
        public Guid IdGroup { get; set; }

        public string Comment { get; set; }
        public string Name { get; set; }
        public List<UserRole> Users { get; set; }
    }

    [Serializable]
    public class UserRole
    {
        // [Key, Column (Order = 0)]
        [Key]
        public Guid IdUserRole { get; set; }
        public User User { get; set; }
        // [Key, Column(Order = 1)]
        public WorkGroup Group { get; set; }

        public Role Role { get; set; }
    }

    [Serializable]
    public enum Role
    {
        Admin,
        Distribution,
        User
    }
}
