using System;
using System.Collections.Generic;

namespace Lucky.Hr.Entity
{
    public partial class LanguageCategory
    {
        public string CategoryId { get; set; }
        public string ParentId { get; set; }
        public string CategoryName { get; set; }
        public int Layer { get; set; }
        public int Sort { get; set; }
    }
}
