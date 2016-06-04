using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Attributes;

namespace Lucky.Hr.ViewModels.Models.SiteManager
{
    /// <summary>
    ///Distributor数据实体
    /// </summary>
    [Validator(typeof(DistributorViewModelFluentValidation))]
    [Serializable]
    public class DistributorViewModel : ViewModelsBase
    {
        #region 构造函数
        public DistributorViewModel()
        {
        }
        #endregion

        #region 公共属性
        [Display(Name = "公司Id")]
        public int DistributorId { get; set; }

        [Display(Name = "上级Id")]
        public int ParentId { get; set; }

        [Display(Name = "层级路径")]
        public string Path { get; set; }

        [Display(Name = "公司名称")]
        public string DistributionName { get; set; }

        [Display(Name = "区域Id")]
        public string AreaId { get; set; }

        [Display(Name = "街道")]
        public string Street { get; set; }

        [Display(Name = "经度")]
        public decimal Lng { get; set; }

        [Display(Name = "纬度")]
        public decimal Lat { get; set; }

        [Display(Name = "电话号码")]
        public string Phone { get; set; }

        [Display(Name = "传真号码")]
        public string Fax { get; set; }

        [Display(Name = "电子邮箱")]
        public string Email { get; set; }

        [Display(Name = "公司主页")]
        public string HomePage { get; set; }

        [Display(Name = "微信")]
        public string WeiXin { get; set; }

        [Display(Name = "银行帐号")]
        public string BankAccount { get; set; }

        [Display(Name = "备注")]
        public string Remark { get; set; }

        [Display(Name = "锁定")]
        public bool IsLock { get; set; }

        [Display(Name = "公司状态")]
        public int State { get; set; }

        #endregion
    }
    public class DistributorViewModelFluentValidation : AbstractValidator<DistributorViewModel>
    {
        public DistributorViewModelFluentValidation()
        {
            RuleFor(x => x.DistributorId).NotEmpty().WithMessage("不能为空！");
            RuleFor(x => x.ParentId).NotEmpty().WithMessage("不能为空！");
            RuleFor(x => x.Path).NotEmpty().WithMessage("不能为空！");
            RuleFor(x => x.DistributionName).NotEmpty().WithMessage("不能为空！");
            RuleFor(x => x.AreaId).NotEmpty().WithMessage("不能为空！");
            RuleFor(x => x.Street).NotEmpty().WithMessage("不能为空！");
            RuleFor(x => x.Lng).NotEmpty().WithMessage("不能为空！");
            RuleFor(x => x.Lat).NotEmpty().WithMessage("不能为空！");
            RuleFor(x => x.Phone).NotEmpty().WithMessage("不能为空！");
            RuleFor(x => x.Fax).NotEmpty().WithMessage("不能为空！");
            RuleFor(x => x.Email).NotEmpty().WithMessage("不能为空！");
            RuleFor(x => x.HomePage).NotEmpty().WithMessage("不能为空！");
            RuleFor(x => x.WeiXin).NotEmpty().WithMessage("不能为空！");
            RuleFor(x => x.BankAccount).NotEmpty().WithMessage("不能为空！");
            RuleFor(x => x.Remark).NotEmpty().WithMessage("不能为空！");
            RuleFor(x => x.IsLock).NotEmpty().WithMessage("不能为空！");
            RuleFor(x => x.State).NotEmpty().WithMessage("不能为空！");
        }
    }
}
