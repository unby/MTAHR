using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseType;

namespace ManagementGui.Utils
{
    internal static class DbHelper
    {
        private static ApplicationDbContext dataContext;

        public static ApplicationDbContext GetDbProvider {
            get { return dataContext; }
        }

        private static string _connectionString;
        public static void Init(string connStr)
        {
            _connectionString = connStr;
            dataContext=new ApplicationDbContext(_connectionString);
        }
    }
}
