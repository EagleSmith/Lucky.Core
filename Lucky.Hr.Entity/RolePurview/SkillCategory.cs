using System;
using System.Collections.Generic;

namespace Lucky.Entity
{
    public partial class SkillCategory
    {
        public string CategoryId { get; set; }
        public string ParentId { get; set; }
        public string CategoryName { get; set; }
        public int Layer { get; set; }
        public int Sort { get; set; }
    }
}
