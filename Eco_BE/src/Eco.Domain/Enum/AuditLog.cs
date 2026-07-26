using System;
using System.Collections.Generic;
using System.Text;

namespace Eco.Domain.Enum
{
    public class AuditLog
    {
        public enum AuditAction
        {
            Create = 0,
            Update = 1,
            Delete = 2,
            Login = 3,
            Logout = 4,
            Export = 5,
            Import = 6
        }
    }
}
