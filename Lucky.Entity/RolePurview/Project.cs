using System;
using System.Collections.Generic;

namespace Lucky.Entity
{
    public partial class Project
    {
        public int ProjectId { get; set; }
        public int ResumeId { get; set; }
        public string ProjectName { get; set; }
        public System.DateTime StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
        public bool ProjectNow { get; set; }
        public string Duty { get; set; }
        public string ProjectIntro { get; set; }
        public string ProjectExperience { get; set; }
        public System.DateTime AddDate { get; set; }
        public virtual Resume Resume { get; set; }
    }
}
