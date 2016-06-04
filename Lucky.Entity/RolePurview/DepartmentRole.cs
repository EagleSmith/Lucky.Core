using System;
using System.Collections.Generic;

namespace Lucky.Entity
{
    public partial class DepartmentRole
    {
        public int ID { get; set; }
        public string DepartmentId { get; set; }
        public string RoleId { get; set; }
        public virtual Department Department { get; set; }
        public virtual Role Role { get; set; }
    }
}
