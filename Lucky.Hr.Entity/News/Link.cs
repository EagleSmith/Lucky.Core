using System;
using System.Collections.Generic;

namespace Lucky.Hr.Entity
{
    public partial class Link
    {
        public System.Guid LinkID { get; set; }
        public string Title { get; set; }
        public string UserName { get; set; }
        public string UserTel { get; set; }
        public string UserEmail { get; set; }
        public bool IsImage { get; set; }
        public int DisplayOrder { get; set; }
        public string WebUrl { get; set; }
        public string ImageUrl { get; set; }
        public bool IsLock { get; set; }
        public System.DateTime CreateDate { get; set; }
    }
}
