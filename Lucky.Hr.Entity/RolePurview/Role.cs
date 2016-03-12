using System;
using System.Collections.Generic;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Lucky.Hr.Entity
{
    public partial class Role:IdentityRole
    {
        public Role()
        {
            this.DepartmentRoles = new List<DepartmentRole>();
            
            this.RoleNavs = new List<RoleNav>();
        }

        
        public int DistributorId { get; set; }
        public string RoleName { get; set; }
        public bool IsSystem { get; set; }
        public int Sort { get; set; }
        public virtual ICollection<DepartmentRole> DepartmentRoles { get; set; }
        
        public virtual ICollection<RoleNav> RoleNavs { get; set; }
    }
}
