using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseType;

namespace MTHRUtility
{
    class Program
    {
        static ConsoleFactory Param =new ConsoleFactory();
        static void Main(string[] args)
        {
            if(Param.Init(args))
            Param.Execute();
        }
    }
}
