using System;
using System.Collections.Generic;

namespace Lucky.Entity
{
    public partial class Work
    {
        public int WorkId { get; set; }
        public int ResumeId { get; set; }
        public string CompanyName { get; set; }
        public int CompanyProperty { get; set; }
        public int CompanySize { get; set; }
        public string IndustryId { get; set; }
        public System.DateTime StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
        public bool WorkNow { get; set; }
        public string JobName { get; set; }
        public int JobType { get; set; }
        public string CategoryId { get; set; }
        public int Joblevel { get; set; }
        public string ManageDempartment { get; set; }
        public int SubordinateSize { get; set; }
        public string ReportMan { get; set; }
        public decimal Salary { get; set; }
        public bool SalarySecrecy { get; set; }
        public string WorkContent { get; set; }
        public string LeaveReason { get; set; }
        public System.DateTime AddDate { get; set; }
        public virtual Resume Resume { get; set; }
    }
}
