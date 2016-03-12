using System;
using System.Collections.Generic;

namespace Lucky.Hr.Entity
{
    public partial class Practice
    {
        public int PracticeId { get; set; }
        public int ResumeId { get; set; }
        public string PracticeName { get; set; }
        public System.DateTime StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
        public bool PracticeNow { get; set; }
        public string Intro { get; set; }
        public System.DateTime AddDate { get; set; }
    }
}
