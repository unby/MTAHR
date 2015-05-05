using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseType.Common;
using GalaSoft.MvvmLight;
using ManagementGui.Config;

namespace ManagementGui.ViewModel.Menu
{
    public class NewFilesViewModel : ViewModelBase
    {
        public NewFilesViewModel() { }
        public NotifyObservableCollection<NewFile> NewFiles
        {
            get { return WorkEnviroment.NewFiles; }
        }
    }
}
