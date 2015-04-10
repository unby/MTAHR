using System.Data.Entity;
using System.Windows;
using BaseType;
using GalaSoft.MvvmLight.Threading;

namespace ManagementGui
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        static App()
        {

            Database.SetInitializer<ApplicationDbContext>(null);
           // DispatcherHelper.Initialize();
        }
       
    }
}
