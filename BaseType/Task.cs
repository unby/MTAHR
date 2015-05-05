using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BaseType
{
    [Serializable]
    public class Task
    {
        [Required]
        public Guid Author { get; set; }
        [Required]
        public Guid Project { get; set; }

        [Key][Required]
        public Guid IdTask { get; set; }
        public DateTime DateCreate { get; set; }
        public DateTime DateFinish { get; set; }
        public DateTime DateUpdate { get; set; }
        public DateTime? DateClose { get; set; }
        public int TaskRating { get; set; }
        [Required]
        [StringLength(160)]
        public string NameTask { get; set; }
        [StringLength(300)]
        public string Result { get; set; }
        [StringLength(700)]
        public string Description { get; set; }
        [StringLength(600)]
        public string Comment { get; set; }
        public Guid ParentTask { get; set; }

        public StatusTask Status { get; set; }
        public virtual ICollection<WorkFile> Files { get; set; } 
        public virtual ICollection<TaskMembers> WorkGroup { get; set; }
        public virtual ICollection<Notivication> Notivications { get; set; }
        public virtual ICollection<TaskComment> TaskComments { get; set; }

        public override string ToString()
        {
            return NameTask;
        }
    }

    public enum StatusTask
    {
        [Description("Выполнена")]
        Complete =0,
        [Description("В работе")]
        Open =1,
        [Description("Приостановлена")]
        Suspended=2
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
        [Key]
        public Guid TaskCommentId { get; set; }
        [Required]
        public virtual Task Task { get; set; }
        [Required]
        public string Message { get; set; }
        [Required]
        public virtual ApplicationUser Author { get; set; }
        [Required]
        public DateTime DateMessage { get; set; }
}
}
