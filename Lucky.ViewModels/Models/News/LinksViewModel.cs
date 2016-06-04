using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Attributes;

namespace Lucky.ViewModels.Models.News
{
    /// <summary>
    ///Links数据实体
    /// </summary>
    [Validator(typeof(LinksViewModelFluentValidation))]
    public class LinksViewModel : ViewModelsBase
    {
        #region 构造函数
        public LinksViewModel()
        {
        }
        #endregion

        #region 公共属性
        [Display(Name = "")]
        public Guid LinkID { get; set; }

        [Display(Name = "链接标题")]
        public string Title { get; set; }

        [Display(Name = "联系人")]
        public string UserName { get; set; }

        [Display(Name = "联系电话")]
        public string UserTel { get; set; }

        [Display(Name = "联系邮箱")]
        public string UserEmail { get; set; }

        [Display(Name = "是否图片链接")]
        public bool IsImage { get; set; }

        [Display(Name = "排序号,越小越向前")]
        public int DisplayOrder { get; set; }

        [Display(Name = "网站地址")]
        public string WebUrl { get; set; }

        [Display(Name = "Logo地址")]
        public string ImageUrl { get; set; }

        [Display(Name = "是否锁定0正常1锁定")]
        public bool IsLock { get; set; }

        [Display(Name = "添加时间")]
        public DateTime CreateDate { get; set; }

        #endregion
    }


    public class LinksViewModelFluentValidation : AbstractValidator<LinksViewModel>
    {
        public LinksViewModelFluentValidation()
        {
            RuleFor(x => x.LinkID).NotEmpty().WithMessage("不能为空！");
            RuleFor(x => x.IsImage).NotEmpty().WithMessage("不能为空！");
            RuleFor(x => x.DisplayOrder).NotEmpty().WithMessage("不能为空！");
            RuleFor(x => x.WebUrl).NotEmpty().WithMessage("不能为空！");
            RuleFor(x => x.IsLock).NotEmpty().WithMessage("不能为空！");
            RuleFor(x => x.CreateDate).NotEmpty().WithMessage("不能为空！");
        }
    }
}
