using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementGui.View.TreeViewUserAndTasks.Model
{
    interface ITreeEntity:INotifyPropertyChanged
    {
        Guid IdEntity { get; set; }
        string Display { get; set; }

        int Order { get; set; }
    }
}
