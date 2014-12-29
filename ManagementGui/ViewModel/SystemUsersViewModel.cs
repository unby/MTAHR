using System.Collections.ObjectModel;
using System.Linq;
using BaseType;
using GalaSoft.MvvmLight;
using ManagementGui.Utils;

namespace ManagementGui.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class SystemUsersViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the SystemUsersViewModel class.
        /// </summary>
        public SystemUsersViewModel()
        {
           // Users = new ObservableCollection<User>(DbHelper.Invoke.Users);
            Projects=new ObservableCollection<Project>(DbHelper.Invoke.Projects);
        }

        public User UserCurrent { get; set; }
        public Project ProjectCurrent { get; set; }

      //  public ObservableCollection<User> Users { get; set; }

        public ObservableCollection<Project> Projects { get; set; }
    }
}