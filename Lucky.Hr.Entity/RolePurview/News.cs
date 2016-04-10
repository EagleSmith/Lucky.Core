using System;
using System.Collections.Generic;

namespace Lucky.Entity
{
    public partial class News
    {
        public int NewsId { get; set; }
        public int NewsTypeId { get; set; }
        public int DistributorId { get; set; }
        public string NewsTitle { get; set; }
        public string Image { get; set; }
        public string NewsContent { get; set; }
        public int Praise { get; set; }
        public int Reply { get; set; }
        public int AddManagerId { get; set; }
        public string AddFullName { get; set; }
        public System.DateTime AddDate { get; set; }
        public virtual NewsType NewsType { get; set; }
    }
}
