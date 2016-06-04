using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Attributes;

namespace Lucky.ViewModels.Models.SiteManager
{
    /// <summary>
    ///Operation数据实体
    /// </summary>
    [Validator(typeof(OperationViewModelFluentValidation))]
    [Serializable]
    public class OperationViewModel : ViewModelsBase
    {
        #region 构造函数
        public OperationViewModel()
        {
        }
        #endregion

        #region 公共属性
        [Display(Name = "操作Id")]
        public int OperationId { get; set; }

        [Display(Name = "操作名称")]
        public string OperationName { get; set; }

        [Display(Name = "系统名称")]
        public string SystemName { get; set; }

        [Display(Name = "排序")]
        public int Sort { get; set; }

        [Display(Name = "用与存储2N次方")]
        public long OperationValue { get; set; }

        #endregion
    }


    public class OperationViewModelFluentValidation : AbstractValidator<OperationViewModel>
    {
        public OperationViewModelFluentValidation()
        {
            
            RuleFor(x => x.OperationName).NotEmpty().WithMessage("不能为空！");
            RuleFor(x => x.SystemName).NotEmpty().WithMessage("不能为空！");
            RuleFor(x => x.Sort).NotNull().WithMessage("不能为空！");
            
        }
    }
}
