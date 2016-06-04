using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Lucky.Entity
{
    public partial class Manager:IdentityUser
    {
        public Manager()
        {
            this.ManagerFiles = new List<ManagerFile>();
            this.ManagerLogs = new List<ManagerLog>();
        }
        public async Task<ClaimsIdentity>GenerateUserIdentityAsync(UserManager<Manager> manager)
        {
            var userIdentity = await manager .CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            return userIdentity;
        }
        public int DistributorId { get; set; }
        public string DepartmentId { get; set; }
        
        public string FullName { get; set; }//Discriminator
        public bool IsSuper { get; set; }
        public bool IsLock { get; set; }
        public int LoginCount { get; set; }
        public System.DateTime LastLoginDate { get; set; }
        public string LastLoginIp { get; set; }
        public System.DateTime LastModify { get; set; }
        public string Token { get; set; }
        public int State { get; set; }
        public int BehaviorRemind { get; set; }
        public int AddManagerId { get; set; }
        public string AddFullName { get; set; }
        public System.DateTime AddDate { get; set; }
        public virtual Distributor Distributor { get; set; }
        public virtual ICollection<ManagerFile> ManagerFiles { get; set; }
        public virtual ICollection<ManagerLog> ManagerLogs { get; set; }
        public virtual ManagerRecord ManagerRecord { get; set; }
    }
}
