using System;
using System.Collections.Generic;
using System.Text;

namespace Eco.Domain.Enum
{
    public class AuditLog
    {
        public enum AuditAction
        {
            Create = 0, // tạo
            Update = 1, // cập nhật
            Delete = 2, // xóa
            Login = 3, // đăng nhập
            Logout = 4, // đăng xuất
            Export = 5, // xuất
            Import = 6 // nhập
        }
    }
}
