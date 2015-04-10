using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementGui.MainWindowView.Model
{
    public class TVUser
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        
        public override string ToString()
        {
            return Name;
        }
    }
}
