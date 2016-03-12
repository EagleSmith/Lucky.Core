using System;
using System.Collections.Generic;

namespace Lucky.Hr.Entity
{
    public partial class Resume
    {
        public Resume()
        {
            this.Achievements = new List<Achievement>();
            this.Certs = new List<Cert>();
            this.Educations = new List<Education>();
            this.Languages = new List<Language>();
            this.Other = new List<Other>();
            this.Projects = new List<Project>();
            this.ResumeAreas = new List<ResumeArea>();
            this.ResumeIndustries = new List<ResumeIndustry>();
            this.ResumeJobCategories = new List<ResumeJobCategory>();
            this.Skills = new List<Skill>();
            this.Works = new List<Work>();
        }

        public int ResumeId { get; set; }
        public int PersonalId { get; set; }
        public string Fullname { get; set; }
        public bool HidFullname { get; set; }
        public int Sex { get; set; }
        public System.DateTime BirthDay { get; set; }
        public string CurrentAreaId { get; set; }
        public int WorkState { get; set; }
        public System.DateTime StartWork { get; set; }
        public string NativeAreaId { get; set; }
        public int Marriage { get; set; }
        public int Political { get; set; }
        public double Height { get; set; }
        public double Weight { get; set; }
        public int Jobtype { get; set; }
        public string JobIntention { get; set; }
        public bool Parttime { get; set; }
        public int JobLevel { get; set; }
        public bool LowLevel { get; set; }
        public int Salary { get; set; }
        public int LowSalary { get; set; }
        public bool Negotiable { get; set; }
        public virtual ICollection<Achievement> Achievements { get; set; }
        public virtual ICollection<Cert> Certs { get; set; }
        public virtual ICollection<Education> Educations { get; set; }
        public virtual ICollection<Language> Languages { get; set; }
        public virtual ICollection<Other> Other { get; set; }
        public virtual Personal Personal { get; set; }
        public virtual ICollection<Project> Projects { get; set; }
        public virtual ICollection<ResumeArea> ResumeAreas { get; set; }
        public virtual ICollection<ResumeIndustry> ResumeIndustries { get; set; }
        public virtual ICollection<ResumeJobCategory> ResumeJobCategories { get; set; }
        public virtual ICollection<Skill> Skills { get; set; }
        public virtual ICollection<Work> Works { get; set; }
    }
}
