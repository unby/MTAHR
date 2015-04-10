using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Core.Common.CommandTrees;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BaseType.Utils;

namespace BaseType.Security
{
   
    [Flags]
    public enum Authorization
    {
        [Description("Windows AD авторизация")]
        OAuth = 1,
        [Description("MS SQL Server авторизация")]
        MsSqlServer = 2,
        [Description("OAuth")]
        WindowsAd = 4,
        [Description("Внутренняя авторизация")]
        Application = 8
    }
}
