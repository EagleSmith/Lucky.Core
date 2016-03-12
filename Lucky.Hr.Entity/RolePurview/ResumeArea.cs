using System;
using System.Collections.Generic;

namespace Lucky.Hr.Entity
{
    public partial class ResumeArea
    {
        public int Id { get; set; }
        public int ResumeId { get; set; }
        public string AreaId { get; set; }
        public virtual Area Area { get; set; }
        public virtual Resume Resume { get; set; }
    }
}
