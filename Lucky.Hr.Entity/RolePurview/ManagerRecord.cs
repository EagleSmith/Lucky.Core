using System;
using System.Collections.Generic;

namespace Lucky.Entity
{
    public partial class ManagerRecord
    {
        public string ManagerId { get; set; }
        public string Post { get; set; }
        public string HeadImage { get; set; }
        public bool Sex { get; set; }
        public System.DateTime Birthday { get; set; }
        public string CardId { get; set; }
        public string Native { get; set; }
        public string Graduation { get; set; }
        public int Education { get; set; }
        public string Professional { get; set; }
        public System.DateTime EntryDate { get; set; }
        public string AreaId { get; set; }
        public string Street { get; set; }
        public string Mobile { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Qq { get; set; }
        public string WeiXin { get; set; }
        public int Salary { get; set; }
        public virtual Manager Manager { get; set; }
    }
}
