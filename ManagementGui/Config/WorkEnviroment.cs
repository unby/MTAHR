using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseType;

namespace ManagementGui.Config
{
    public static class WorkEnviroment
    {
        public static User HeadMan { get; set; }
        public static Project CurrentProject { get; set; }
        public static bool IsModerator { get; set; }
    }
}
