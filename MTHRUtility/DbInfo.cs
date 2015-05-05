using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTHRUtility
{
    public class DbInfo:IUtility
    {
        private KeyAndValues keyAndValues;

        public DbInfo(KeyAndValues keyAndValues)
        {
            this.keyAndValues = keyAndValues;
        }
        public void Execute()
        {
            Console.WriteLine("Not impliment");
          //  Console.WriteLine("The utility {0} is executed with parameters {1}", keyAndValues.Key, keyAndValues.Values.Aggregate((current, next) => current + ", " + next));
        }
    }
}
