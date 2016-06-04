using System;
using System.Collections.Generic;

namespace Lucky.Entity
{
    public partial class Department
    {
        public Department()
        {
            this.DepartmentRoles = new List<DepartmentRole>();
            this.Departments=new List<Department>();
        }

        public string DepartmentId { get; set; }
        public int DistributorId { get; set; }
        public string ParentId { get; set; }
        public string DepartmentName { get; set; }
        public string Description { get; set; }
        public int State { get; set; }
        public int Sort { get; set; }

        public virtual ICollection<Department> Departments { get; set; }
        public virtual Department Parent { get; set; }
        public virtual Distributor Distributor { get; set; }
        public virtual ICollection<DepartmentRole> DepartmentRoles { get; set; }
    }
}
