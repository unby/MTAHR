using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseType.Common
{
    public class EnumItem <T>
    {
        public T Code { get; set; }
        public string Description { get; set; }
        public string Display { get; set; }
        public string OriginalName { get; set; }
        public override string ToString()
        {
            return Description;
        }
    }
}
