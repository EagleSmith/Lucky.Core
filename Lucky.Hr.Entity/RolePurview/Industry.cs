using System;
using System.Collections.Generic;

namespace Lucky.Entity
{
    public partial class Industry
    {
        public Industry()
        {
            this.ResumeIndustries = new List<ResumeIndustry>();
        }

        public string IndustryId { get; set; }
        public string ParentId { get; set; }
        public string IndustryName { get; set; }
        public int Sort { get; set; }
        public int Layer { get; set; }
        public virtual ICollection<ResumeIndustry> ResumeIndustries { get; set; }
    }
}
