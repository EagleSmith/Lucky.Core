using System;
using System.Collections.Generic;

namespace Lucky.Entity
{
    public partial class NewsType
    {
        public NewsType()
        {
            this.News = new List<News>();
        }

        public int NewsTypeId { get; set; }
        public int DistributorId { get; set; }
        public string TypeName { get; set; }
        public int Sort { get; set; }
        public virtual Distributor Distributor { get; set; }
        public virtual ICollection<News> News { get; set; }
    }
}
