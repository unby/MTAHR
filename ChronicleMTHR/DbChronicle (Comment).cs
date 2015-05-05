using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseType;
using Task = System.Threading.Tasks.Task;

namespace ChronicleMTHR
{
    public partial class DbChronicle
    {
        public static void CommentUpdateRecord(ApplicationDbContext db, BaseType.TaskComment task)
        {

        }

        public static  Task NewComment(BaseType.Task task, TaskComment comment, ApplicationDbContext dbContext)
        {
            task.DateUpdate = DateTime.Now;
            task.TaskComments.Add(comment);
            dbContext.Tasks.AddOrUpdate(task);
            var autNotivication = new Notivication()
            {
                DateCreate = comment.DateMessage,
                Description =
                    string.Format("Пользователь {0} опубликовал сообщение в вашей задаче", comment.Author.UserName),
                IdNotivication = Guid.NewGuid(),
                From = comment.Author,
                IdTask = task.IdTask,
                NotivicationStatus = NotivicationStatus.Declared,
                Task = task,
                IdUserFrom = comment.Author.Id,
                IdUserTo = task.Author,
                TimeSend = DateTime.Now.AddMinutes(10),
                To = dbContext.Users.First(f=>f.Id==task.Author)
            };
            dbContext.Notivications.Add(autNotivication);
            return  Task.Run(() =>
            { dbContext.Notivications.AddRange(CreateNotificationForNewComment(comment, task));
            }); 
            
        }

        private static  IEnumerable<Notivication> CreateNotificationForNewComment(TaskComment comment, BaseType.Task task)
        {
            return task.WorkGroup.Where(w=>w.User!=comment.Author).Select(userTo => new Notivication()
            {
                DateCreate = comment.DateMessage, Description = string.Format("Пользователь {0} опубликовал сообщение", comment.Author.UserName), IdNotivication = Guid.NewGuid(), From = comment.Author, IdTask = task.IdTask, NotivicationStatus = NotivicationStatus.Declared, Task = task, IdUserFrom = comment.Author.Id, IdUserTo = userTo.IdUser, TimeSend = DateTime.Now.AddMinutes(10), To = userTo.User
            }).ToList();
        }
    }
}
