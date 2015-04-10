using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementGui.ViewModel
{
    public interface IModelAction
    {
        void ModelSave();

    }

    public enum ActionModel
    {
        None,
        Save,
        Cancel
    }
}
