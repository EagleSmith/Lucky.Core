using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Attributes;
using Lucky.Core.Utility;

namespace Lucky.Hr.ViewModels.Models.SiteManager
{
    /// <summary>
    ///AspNetRoles数据实体
    /// </summary>
    [Validator(typeof(AspNetRolesViewModelFluentValidation))]
    [Serializable]
    public class AspNetRolesViewModel : ViewModelsBase
    {
        #region 构造函数
        public AspNetRolesViewModel()
        {
        }
        #endregion

        #region 公共属性
        [Display(Name = "角色Id")]
        public string Id { get; set; }

        [Display(Name = "公司Id")]
        public int DistributorId { get; set; }

        [Display(Name = "角色名称")]
        public string RoleName { get; set; }

        [Display(Name = "系统角色")]
        public bool IsSystem { get; set; }

        [Display(Name = "排序")]
        public int Sort { get; set; }

        public string DistributorName { get; set; }
        public List<ListItemEntity> DistributorListItems { get; set; }

        public IList<NavOperationViewModel> OperationViewModels { get; set; }
        #endregion
    }
    public class AspNetRolesViewModelFluentValidation : AbstractValidator<AspNetRolesViewModel>
    {
        public AspNetRolesViewModelFluentValidation()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("不能为空！");
            RuleFor(x => x.DistributorId).NotNull().WithMessage("不能为空！");
            RuleFor(x => x.RoleName).NotEmpty().WithMessage("不能为空！");
            RuleFor(x => x.IsSystem).NotNull().WithMessage("不能为空！");
            RuleFor(x => x.Sort).NotNull().WithMessage("不能为空！");
            
        }
    }
}
