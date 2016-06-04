using System;
using System.Collections.Generic;

namespace Lucky.Entity
{
    public partial class Cert
    {
        public int CertId { get; set; }
        public int ResumeId { get; set; }
        public string Cert1 { get; set; }
        public System.DateTime GetDate { get; set; }
        public System.DateTime AddDate { get; set; }
        public virtual Resume Resume { get; set; }
    }
}
