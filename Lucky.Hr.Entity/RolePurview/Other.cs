using System;
using System.Collections.Generic;

namespace Lucky.Hr.Entity
{
    public partial class Other
    {
        public int OtherId { get; set; }
        public int ResumeId { get; set; }
        public string Title { get; set; }
        public string Intro { get; set; }
        public System.DateTime AddDate { get; set; }
        public virtual Resume Resume { get; set; }
    }
}
