using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseType.Common
{
    public enum LenghtMessage
    {
        [Description("Сокращенное описание")]
        Truncated,
        [Description("Общее описание")]
        Normal,
        [Description("Полное описание")]
        Full
    }
}
