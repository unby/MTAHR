using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementGui.Utils
{
    public enum DialogResult
    {
        Ok,
        Cancel,
        No
    }

    class DialogResultEvent:EventArgs
    {
        private DialogResult Result { get; set; }

        public void SetResult(DialogResult result)
        {
            Result = result;
        }
    }
}
