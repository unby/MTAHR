using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseType;

namespace ManagementGui.ViewModel.Report
{
    public class UserTaskReportModelTemplate
    {
        public string FIO { get; set; }
        public string NameTask { get; set; }
        public int Weight { get; set; }
        public StatusTask Status { get; set; }
    }
}
