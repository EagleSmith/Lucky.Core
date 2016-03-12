using System;
using System.Collections.Generic;

namespace Lucky.Hr.Entity
{
    public partial class ManagerLog
    {
        public int ManagerLogId { get; set; }
        public int DistributorId { get; set; }
        public int NavId { get; set; }
        public int Operation { get; set; }
        public string Remark { get; set; }
        public string ManagerId { get; set; }
        public string FullName { get; set; }
        public System.DateTime AddDate { get; set; }
        public string IpAddress { get; set; }
        public virtual Manager Manager { get; set; }
    }
}
