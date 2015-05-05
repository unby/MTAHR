using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BaseType;
using Microsoft.AspNet.Identity;

namespace WebMTHR.Utils
{
    public static class ExtendetMethods
    {
        public static Guid GetUserId(this Controller controller)
        {
            return Guid.Parse(controller.User.Identity.GetUserId());
        }

        public async static Task<ApplicationUser> GetUserEntityAsync(this Controller controller,ApplicationDbContext context)
        {
            var userId = controller.GetUserId();
            return await context.Users.FirstOrDefaultAsync(f => f.Id == userId);
        }

        public async static Task<ApplicationUser> GetUserEntityAsync(this Controller controller, ApplicationDbContext context,Guid id)
        {
            return await context.Users.FirstOrDefaultAsync(f => f.Id == id);
        }
        public async static Task<ApplicationUser> GetUserEntityAsync(this Controller controller, ApplicationDbContext context, string id)
        {
            var userId = Guid.Parse(id);
            return await context.Users.FirstOrDefaultAsync(f => f.Id == userId);
        }
        public static async Task<BaseType.TaskMembers> GetTaskMemberkEntityAsync(ApplicationDbContext context, Guid idTask, Guid idUser)
        {
            var query = from userMembers in context.TaskMembers.Where(w => w.IdUser == idUser && w.IdTask == idTask)
                        select userMembers;
            return await query.FirstOrDefaultAsync();
        }
        public static async Task<BaseType.Task> GetTaskEntityAsync(ApplicationDbContext context,Guid idTask,Guid idUser)
        {
            var query = from tasks in context.Tasks.Where(f => f.IdTask == idTask)
                        join userMembers in context.TaskMembers.Where(w => w.IdUser == idUser)
                        on tasks.IdTask equals userMembers.IdTask
                        select tasks;
            return await query.FirstOrDefaultAsync();
        }
        public static async Task<BaseType.Task> GetTaskEntityAsync(ApplicationDbContext context, string idTask, string idUser)
        {
            var idTaskGuid = Guid.Parse(idTask);
            var idUserGuid = Guid.Parse(idUser);
            var query = from tasks in context.Tasks.Where(f => f.IdTask == idTaskGuid)
                        join userMembers in context.TaskMembers.Where(w => w.IdUser == idUserGuid)
                        on tasks.IdTask equals userMembers.IdTask
                        select tasks;
            return await query.FirstOrDefaultAsync();
        }
    }
}