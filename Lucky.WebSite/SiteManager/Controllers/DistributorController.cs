using System.Linq;
using System.Web.Mvc;
using Lucky.Core;
using Lucky.Entity;
using Lucky.Hr.IService;
using Lucky.Hr.ViewModels;
using Lucky.Hr.ViewModels.Models.SiteManager;

namespace Lucky.Hr.SiteManager.Controllers
{
    public class DistributorController : BaseAdminController
    {
        private readonly IDistributorService _distributorService;
        public DistributorController(IDistributorService distributorService)
        {
            _distributorService = distributorService;
        }
        // GET: Distributor
        public ActionResult Index(int pageIndex = 1)
        {
            var pagedList = _distributorService.GetPaged(a=>a.DistributorId, pageIndex, 1);
            var models = pagedList.Select(x => { var m = x.ToModel();
                                                   return m;
            }).ToPagedList(pageIndex,1,pagedList.TotalCount);
            
            
            return View(models);
        }
        #region 基本操作

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(DistributorViewModel model)
        {
            if (!ModelState.IsValid) return View();
            Distributor entity = model.ToEntity();
            entity.ParentId = 0;
            entity.AreaId = "11010000";
            entity.Street = "上海";
            entity.Lat = 29;
            entity.Lng = 30;
            entity.State = 1;
            entity.Path = "aaa";
            _distributorService.Add(entity);
            return RedirectToAction("Detail",entity.ToModel());
        }
        public ActionResult Edit()
        {
            return View();
        }

        public ActionResult Detail(DistributorViewModel model)
        {
            if (model==null)
            {
                model=new DistributorViewModel();
            }
            return View(model);
        }
        #endregion

        #region 私有方法

        #endregion
    }
}