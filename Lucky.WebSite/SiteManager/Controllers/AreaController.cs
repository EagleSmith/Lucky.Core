using System.Collections.Generic;
using System.Linq;
using System.Web.Helpers;
using System.Web.Mvc;
using Lucky.Core;
using Lucky.Core.Cache;
using Lucky.Core.Data.Specification;
using Lucky.Core.Infrastructure;
using Lucky.Core.Logging;
using Lucky.Core.Utility;
using Lucky.Entity;
using Lucky.IService;
using Lucky.ViewModels;
using Lucky.ViewModels.Models;
using Lucky.ViewModels.Models.SiteManager;
using Lucky.Web.Framework;

namespace Lucky.Hr.SiteManager.Controllers
{
    public class AreaController :BaseAdminController
    {
        #region 私有变量
        private ICacheManager _cacheManager;
        public ILogger _Logger;
        private readonly IAreaService _areaService;
        private IHrDbContext _dbContext;
        #endregion

        #region 构造函数
        public AreaController(
            ICacheManager cacheManager,
            ILogger logger,
            IAreaService areaService,
            IHrDbContext dbContext
            )
        {
            _cacheManager = cacheManager;
            _Logger = logger;
            _areaService = areaService;
            _dbContext = dbContext;
        }
        #endregion

        public JsonpResult Droplet(string citycode = "", int levelid = 1)
        {
            var list = _areaService.Find(a => a.ParentId == citycode).Select(a => new {code = a.AreaId, label = a.AreaName}).ToList();// AreaHelper.GetAll(citycode).Select(a => new { code = a.AreaId, label = a.AreaName }).ToList();
            return new JsonpResult(new { result = new { division = list } });
        }
        // GET: Area
        public ActionResult Index(AreaSearchModel model,int pageIndex = 1, string keyword = "")
        {
            
            var forms = HttpContext.Request.QueryString.AllKeys;
            var spec = SpecificationBuilder.Create<Area>();
            model.Expression(spec);
            var pagedList = _areaService.GetPaged(spec.Predicate,a => a.AreaId, pageIndex, 20);
            var models = pagedList.Select(x =>
            {
                var m = x.ToModel();
                return m;
            }).ToPagedList(pageIndex, 20, pagedList.TotalCount);

            return View(models);
        }
        [HttpPost]
        public ActionResult Create(AreaViewModel model)
        {
            if (ModelState.IsValid)
            {
                Area area = model.ToEntity();
                
                return RedirectToAction("Detail");
            }
            return View(model);
        }
        public ActionResult Create()
        {
            AreaViewModel model = new AreaViewModel();
            ViewBag.SelectItems = new List<SelectListItem>()
                {
                    new SelectListItem {Text = "A",Value = "A"},
                    new SelectListItem {Text = "B",Value = "B"}
                };
            //model.AreaItems = new List<ListItemEntity> { new ListItemEntity {ID = "1", ParentID = "", Title = "测试"} };
            GetViewModel(model);
            return View(model);
        }

        public ActionResult Detail(string id)
        {
            var entity = _areaService.Single(a => a.AreaId == id);
            var model = entity.ToModel();
            string pid = entity.ParentId;
            var ep = _areaService.Single(a => a.AreaId == pid);

            model.ParentName = ep == null ? "无" : ep.AreaName;
            return View(model);
        }

        public ActionResult Edit()
        {
            return View();
        }
        #region 私有方法

        private void GetViewModel(AreaViewModel model)
        {
            model.AreaItems = _areaService.GetQuery(a=>a.ParentId=="").Select(
                a=>new ListItemEntity
                {
                    ID =a.AreaId,
                    ParentID = a.ParentId,
                    Title = a.AreaName
                }).ToList();
            
        }

        public JsonResult GetAreaParentID(string parentid)
        {
            parentid = parentid.Substring(0, 2);
            IList<SelectListItem> list = _areaService.GetQuery(a => a.ParentId.StartsWith(parentid)).Select(
                a => new SelectListItem() {Value = a.AreaId,Text = a.AreaName}
                ).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        #endregion

    }
}