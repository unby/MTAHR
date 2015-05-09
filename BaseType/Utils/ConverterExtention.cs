using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseType.Utils
{
    public static class ConverterExtention
    {
        public static string ConvertByteToStringSid(this Byte[] sidBytes)
        {
            if (sidBytes == null) return "";
            var sb = new StringBuilder();
            foreach (var b in sidBytes)
            {
                sb.Append(b.ToString("X1"));
            }
            return sb.ToString();
        }
    }
}
