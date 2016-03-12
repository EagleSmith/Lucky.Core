using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Lucky.Hr.Caching;
using Lucky.Hr.Core;
using Lucky.Hr.Core.Logging;
using Lucky.Hr.Entity;
using Lucky.Hr.Entity.Mapping;
using Lucky.Hr.IService;
using Lucky.Hr.Service;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace Lucky.Hr.WebSite.Controllers
{
    public class HomeController : Controller
    {
        private ICacheManager _cacheManager;
        public ILogger _Logger;
        private IAreaRepository _areaRepository;
        private IDistributorConfigRepository _distributorConfigRepository;
        private IHrDbContext _dbContext;

        public HomeController(
            ICacheManager cacheManager,
            ILogger logger,
            IAreaRepository areaRepository,
            IHrDbContext hrDbContext,
            IDistributorConfigRepository distributorConfigRepository
            )
        {
            _cacheManager = cacheManager;
            _Logger = logger;
            _areaRepository = areaRepository;
            _dbContext = hrDbContext;
            _distributorConfigRepository = distributorConfigRepository;
        }
        // GET: Home
        public  ActionResult Index()
        {
            IList<Area> list = new List<Area>();
            _areaRepository.IContext.Sql("").QueryComplexMany<Area>(list, mTest.AreaMapperToList);
            
            //ViewBag.MyTime = _cacheManager.Get("time", ctx => DateTime.Now.ToString());
            //_Logger.Information("测试！");
            //_dbContext=new HrDbContext();
            //_areaRepository.Add(new Area() { AreaId = "002", AreaName = "北京分公司", FullName = "北京分公司", Layer = 1, ParentId = "" },false);
            //_distributorConfigRepository.Add(new DistributorConfig() { DistributorId = 2, Logo = "ssss", PageSize = 20 });
            //_dbContext.SaveChanges();

            //var claims = new List<Claim>();
            //claims.Add(new Claim(ClaimTypes.Name, "Brock"));
            //claims.Add(new Claim(ClaimTypes.Email, "brockallen@gmail.com"));
            //var id = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);

            //var ctx = Request.GetOwinContext();
            //var authenticationManager = ctx.Authentication;
            //authenticationManager.SignIn(id);


            //var user = new Manager() { UserName = "luckearth@luckearth.cn", Email = "luckearth@luckearth.cn" };

            ////user.Password = "Admin@123456";
            //user.AddDate = DateTime.Now;
            //user.LastLoginDate = DateTime.Now;
            //user.LastModify = DateTime.Now;
            //user.LoginCount = 0;
            //user.EmailConfirmed = false;
            //user.PhoneNumberConfirmed = false;
            //user.TwoFactorEnabled = false;
            //user.LockoutEnabled = false;
            //user.LockoutEndDateUtc = DateTime.Now;
            //user.AccessFailedCount = 0;
            //user.DistributorId = 1;
            //user.DepartmentId = "001";
            //user.FullName = "";
            //user.LastLoginIp = "";
            //user.IsLock = false;
            //user.IsSuper = true;
            //user.Token = "";
            //user.State = 1;
            //user.BehaviorRemind = 1;
            //user.AddManagerId = 1;
            //user.AddFullName = "";

            //try
            //{
            //    var result = await HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>().CreateAsync(user, "Admin@123456");
            //}
            //catch (Exception)
            //{

            //    throw;
            //}
            //var result = await SignInManager.PasswordSignInAsync("luckearth@luckearth.cn", "Admin@123456", false, shouldLockout: false);
           // HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>()
            var ar = _areaRepository.Single(a => a.AreaId == "11000000");
            return  View();
        }
        
    }

    public  class mTest
    {

        public static Func<Lucky.Hr.Core.IDataReader, Area> AreaMapperFunc = (Lucky.Hr.Core.IDataReader dr) =>
        {
            Area entity = new Area();
            entity.AreaId = dr.GetString("AreaId");
            entity.ParentId = dr.GetString("ParentId");
            entity.AreaName = dr.GetString("AreaName");
            entity.FullName = dr.GetString("FullName");
            entity.Layer = dr.GetInt32("Layer");
            return entity;
        };
        public static Action<Lucky.Hr.Core.IDataReader, Area> AreaMapperAction = (Lucky.Hr.Core.IDataReader dr, Area entity) =>
        {

            entity.AreaId = dr.GetString("AreaId");
            entity.ParentId = dr.GetString("ParentId");
            entity.AreaName = dr.GetString("AreaName");
            entity.FullName = dr.GetString("FullName");
            entity.Layer = dr.GetInt32("Layer");

        };

        public static Action<IList<Area>, Lucky.Hr.Core.IDataReader> AreaMapperToList = (IList<Area> list, Lucky.Hr.Core.IDataReader dr) =>
        {
            Area entity = new Area();
            entity.AreaId = dr.GetString("AreaId");
            entity.ParentId = dr.GetString("ParentId");
            entity.AreaName = dr.GetString("AreaName");
            entity.FullName = dr.GetString("FullName");
            entity.Layer = dr.GetInt32("Layer");
            list.Add(entity);
        };
 
 


    }
}