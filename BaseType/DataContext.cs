using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using NLog.Internal;

namespace BaseType
{
    

    public class MthrData : DbContext
    {

        public MthrData()
            : base("Data Source=UNBY;Database=MTHRData;Initial Catalog=MTHRData;Integrated Security=True")
        {
        }

        public MthrData(string connString)
            : base(connString)
        {
        }

        public DbSet<Project> Projects { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<AppJurnal> AppJurnal { get; set; }
        public DbSet<TaskComment> TaskComments { get; set; }
        public DbSet<WorkGroup> WorkGroups { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

    }
}
