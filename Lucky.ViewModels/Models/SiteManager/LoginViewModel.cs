using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lucky.ViewModels.Models.SiteManager
{
    public class LoginViewModel : ViewModelsBase
    {
        [Required]
        [Display(Name = "用户名：")]
        [EmailAddress]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "密  码：")]
        public string PassWord { get; set; }
        [Display(Name = "记住我?")]
        public bool RememberMe { get; set; }
    }
}
