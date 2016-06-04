using System;
using System.Collections.Generic;

namespace Lucky.Entity
{
    public partial class Personal
    {
        public Personal()
        {
            this.Resumes = new List<Resume>();
        }

        public int PersonalId { get; set; }
        public string Mobile { get; set; }
        public bool ValidMobile { get; set; }
        public string Email { get; set; }
        public bool ValidEmail { get; set; }
        public string QQ { get; set; }
        public string Password { get; set; }
        public System.DateTime RegisterDate { get; set; }
        public int LoginCount { get; set; }
        public string prevLoginIp { get; set; }
        public System.DateTime PrevLoginDate { get; set; }
        public string LastLoginIp { get; set; }
        public System.DateTime LastLoginDate { get; set; }
        public System.DateTime LastModifyDate { get; set; }
        public string Token { get; set; }
        public bool IsOpen { get; set; }
        public virtual ICollection<Resume> Resumes { get; set; }
    }
}
