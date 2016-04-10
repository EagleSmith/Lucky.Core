using System;
using System.Collections.Generic;

namespace Lucky.Entity
{
    public partial class NavOperation
    {
        public int ID { get; set; }
        public string NavId { get; set; }
        public int OperationId { get; set; }
        public virtual Nav Nav { get; set; }
        public virtual Operation Operation { get; set; }
    }
}
