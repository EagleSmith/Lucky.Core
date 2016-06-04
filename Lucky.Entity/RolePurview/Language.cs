using System;
using System.Collections.Generic;

namespace Lucky.Entity
{
    public partial class Language
    {
        public int LanguageId { get; set; }
        public int ResumeId { get; set; }
        public string LanguageType { get; set; }
        public string SkillLevel { get; set; }
        public string Cert { get; set; }
        public System.DateTime AddDate { get; set; }
        public virtual Resume Resume { get; set; }
    }
}
