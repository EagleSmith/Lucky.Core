using System;
using System.Collections.Generic;

namespace Lucky.Hr.Entity
{
    public partial class ResumeIndustry
    {
        public int Id { get; set; }
        public int ResumeId { get; set; }
        public string IndustryId { get; set; }
        public virtual Industry Industry { get; set; }
        public virtual Resume Resume { get; set; }
    }
}
