using System;
using System.Collections.Generic;

namespace Lucky.Hr.Entity
{
    public partial class Education
    {
        public int EducationId { get; set; }
        public int ResumeId { get; set; }
        public int EducationType { get; set; }
        public string SchoolName { get; set; }
        public System.DateTime StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
        public bool InSchool { get; set; }
        public int Degree { get; set; }
        public string Major { get; set; }
        public string Course { get; set; }
        public string Duty { get; set; }
        public string Cert { get; set; }
        public System.DateTime AddDate { get; set; }
        public virtual Resume Resume { get; set; }
    }
}
