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
    [Validator(typeof(ChangePasswordViewModelFluentValidation))]
    public class ChangePasswordViewModel
    {
        
        [Display(Name = "当前密码")]
        public string OldPassword { get; set; }

        
        [DataType(DataType.Password)]
        [Display(Name = "新密码")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "确认密码")]
        public string ConfirmPassword { get; set; }
    }

    public class ChangePasswordViewModelFluentValidation : AbstractValidator<ChangePasswordViewModel>
    {
        public ChangePasswordViewModelFluentValidation()
        {
            RuleFor(a => a.OldPassword).NotEmpty().WithMessage("密码不能为空！");
            RuleFor(a => a.NewPassword).NotEmpty().WithMessage("新密码不能为空！");
            RuleFor(a => a.ConfirmPassword).Equal(a => a.NewPassword).WithMessage("新密码不匹配！");
        }
    }
}
