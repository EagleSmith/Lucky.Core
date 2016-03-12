using System;
using System.Collections.Generic;

namespace Lucky.Hr.Entity
{
    public partial class DistributorConfig
    {
        public int DistributorId { get; set; }
        public int PageSize { get; set; }
        public string Logo { get; set; }
        public virtual Distributor Distributor { get; set; }
    }
}
