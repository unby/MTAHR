using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BaseType
{

    [Serializable]
    public class Member
    {
        [Key, Column(Order = 1), ForeignKey("User")]
        public Guid IdUser { get; set; }
       // [Key,]
        [Key, Column(Order = 0), ForeignKey("Project")]
        public Guid IdProject { get; set; }
        public virtual Project Project { get; set; }
        public virtual ApplicationUser User { get; set; }
        public Role Role { get; set; }
        [MaxLength(120)]
        public string Comment { get; set; }
    }

    [Serializable]
    public enum TypeProject
    {
        [Description("Подразделение")]
        Divison,
        [Description("Студенческий проект")]
        EducationProject,
        [Description("Совместный проект")]
        Collaboration
    }

    [Serializable]
    public enum Role
    {
        [Description("Администратор информационной системы")]
        Admin,
        [Description("Руководитель проекта")]
        Distribution,
        [Description("Исполнитель")]
        User
    }
}
