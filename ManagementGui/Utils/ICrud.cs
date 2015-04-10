using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ManagementGui.Utils
{
    interface ICrud
    {

        ICommand CreateCommand { get; }
        ICommand DeleteCommand { get; }
        ICommand UpdateCommand { get; }
        ICommand UpdateAllCommand { get; }
    }
}
