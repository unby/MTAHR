using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseType;
using BaseType.Migrations;

namespace MTHRUtility
{
    class Program
    {
        static ConsoleFactory Param =new ConsoleFactory();
        static void Main(string[] args)
        {
            Database.SetInitializer<ApplicationDbContext>(new MigrateDatabaseToLatestVersion<ApplicationDbContext, Configuration>());
            //if(Param.Init(args))
            //Param.Execute();
            Console.ReadLine();
        }
    }
}
