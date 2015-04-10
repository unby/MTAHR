using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
using BaseType;

namespace ManagementGui.ViewModel.Report
{
    public class ReportTaskModelTemplate
    {
        [DisplayName(@"ФИО")]
        public string FIO { get; set; }
        [DisplayName(@"Задача")]
        public string TaskName { get; set; }
        [DisplayName(@"Дата создания")]
        public DateTime TaskCreate { get; set; }
        [DisplayName(@"Статус")]
        public StatusTask Status { get; set; }
        [DisplayName(@"Последенние изменение")]
        public DateTime TaskUpdate { get; set; }
        [DisplayName(@"Комментарий")]
        public string Comment { get; set; }
    }
}
