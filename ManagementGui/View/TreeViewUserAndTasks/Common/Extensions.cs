using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;

namespace ManagementGui.View.TreeViewUserAndTasks.Common
{
    public static class Extensions
    {
        /// <summary>
        /// Entension method for an ObservableCollection<T> to repopulate from a List<T>
        /// </summary>
        public static void RepopulateFromList<T>(this ObservableCollection<T> observableCollection, IEnumerable<T> items)
        {
            observableCollection.Clear();
            foreach (T item in items)
            {
                observableCollection.Add(item);
            }
        }

        /// <summary>
        /// Extension method to trim off the set_, get_ from the front of the method name for a property
        /// </summary>
        /// <param name="methodBase"></param>
        /// <returns></returns>
        public static string GetPropertyName(this MethodBase methodBase)
        {
            return methodBase.Name.Substring(4);
        }

    }
}
