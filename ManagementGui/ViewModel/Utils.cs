using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ManagementGui.ViewModel
{
    public enum AuthenticationUser
    {
        [Description("Windows авторизация")]
        Windows,
        [Description("Авторизация MS Sql Server")]
        Integraten
    }
      
}