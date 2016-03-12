using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using FluentValidation;
using FluentValidation.Attributes;
using Lucky.Hr.Core.Utility;

namespace Lucky.Hr.ViewModels.Models.SiteManager
{
    /// <summary>
    ///Department数据实体
    /// </summary>
   [Validator(typeof(DepartmentViewModelFluentValidation))]
    [Serializable]
    public class DepartmentViewModel : ViewModelsBase
    {
        #region 构造函数
        public DepartmentViewModel()
        {
        }
        #endregion

        #region 公共属性
        [Display(Name = "部门Id")]
        public string DepartmentId { get; set; }

        [Display(Name = "公司Id")]
        public int DistributorId { get; set; }

        [Display(Name = "上级Id")]
        public string ParentId { get; set; }

        [Display(Name = "部门名称")]
        public string DepartmentName { get; set; }

        [Display(Name = "部门描述")]
        public string Description { get; set; }

        [Display(Name = "状态")]
        public int State { get; set; }

        [Display(Name = "排序")]
        public int Sort { get; set; }
        public string ParentName { get; set; }
        public string DistributorName { get; set; }

        #endregion

        public List<ListItemEntity> DepartmentListItems { get; set; }

        public List<SelectListItem> DistributorListItems { get; set; }
    }
    public class DepartmentViewModelFluentValidation : AbstractValidator<DepartmentViewModel>
    {
        public DepartmentViewModelFluentValidation()
        {
            RuleFor(x => x.DepartmentId).NotEmpty().WithMessage("不能为空！");
            RuleFor(x => x.DistributorId).NotEmpty().WithMessage("不能为空！");
            RuleFor(x => x.ParentId).NotEmpty().WithMessage("不能为空！");
            RuleFor(x => x.DepartmentName).NotEmpty().WithMessage("不能为空！");
            RuleFor(x => x.Description).NotEmpty().WithMessage("不能为空！");
            RuleFor(x => x.State).NotEmpty().WithMessage("不能为空！");
            RuleFor(x => x.Sort).NotEmpty().WithMessage("不能为空！");
        }
    }
}
