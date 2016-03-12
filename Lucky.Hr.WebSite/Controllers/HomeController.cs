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
using Lucky.Hr.ViewModels.Models.News;
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
        private INewsArticlesRepository _articlesRepository;

        public HomeController(
            ICacheManager cacheManager,
            ILogger logger,
            IAreaRepository areaRepository,
            IHrDbContext hrDbContext,
            IDistributorConfigRepository distributorConfigRepository,
            INewsArticlesRepository articlesRepository
            )
        {
            _cacheManager = cacheManager;
            _Logger = logger;
            _areaRepository = areaRepository;
            _dbContext = hrDbContext;
            _distributorConfigRepository = distributorConfigRepository;
            _articlesRepository = articlesRepository;
        }
        // GET: Home
        public  ActionResult Index()
        {
            List<NewsArticlesViewModel> list = _articlesRepository.GetArticlesViewModels();
            return  View(list);
        }
        
    }

}