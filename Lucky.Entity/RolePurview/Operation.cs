using System;
using System.Collections.Generic;

namespace Lucky.Entity
{
    public partial class Operation
    {
        public Operation()
        {
            this.NavOperations = new List<NavOperation>();
            this.RoleNavs = new List<RoleNav>();
        }

        public int OperationId { get; set; }
        public string OperationName { get; set; }
        public string SystemName { get; set; }
        public int Sort { get; set; }
        public long OperationValue { get; set; }
        public virtual ICollection<NavOperation> NavOperations { get; set; }
        public virtual ICollection<RoleNav> RoleNavs { get; set; }
    }
}
