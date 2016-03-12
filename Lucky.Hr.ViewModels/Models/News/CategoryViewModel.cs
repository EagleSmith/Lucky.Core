using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Attributes;
using Lucky.Hr.Core.Utility;

namespace Lucky.Hr.ViewModels.Models.News
{
    /// <summary>
    ///Category数据实体
    /// </summary>
    [Validator(typeof(CategoryViewModelFluentValidation))]
    public class CategoryViewModel : ViewModelsBase
    {
        #region 构造函数
        public CategoryViewModel()
        {
        }
        #endregion

        #region 公共属性
        [Display(Name = "分类编号")]
        public string CategoryID { get; set; }

        [Display(Name = "分类标题")]
        public string Title { get; set; }

        [Display(Name = "描述信息")]
        public string Description { get; set; }

        [Display(Name = "链接地址")]
        public string HyperLink { get; set; }

        [Display(Name = "父级分类")]
        public string ParentID { get; set; }

        [Display(Name = "排序号")]
        public int DisplayOrder { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "添加时间")]
        public DateTime CreateDate { get; set; }

        [Display(Name = "分类类型")]
        public string CategoryType { get; set; }
        public List<ListItemEntity> ParentItems { get; set; }

        #endregion
    }


    public class CategoryViewModelFluentValidation : AbstractValidator<CategoryViewModel>
    {
        public CategoryViewModelFluentValidation()
        {
            RuleFor(x => x.CategoryID).NotEmpty().WithMessage("不能为空！");
            RuleFor(x => x.Title).NotEmpty().WithMessage("不能为空！");
            RuleFor(x => x.ParentID).NotEmpty().WithMessage("不能为空！");
            RuleFor(x => x.DisplayOrder).NotNull().WithMessage("不能为空！");
            RuleFor(x => x.CreateDate).NotEmpty().WithMessage("不能为空！");
        }
    }
}
