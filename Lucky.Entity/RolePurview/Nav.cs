using System;
using System.Collections.Generic;

namespace Lucky.Entity
{
    public partial class Nav
    {
        public Nav()
        {
            this.NavOperations = new List<NavOperation>();
            this.RoleNavs = new List<RoleNav>();
        }

        public string NavId { get; set; }
        public string ParentId { get; set; }
        public int NavType { get; set; }
        public string NavName { get; set; }
        public string SystemName { get; set; }
        public string Logo { get; set; }
        public string Url { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public bool IsExpend { get; set; }
        public int State { get; set; }
        public int Sort { get; set; }
        public virtual ICollection<NavOperation> NavOperations { get; set; }
        public virtual ICollection<RoleNav> RoleNavs { get; set; }
    }
}
