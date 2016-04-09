using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using FluentValidation;
using FluentValidation.Attributes;
using Lucky.Core.Utility;

namespace Lucky.Hr.ViewModels.Models.SiteManager
{
    /// <summary>
    ///Area数据实体
    /// </summary>
   [Validator(typeof(AreaViewModelFluentValidation))]
    [Serializable]
    public class AreaViewModel:ViewModelsBase
    {
        #region 构造函数
        public AreaViewModel()
        {
        }
        #endregion

        #region 公共属性
        [Display(Name = "地区Id")]
        public string AreaId { get; set; }

        [Display(Name = "上级Id")]
        public string ParentId { get; set; }

        [Display(Name = "地区名称")]
        public string AreaName { get; set; }

        [Display(Name = "地区全名")]
        public string FullName { get; set; }

        [Display(Name = "层级")]
        public int Layer { get; set; }
        public string ParentName { get; set; }
        public List<ListItemEntity> AreaItems { get; set; }
        #endregion
    }
    public class AreaViewModelFluentValidation : AbstractValidator<AreaViewModel>
    {
        public AreaViewModelFluentValidation()
        {
            RuleFor(x => x.AreaName).NotEmpty().WithMessage("请填写区域名称！");
        }
    }
}
