using System;
using System.Collections.Generic;

namespace Lucky.Entity
{
    public partial class Distributor
    {
        public Distributor()
        {
            this.Departments = new List<Department>();
            this.Managers = new List<Manager>();
            this.NewsTypes = new List<NewsType>();
        }

        public int DistributorId { get; set; }
        public int ParentId { get; set; }
        public string Path { get; set; }
        public string DistributionName { get; set; }
        public string AreaId { get; set; }
        public string Street { get; set; }
        public decimal Lng { get; set; }
        public decimal Lat { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string HomePage { get; set; }
        public string WeiXin { get; set; }
        public string BankAccount { get; set; }
        public string Remark { get; set; }
        public bool IsLock { get; set; }
        public int State { get; set; }
        public virtual ICollection<Department> Departments { get; set; }
        public virtual DistributorConfig DistributorConfig { get; set; }
        public virtual ICollection<Manager> Managers { get; set; }
        public virtual ICollection<NewsType> NewsTypes { get; set; }
    }
}
