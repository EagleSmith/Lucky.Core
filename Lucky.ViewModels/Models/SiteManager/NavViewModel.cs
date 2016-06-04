using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using FluentValidation;
using FluentValidation.Attributes;
using Lucky.Core.Utility;

namespace Lucky.ViewModels.Models.SiteManager
{
    /// <summary>
    ///Nav数据实体
    /// </summary>
    [Validator(typeof(NavViewModelFluentValidation))]
    [Serializable]
    public class NavViewModel : ViewModelsBase
    {
        #region 构造函数
        public NavViewModel()
        {
        }
        #endregion

        #region 公共属性
        [Display(Name = "菜单Id")]
        public string NavId { get; set; }

        [Display(Name = "上级Id")]
        public string ParentId { get; set; }

        [Display(Name = "菜单类型")]
        public int NavType { get; set; }

        [Display(Name = "菜单名称")]
        public string NavName { get; set; }

        [Display(Name = "系统名称")]
        public string SystemName { get; set; }

        [Display(Name = "菜单图标")]
        public string Logo { get; set; }

        [Display(Name = "菜单连接")]
        public string Url { get; set; }

        [Display(Name = "控制器名称")]
        public string ControllerName { get; set; }

        [Display(Name = "操作名称")]
        public string ActionName { get; set; }

        [Display(Name = "展开")]
        public bool IsExpend { get; set; }

        [Display(Name = "状态")]
        public int State { get; set; }

        [Display(Name = "排序")]
        public int Sort { get; set; }

        public List<ListItemEntity> ParentItems { get; set; }
        public List<SelectListItem> NavOperationItems { get; set; }
        #endregion
    }
    public class NavViewModelFluentValidation : AbstractValidator<NavViewModel>
    {
        public NavViewModelFluentValidation()
        {
            RuleFor(x => x.NavId).NotEmpty().WithMessage("不能为空！");
            RuleFor(x => x.ParentId).NotEmpty().WithMessage("不能为空！");
            RuleFor(x => x.NavType).NotNull().WithMessage("不能为空！");
            RuleFor(x => x.NavName).NotEmpty().WithMessage("不能为空！").SetValidator(new RemoteValidator("名字称重复", "ValidateNavName", "Nav", HttpVerbs.Get, "NavId"));
            RuleFor(x => x.SystemName).NotEmpty().WithMessage("不能为空！");
            RuleFor(x => x.Logo).NotEmpty().WithMessage("不能为空！");
            RuleFor(x => x.Url).NotEmpty().WithMessage("不能为空！");
            RuleFor(x => x.ControllerName).NotEmpty().WithMessage("不能为空！");
            RuleFor(x => x.ActionName).NotEmpty().WithMessage("不能为空！");
            RuleFor(x => x.IsExpend).NotNull().WithMessage("不能为空！");
            RuleFor(x => x.State).NotNull().WithMessage("不能为空！");
            RuleFor(x => x.Sort).NotNull().WithMessage("不能为空！");
        }
    }
}
