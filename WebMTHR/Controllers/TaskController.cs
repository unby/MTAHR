using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BaseType;
using BaseType.Utils;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;
using NLog;
using WebMTHR.Models;
using WebMTHR.Utils;
using Task = BaseType.Task;

namespace WebMTHR.Controllers
{
    public class TaskController : Controller
    {
        private Logger log = LogManager.GetCurrentClassLogger();
        public ApplicationUserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            private set { _userManager = value; }
        }

        private readonly ApplicationDbContext _dbContext = WebDbHelper.GetDbContext();
        private ApplicationUserManager _userManager;

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var listTask = new ListTask();
            Guid currentUserId = Guid.Parse(User.Identity.GetUserId());
            listTask.ShortTasks = await (from tasks in _dbContext.Tasks
                join userMembers in
                      _dbContext.TaskMembers.Where(w => w.IdUser == currentUserId) on
                    tasks.IdTask equals userMembers.IdTask
                select
                    new ShortTask()
                    {
                        IdTask = tasks.IdTask,
                        LastUpdate = tasks.DateUpdate,
                        Name = tasks.NameTask,
                        Status = tasks.Status
                    }).ToListAsync();
            return View(listTask);
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> OpenTask()
        {
            try
            {
                var view = new TaskViewModels();
                var idTask = Guid.Parse(Request.QueryString["idTask"]);
                var currentUserId = Guid.Parse(User.Identity.GetUserId());
                TaskMembers member = await ExtendetMethods.GetTaskMemberkEntityAsync(_dbContext, idTask, currentUserId);
                if (member == null)
                    return await Index();

                if (member.Participation != StatusParticipation.ToAccept ||
                    member.Participation != StatusParticipation.ItIsAppointed)
                    return  InitTask(member);
                view.AuthorName = member.User.UserName;
                view.Task = member.Task;
                view.Comments = member.Task.TaskComments.OrderByDescending(o => o.DateMessage).ToList();
               
                return View(view);
            }
            catch (Exception ex)
            {
                log.ErrorException("Ошибка открытия задачи",ex);
                return new HttpStatusCodeResult(500);
            }
        }
        [Authorize]
        private ActionResult InitTask(TaskMembers memberModel)
        {
            return View(memberModel);

        }

        [Authorize]
        [HttpPost]
        private async Task<ActionResult> AcceptTask(TaskMembers memberModel)
        {
            try
            {
                var view = new TaskViewModels();
                var idTask = Guid.Parse(Request.QueryString["idTask"]);
                var currentUserId = Guid.Parse(User.Identity.GetUserId());
                TaskMembers member = await ExtendetMethods.GetTaskMemberkEntityAsync(_dbContext, idTask, currentUserId);
                if (member == null)
                    return await Index();
                member.Participation = memberModel.Participation;
                member.Comment = memberModel.Comment;
                _dbContext.TaskMembers.AddOrUpdate(member);
                await _dbContext.SaveChangesAsync();
                if (member.Participation != StatusParticipation.ToAccept ||
                    member.Participation != StatusParticipation.ItIsAppointed)
                    return InitTask(member);               
                view.AuthorName = member.User.UserName;
                view.Task = member.Task;
                view.Comments = member.Task.TaskComments.OrderByDescending(o => o.DateMessage).ToList();

                return View(view);
            }
            catch (Exception ex)
            {
                log.ErrorException("Ошибка открытия задачи", ex);
                return new HttpStatusCodeResult(500);
            }
        }


        [Authorize]
         [HttpPost]
         public async Task<ActionResult> UpdateComment(TaskViewModels view)
         {
             try
             {
                 var resolveRequest = HttpContext.Request;
                 resolveRequest.InputStream.Seek(0, SeekOrigin.Begin);
                 string jsontextString = new StreamReader(resolveRequest.InputStream).ReadToEnd();
                 var jsonResponse = JsonConvert.DeserializeObject<dynamic>(jsontextString);

                 string updComment = jsonResponse["NewComment"];
                 var idTask = Guid.Parse(Request.QueryString["idTask"]);
                 var currentUserId = Guid.Parse(User.Identity.GetUserId());

                 var task = await ExtendetMethods.GetTaskEntityAsync(_dbContext, idTask, currentUserId);
                 if (task == null) throw new NullReferenceException("Задача не найдена");
                 if (string.IsNullOrEmpty(view.NewComment)) throw new ArgumentException("Введите данные");
                 
                 view.Task = task;
                 view.NewComment = string.Empty;
                 view.Comments = task.TaskComments.OrderByDescending(de => de.DateMessage).ToList();
                 if (view.Comments[0].Author.Id == currentUserId)
                 {
                     var id = view.Comments[0].TaskCommentId;
                     view.Comments[0].Message = updComment;
                     var comment =
                         await
                             _dbContext.TaskComments.FirstOrDefaultAsync(
                                 f => f.TaskCommentId == id);
                     comment.Message = updComment;
                      _dbContext.TaskComments.AddOrUpdate(comment);
                     await _dbContext.SaveChangesAsync();

                 }
                 return View("OpenTask", view);
             }
             catch (Exception ex)
             {
                 log.ErrorException("Ошибка добавления комментария", ex);
                 return new HttpStatusCodeResult(500);
             }
         }


        [Authorize]
        [HttpPost]
        public async Task<ActionResult> PublishComment(TaskViewModels view)
        {
            try
            {
                var task =await ExtendetMethods.GetTaskEntityAsync(_dbContext, Request.QueryString["idTask"], User.Identity.GetUserId());
                if(task==null)throw new NullReferenceException("Задача не найдена");
                if (string.IsNullOrEmpty(view.NewComment)) throw new ArgumentException("Введите данные");
                var userid = this.GetUserId();
                var comment = new TaskComment()
                {
                    Author =
                        _dbContext.Users.First(f => f.Id == userid),
                    DateMessage = DateTime.Now,
                    Task = task,
                    Message = view.NewComment,
                    TaskCommentId = Guid.NewGuid()
                };
                
                _dbContext.TaskComments.Add(comment);
                await
                    System.Threading.Tasks.Task.Run(
                        () => { ChronicleMTHR.DbChronicle.NewComment(task, comment, _dbContext); });
                await _dbContext.SaveChangesAsync();
                view.Task = task;
                view.NewComment = string.Empty;
                view.Comments = task.TaskComments.OrderByDescending(o=>o.DateMessage).ToList();
                if (view.Comments[0].Author.Id == userid)
                    view.EditOld = true;
                return View("OpenTask",view);
            }
            catch (Exception ex)
            {
                log.ErrorException("Ошибка добавления комментрия", ex);
                return new HttpStatusCodeResult(500);
            }
        }
    }
}
