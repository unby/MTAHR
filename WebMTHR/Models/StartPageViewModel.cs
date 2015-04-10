using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebMTHR.Models
{
    public class StartPageViewModel
    {
        public string Helper { get; set; }
        public List<Theme> Themes { get; set; }

        public StartPageViewModel()
        {
            Themes=new List<Theme>();
        }
    }

    public class Theme
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public string Link { get; set; }
    }
}