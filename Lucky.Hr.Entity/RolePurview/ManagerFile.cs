using System;
using System.Collections.Generic;

namespace Lucky.Hr.Entity
{
    public partial class ManagerFile
    {
        public int ManagerFileId { get; set; }
        public int DistributorId { get; set; }
        public string FileType { get; set; }
        public string FileName { get; set; }
        public string ManagerId { get; set; }
        public string FullName { get; set; }
        public System.DateTime AddDate { get; set; }
        public virtual Manager Manager { get; set; }
    }
}
