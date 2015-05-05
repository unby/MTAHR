using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseType
{
    [Serializable]
    public enum LevelNotivication
    {
        [Description("Нет")] None = 0,
        [Description("Низкий")] Low = 1,
        [Description("Средний")] Normal =4,
        [Description("Высокий")] High = 8
    }
    [Flags]
    [Serializable]
    public enum TaskRoles
    {
        [Description("Наблюдатель")]
        Observable = 4,
        [Description("Участник")]
        Participant=8,
        [Description("Инициатор")]
        Initiator=128
    }

    public enum StatusParticipation
    {
        [Description("Подвержден")]
        ToAccept=0,
        [Description("Отказано")]
        ToRefuse=1,
        [Description("Уведомлен")]
        ToNotify=2,
        [Description("Назначен")]
        ItIsAppointed=4
    }

    [Serializable]
    public class TaskMembers
    {
        public TaskMembers()
        {
            Participation=StatusParticipation.ToNotify;
            LevelNotivication=LevelNotivication.Normal;
            TaskRole=TaskRoles.Participant;
        }

        [Key, Column(Order = 0), ForeignKey("Task")]
        public Guid IdTask { get; set; }
        [Key, Column(Order = 1), ForeignKey("User")]
        public Guid IdUser { get; set; }
        public virtual Task Task { get; set; }
        public virtual ApplicationUser User { get; set; }
        public StatusParticipation Participation { get; set; }
        public LevelNotivication LevelNotivication { get; set; }
        public TaskRoles TaskRole { get; set; }
        [StringLength(80,ErrorMessage = "Превышена длина комментария, допустимая длина 80 символов")]
        public string Comment { get; set; }
    }
}
