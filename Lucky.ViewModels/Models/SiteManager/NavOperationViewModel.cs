using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lucky.Hr.ViewModels.Models.SiteManager
{
    public class NavOperationViewModel:ViewModelsBase
    {
        [Display(Name = "菜单编号")]
        public string NavId { get; set; }
        [Display(Name = "菜单名称")]
        public string NavName { get; set; }
        public string ParentId { get; set; }
        public IEnumerable<NavOperationRoleViewModel> Operations { get; set; }
    }

    public class NavOperationRoleViewModel
    {
        [Display(Name = "权限编号")]
        public int OperationId { get; set; }
        [Display(Name = "权限编号")]
        public string OperationName { get; set; }
        [Display(Name = "是否有权")]
        public bool Checked { get; set; }
    }
}
