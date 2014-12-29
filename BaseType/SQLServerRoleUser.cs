using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseType
{
    public class SQLServerRoleUser
    {
        public string UserName { get; set; }
        public string RoleName { get; set; }
        public string LoginName { get; set; }
        public string DefDBName { get; set; }
        public string DefSchemaName { get; set; }
        public string UserId { get; set; }
        public byte[] SID { get; set; }
    }
}
