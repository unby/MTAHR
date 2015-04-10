using System.Reflection;
using System.Windows.Data;

namespace ManagementGui.View.TreeViewUserAndTasks.Common
{
public class HierarchyItemViewModel : ViewModelBase
{
    private CollectionView _children;
    private bool _isExpanded;
    private bool _isSelected;

    public HierarchyItemViewModel(object dataItem)
    {
        DataItem = dataItem;
    }

    public object DataItem { get; private set; }

    public HierarchyItemViewModel Parent { get; set; }

    public bool IsExpanded
    {
        get { return _isExpanded; }
        set
        {
            _isExpanded = value;
            RaisePropertyChanged(MethodBase.GetCurrentMethod().GetPropertyName());
        }
    }

    public bool IsSelected
    {
        get { return _isSelected; }
        set
        {
            _isSelected = value;
            RaisePropertyChanged(MethodBase.GetCurrentMethod().GetPropertyName());
        }
    }

    public System.Windows.Data.CollectionView Children
    {
        get { return _children; }
        set { _children = value; }
    }

}
}
