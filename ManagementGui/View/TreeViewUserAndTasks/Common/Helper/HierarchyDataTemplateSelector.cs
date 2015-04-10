using System.Windows;
using System.Windows.Controls;
using BaseType;
using ManagementGui.View.TreeViewUserAndTasks.Model;

namespace ManagementGui.View.TreeViewUserAndTasks.Common.Helper
{
public class HierarchyDataTemplateSelector : DataTemplateSelector
{
    public override DataTemplate SelectTemplate(object item, DependencyObject container)
    {
        DataTemplate retval = null;
        FrameworkElement element = container as FrameworkElement;

        if (element != null && item != null && item is HierarchyItemViewModel)
        {
            HierarchyItemViewModel hierarchyItem = item as HierarchyItemViewModel;
            if (hierarchyItem.DataItem != null)
            {

                if (hierarchyItem.DataItem.GetType() == typeof(UserTrV))
                {
                    retval = element.FindResource("CustomerTemplate") as DataTemplate;
                }

                else if (hierarchyItem.DataItem.GetType() == typeof(TaskTrV))
                {
                    retval = element.FindResource("OrderTemplate") as DataTemplate;
                }

            }
        }

        return retval;
    }
}

}
