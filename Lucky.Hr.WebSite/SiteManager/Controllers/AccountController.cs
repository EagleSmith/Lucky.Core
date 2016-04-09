

using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Lucky.Core.Cache;
using Lucky.Core.Logging;
using Lucky.Hr.Entity;
using Lucky.Hr.IService;
using Lucky.Hr.Service;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;


namespace Lucky.Hr.SiteManager.Controllers
{
    public class AccountController : BaseAdminController
    {
        private ICacheManager _cacheManager;
        public ILogger _Logger;
        private IAreaService _areaService;
        private IDistributorConfigService _distributorConfigService;
        private IHrDbContext _dbContext;

        public AccountController(
            ICacheManager cacheManager,
            ILogger logger,
            IAreaService areaService,
            IHrDbContext hrDbContext,
            IDistributorConfigService distributorConfigService
            )
        {
            _cacheManager = cacheManager;
            _Logger = logger;
            _areaService = areaService;
            _dbContext = hrDbContext;
            _distributorConfigService = distributorConfigService;
        }
        // GET: Account
        public ActionResult Index()
        {
            
            return View();
        }
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(string username, string password, bool rememberme, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            Manager manager = await UserManager.FindByEmailAsync("luckearth@luckearth.cn");
            var result =  UserManager.PasswordHasher.VerifyHashedPassword(manager.PasswordHash, "Admin@123456");
            switch (result)
            {
                case PasswordVerificationResult.Success:
                    if (returnUrl != null)
                    {
                        SetCalims(username, rememberme);
                        return Redirect("~" + returnUrl);
                    }
                    else
                    {
                        SetCalims(username, rememberme);
                    }
                    return RedirectToAction("index", "home");
                default:
                    return View();
            }
            
            
           
            //Manager manager = await UserManager.FindByEmailAsync("luckearth@luckearth.cn");
            //await SignInManager.SignInAsync(manager, false, false);
            //SignInStatus result = await SignInManager.PasswordSignInAsync("luckearth@luckearth.cn", "Admin@123456", false, shouldLockout: false);

            //switch (result)
            //{
            //    case SignInStatus.Success:
            //        if (returnUrl != null)
            //        {
            //            return Redirect("~" + returnUrl);
            //        }
            //        return RedirectToAction("index", "home");
            //    default:
            //        return View();
            //}
        }

        private void SetCalims(string username, bool rememberme)
        {
            var identity = new ClaimsIdentity(new[] {new Claim(ClaimTypes.Name, username),}, DefaultAuthenticationTypes.ApplicationCookie, ClaimTypes.Name, ClaimTypes.Role);
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"));
            
            identity.AddClaim(new Claim(ClaimTypes.Role, "guest"));
            identity.AddClaim(new Claim(ClaimTypes.GivenName, "A Person"));
            identity.AddClaim(new Claim(ClaimTypes.Sid, "123456")); //OK to store userID here?

            AuthenticationManager.SignIn(new AuthenticationProperties
            {
                IsPersistent = rememberme
            }, identity);
        }

        private IAuthenticationManager AuthenticationManager
        {
            get { return HttpContext.GetOwinContext().Authentication; }
        }
        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        private ApplicationSignInManager _signInManager;

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set { _signInManager = value; }
        }
    }
}