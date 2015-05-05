using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseType;

namespace WebMTHR.Models
{
    public class WebDbHelper
    {
        private static string _mthrConnectionString;

        public static string MthrConnectionString
        {
            get
            {
                if(string.IsNullOrEmpty(_mthrConnectionString))
                    _mthrConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MTHRData"].ConnectionString;
                return _mthrConnectionString;
            }

        }
        public static ApplicationDbContext GetDbContext()
        {
            return new ApplicationDbContext();
        }
    }
}