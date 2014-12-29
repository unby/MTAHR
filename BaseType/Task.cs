using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseType
{
    [Serializable]
    public class Task
    {
        [Required]
        public Guid Author { get; set; }
        [Key][Required]
        public Guid IdTask { get; set; }
        public DateTime DateCreate { get; set; }
        public DateTime DateFinish { get; set; }
        public DateTime DateUpdate { get; set; }
        public DateTime DateFisrtNotification { get; set; }
        public DateTime DateLastNotification { get; set; }
        public DateTime DateEndNotofication { get; set; }
        public DateTime DateClose { get; set; }
        [Required]
        public string NameTask { get; set; }
        public string Result { get; set; }
        public string Description { get; set; }
        public string Comment { get; set; }
        public Guid ParentTask { get; set; }
        [Required]
        public User Performer { get; set; }
        public StatusTask Status { get; set; }
        public IObservable<TaskComment> CommentsToTaskObservable { get; set; }
    }

    public enum StatusTask
    {
        Complete,
        Open,
        Suspended,
        Cancelled,
        Response,
        Request
    }
[Serializable]
    public class AppJurnal
    {
        
        public Guid IdTask { get; set; }
        [Key]
        public Guid IdEntry { get; set; }
        [Required]
        public DateTime DateEntry { get; set; }
        [Required]
        public string Message { get; set; }

    public MessageType MessageType { get; set; }
    public int MessageCode { get; set; }
    }
    [Serializable]
    public class TaskComment
{
        [Key,Column(Order = 0)]
        public Guid IdComment { get; set; }
        [Key, Column(Order = 1)]
        public Guid IdTask { get; set; }
        public string Message { get; set; }
        public User AuthorUser { get; set; }
        public DateTime DateMessage { get; set; }
}
}
