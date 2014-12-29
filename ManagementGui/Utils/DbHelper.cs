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
        private static MthrData dataContext;

        public static MthrData Invoke {
            get { return dataContext; }
        }

        public static void Init(string connStr)
        {
            dataContext = new MthrData(connStr);
        }
    }
}
