using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseType;

namespace WebMTHR.Models
{
    public class WebDbHelper
    {
        public static ApplicationDbContext GetDbContext()
        {
            return new ApplicationDbContext();
        }
    }
}