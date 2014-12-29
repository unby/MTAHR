using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BaseType
{
    [Serializable]
    public class WorkProfile
    {
        public Guid IdUser { get; set; }
        public Guid IdProfile { get; set; }

    }
    [Serializable]
    public class UserSkill
    {
        public Guid IdSkill { get; set; }
        public string Name { get; set; }
        public DateTime DateCreate { get; set; }
    }
}
