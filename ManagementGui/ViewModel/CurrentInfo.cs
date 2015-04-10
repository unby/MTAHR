using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementGui.ViewModel
{
    public class CurrentInfo
    {
        public CurrentInfo()
        {
            Name = "Данные не найдены";
            Common = 0;
            Opens = 0;
            Closed = 0;
        }

        public string Name { get; set; }
        public int Common { get; set; }
        public int Opens { get; set; }
        public int Closed { get; set; }

        public int CommonPercent { get { return 100; } }

        public decimal OpensPercent
        {
            get
            {
                if (Common == 0)
                    return 0;
                return 100*Opens/Common;
            }
        }

        public decimal ClosedPercent
        {
            get
            {
                if (Common == 0)
                    return 0;
                return 100*Closed/Common;
            }
        }
    }
}
