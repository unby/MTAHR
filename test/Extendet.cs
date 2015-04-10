using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test
{
    public static class  Extendet
    {
        public static string RemoveEndString(this string str, string sample)
        {
            int index = str.LastIndexOf(sample, System.StringComparison.Ordinal);
            return str.Substring(0, index);
        }
    }
}
