using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BaseType;
using Task = BaseType.Task;

namespace WebMTHR.Models
{
    public class TaskViewModels
    {
        public Task Task { get; set; }
        public List<TaskComment> Comments { get; set; }
        public string AuthorName { get; set; }

        public TaskViewModels()
        {
            Comments=new List<TaskComment>();
        }
        [Required(ErrorMessage = @"Введите текст сообщения!")]

        public string NewComment { get; set; }

        public bool EditOld { get; set; }
    }

    public class ListTask
    {
        public ListTask()
        {
            ShortTasks=new List<ShortTask>();
        }

        public List<ShortTask> ShortTasks { get; set; }
    }

    public class ShortTask
    {
        public Guid IdTask { get; set; }
        public string Name { get; set; }

        public StatusTask Status { get; set; }
        public DateTime LastUpdate { get; set; }
    }
}