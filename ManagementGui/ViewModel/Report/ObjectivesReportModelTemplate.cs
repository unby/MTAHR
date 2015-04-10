using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace ManagementGui.ViewModel.Report
{
    public class ObjectivesReportModelTemplate
    {
        public string FIO { get; set; }
        public int CountTask { get; set; }
        public int CountTaskComplite { get; set; }
        public int CountTaskRun { get; set; }
    }
}
