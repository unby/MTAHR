using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Windows.Media.Effects;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
namespace BaseType
{

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>
    {
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        public ApplicationDbContext()
            : base("MTHRData")
        {
        }

        public ApplicationDbContext(string connString)
            : base(connString)
        {
        }

        // public DbSet<User> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<AppJurnal> AppJurnal { get; set; }
        public DbSet<TaskComment> TaskComments { get; set; }
        public DbSet<TaskMembers> TaskMembers { get; set; }
        public DbSet<Member> UserRoles { get; set; }
        public DbSet<Notivication> Notivications { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Member>().HasKey(key => new {ProjectId = key.IdProject, UserId = key.Id}).HasMany(hm=>);
           
            //modelBuilder.Entity<Task>().HasRequired(her => her.WorkGroup).WithRequiredPrincipal(wrp => wrp.Task);
            //modelBuilder.Entity<WorkGroup>().HasRequired(her => her.Task).WithRequiredDependent(wrd => wrd.WorkGroup);
         
            //До измения (сделано  для автоматического мапинга ключей и виртуальных сущностей 
            // modelBuilder.Entity<Member>().HasKey(k => new { k.IdProject, k.IdUser });
            //  modelBuilder.Entity<TaskMembers>().HasKey(k => new { k.IdTask, k.IdUser });
            modelBuilder.Entity<Notivication>().HasOptional(g => g.From).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<Notivication>().HasOptional(g => g.To).WithMany().WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                    .SelectMany(x => x.ValidationErrors)
                    .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                // Throw a new DbEntityValidationException with the improved exception message.
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);

            }
        }
    }


}
