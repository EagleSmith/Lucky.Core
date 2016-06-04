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
    ///AspNetUsers数据实体
    /// </summary>
    [Validator(typeof(AspNetUsersViewModelFluentValidation))]
    [Serializable]
    public class AspNetUsersViewModel : ViewModelsBase
    {
        #region 构造函数
        public AspNetUsersViewModel()
        {
        }
        #endregion

        #region 公共属性

        [Display(Name = "管理员Id")]
        public string Id { get; set; }

        [Display(Name = "公司Id")]
        public int DistributorId { get; set; }

        [Display(Name = "部门Id")]
        public string DepartmentId { get; set; }

        [Display(Name = "用户名")]
        public string UserName { get; set; }

        [Display(Name = "密码")]
        public string Password { get; set; }

        [Display(Name = "姓名")]
        public string FullName { get; set; }

        [Display(Name = "超级管理员")]
        public bool IsSuper { get; set; }

        [Display(Name = "锁定")]
        public bool IsLock { get; set; }

        [Display(Name = "登陆次数")]
        public int LoginCount { get; set; }

        [Display(Name = "最后登录时间")]
        public DateTime LastLoginDate { get; set; }

        [Display(Name = "最后登录Ip")]
        public string LastLoginIp { get; set; }

        [Display(Name = "最后修改时间")]
        public DateTime LastModify { get; set; }

        [Display(Name = "用户令牌")]
        public string Token { get; set; }

        [Display(Name = "状态")]
        public int State { get; set; }

        [Display(Name = "行为提醒")]
        public int BehaviorRemind { get; set; }

        [Display(Name = "创建者Id")]
        public int AddManagerId { get; set; }

        [Display(Name = "创建者姓名")]
        public string AddFullName { get; set; }

        [Display(Name = "创建时间")]
        public DateTime AddDate { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "是否Email确认")]
        public bool? EmailConfirmed { get; set; }

       

        [Display(Name = "手机号码")]
        public string PhoneNumber { get; set; }

        [Display(Name = "是否手机确认")]
        public bool? PhoneNumberConfirmed { get; set; }

        [Display(Name = "是否双重认证")]
        public bool? TwoFactorEnabled { get; set; }

        [Display(Name = "锁定时间")]
        public DateTime? LockoutEndDateUtc { get; set; }

        [Display(Name = "是否锁定")]
        public bool? LockoutEnabled { get; set; }
        
        [Display(Name = "失败次数")]
        public int? AccessFailedCount { get; set; }

        

        #endregion
    }
    public class AspNetUsersViewModelFluentValidation : AbstractValidator<AspNetUsersViewModel>
    {
        public AspNetUsersViewModelFluentValidation()
        {
            //RuleFor(x => x.Id).NotEmpty().WithMessage("不能为空！");
            RuleFor(x => x.DistributorId).NotEmpty().WithMessage("不能为空！");
            RuleFor(x => x.DepartmentId).NotEmpty().WithMessage("不能为空！");
            RuleFor(x => x.UserName).NotEmpty().WithMessage("不能为空！");
            RuleFor(x => x.Password).NotEmpty().WithMessage("不能为空！").Length(6,100).WithMessage("密码长度不能小于6或大于100！");
            RuleFor(x => x.FullName).NotEmpty().WithMessage("不能为空！");
            RuleFor(x => x.IsSuper).NotEmpty().WithMessage("不能为空！");
            RuleFor(x => x.IsLock).NotEmpty().WithMessage("不能为空！");
            RuleFor(x => x.LoginCount).NotEmpty().WithMessage("不能为空！");
           // RuleFor(x => x.LastLoginDate).NotEmpty().WithMessage("不能为空！");
           // RuleFor(x => x.LastLoginIp).NotEmpty().WithMessage("不能为空！");
           // RuleFor(x => x.LastModify).NotEmpty().WithMessage("不能为空！");
            RuleFor(x => x.Token).NotEmpty().WithMessage("不能为空！");
            RuleFor(x => x.State).NotEmpty().WithMessage("不能为空！");
            RuleFor(x => x.BehaviorRemind).NotEmpty().WithMessage("不能为空！");
            RuleFor(x => x.AddManagerId).NotEmpty().WithMessage("不能为空！");
            RuleFor(x => x.AddFullName).NotEmpty().WithMessage("不能为空！");
           // RuleFor(x => x.AddDate).NotEmpty().WithMessage("不能为空！");
        }
    }
}
