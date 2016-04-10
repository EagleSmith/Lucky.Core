using System;
using System.Collections.Generic;

namespace Lucky.Entity
{
    public partial class RoleNav
    {
        public string RoleId { get; set; }
        public string NavId { get; set; }
        public int OperationId { get; set; }
        public virtual Nav Nav { get; set; }
        public virtual Operation Operation { get; set; }
        public virtual Role Role { get; set; }
    }
}
