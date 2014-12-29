using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseType;


namespace UtilsMTHR
{
    class Program
    {
        static void Main(string[] args)
        {
           // Console.WriteLine(DesktopSettings.Config.ConnectionSettings.ServerName);
            args = new string[] { "create" };
            MthrData dataContext = new MthrData();
            if (args.Any(x => x == "create"))
            {
                User adminUser = new User() { IdUser = Guid.NewGuid(), Email = "admin@localHost.ru", Password = "password", SystemRole = Role.Admin, Surname = "Admin", Name = "Admin", IsWork = true };
                dataContext.Users.Add(adminUser);

            }
            dataContext.SaveChanges();
        }
    }
}
