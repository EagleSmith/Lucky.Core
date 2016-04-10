using System;
using System.Collections.Generic;

namespace Lucky.Entity
{
    public partial class Skill
    {
        public int SkillId { get; set; }
        public int ResumeId { get; set; }
        public string SkillName { get; set; }
        public string SkillLevel { get; set; }
        public System.DateTime AddDate { get; set; }
        public virtual Resume Resume { get; set; }
    }
}
