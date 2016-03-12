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

namespace Lucky.Hr.ViewModels.Models.News
{
    /// <summary>
    ///NewsArticles数据实体
    /// </summary>
    [Validator(typeof(NewsArticlesViewModelFluentValidation))]
    public class NewsArticlesViewModel : ViewModelsBase
    {
        #region 构造函数
        public NewsArticlesViewModel()
        {
        }
        #endregion

        #region 公共属性
        [Display(Name = "主键")]
        public Guid ArticleID { get; set; }

        [Display(Name = "新闻分类")]
        public string CategoryID { get; set; }

        [Display(Name = "文章标题 ")]
        public string Title { get; set; }

        [Display(Name = "文章摘要")]
        public string Summarize { get; set; }

        [Display(Name = "文章来源")]
        public string Source { get; set; }

        [Display(Name = "原文作者")]
        public string Author { get; set; }

        [Display(Name = "责任编辑")]
        public string Editor { get; set; }

        [Display(Name = "关键字")]
        public string KeyWord { get; set; }

        [Display(Name = "是否置顶")]
        public bool IsTop { get; set; }

        [Display(Name = "是否热门")]
        public bool IsHot { get; set; }

        [Display(Name = "是否可以评论")]
        public bool IsComment { get; set; }

        [Display(Name = "是否锁定,锁定是不可以查看的")]
        public bool IsLock { get; set; }

        [Display(Name = "是否推荐")]
        public bool IsCommend { get; set; }

        [Display(Name = "是否为幻灯片")]
        public bool IsSlide { get; set; }

        [Display(Name = "图片地址")]
        public string ImgUrl { get; set; }

        [Display(Name = "点击次数")]
        public int ClickNum { get; set; }

        [Display(Name = "添加时间")]
        public DateTime CreateDate { get; set; }

        [Display(Name = "更新时间")]
        public DateTime UpdateDate { get; set; }

        [Display(Name = "机构编号")]
        public int? OrgID { get; set; }

        [Display(Name = "用户编号")]
        public string UserID { get; set; }

        [AllowHtml]
        [Display(Name = "文章内容")]
        public string ArticleText { get; set; }
        public string NoHtml { get; set; }
        public long TextID { get; set; }
        public List<ListItemEntity> CategoryItems { get; set; }
        [Display(Name = "新闻分类")]
        public string CategoryTitle { get; set; }
        #endregion
    }


    public class NewsArticlesViewModelFluentValidation : AbstractValidator<NewsArticlesViewModel>
    {
        public NewsArticlesViewModelFluentValidation()
        {
            RuleFor(x => x.ArticleID).NotNull().WithMessage("不能为空！");
            RuleFor(x => x.CategoryID).NotNull().WithMessage("不能为空！");
            RuleFor(x => x.Title).NotEmpty().WithMessage("不能为空！");
            RuleFor(x => x.IsTop).NotNull().WithMessage("不能为空！");
            RuleFor(x => x.IsHot).NotNull().WithMessage("不能为空！");
            RuleFor(x => x.IsComment).NotNull().WithMessage("不能为空！");
            RuleFor(x => x.IsLock).NotNull().WithMessage("不能为空！");
            RuleFor(x => x.IsCommend).NotNull().WithMessage("不能为空！");
            RuleFor(x => x.IsSlide).NotNull().WithMessage("不能为空！");
            RuleFor(x => x.ClickNum).NotNull().WithMessage("不能为空！");
            RuleFor(x => x.CreateDate).NotNull().WithMessage("不能为空！");
            RuleFor(x => x.UpdateDate).NotNull().WithMessage("不能为空！");
        }
    }
}
