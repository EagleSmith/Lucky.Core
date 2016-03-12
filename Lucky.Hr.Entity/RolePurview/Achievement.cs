using System;
using System.Collections.Generic;

namespace Lucky.Hr.Entity
{
    public partial class Achievement
    {
        public int AchievementId { get; set; }
        public int ResumeId { get; set; }
        public string Title { get; set; }
        public string Intro { get; set; }
        public string Photo { get; set; }
        public System.DateTime AddDate { get; set; }
        public virtual Resume Resume { get; set; }
    }
}
