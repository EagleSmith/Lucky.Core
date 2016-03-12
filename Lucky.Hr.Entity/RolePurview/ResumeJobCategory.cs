using System;
using System.Collections.Generic;

namespace Lucky.Hr.Entity
{
    public partial class ResumeJobCategory
    {
        public int Id { get; set; }
        public int ResumeId { get; set; }
        public string CategoryId { get; set; }
        public virtual JobCategory JobCategory { get; set; }
        public virtual Resume Resume { get; set; }
    }
}
