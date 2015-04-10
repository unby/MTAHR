using BaseType;

namespace ManagementGui.ViewModel.Report
{
    public class WeightOfTasksReportModelTemplate
    {
        public string TaskName { get; set; }
        public int Weight { get; set; }
        public StatusTask Status { get; set; }
    }
}
