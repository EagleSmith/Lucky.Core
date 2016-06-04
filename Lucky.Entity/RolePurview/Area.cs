using System;
using System.Collections.Generic;

namespace Lucky.Entity
{
    public partial class Area
    {
        public Area()
        {
            this.ResumeAreas = new List<ResumeArea>();
        }

        public string AreaId { get; set; }
        public string ParentId { get; set; }
        public string AreaName { get; set; }
        public string FullName { get; set; }
        public int Layer { get; set; }
        public virtual ICollection<ResumeArea> ResumeAreas { get; set; }
    }
}
