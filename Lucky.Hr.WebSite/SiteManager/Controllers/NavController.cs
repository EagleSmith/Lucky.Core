using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lucky.Core;
using Lucky.Core.Data.Specification;
using Lucky.Core.Utility;
using Lucky.Hr.Entity;
using Lucky.Hr.ViewModels.Models.SiteManager;
using Lucky.Hr.IService;
using Lucky.Hr.ViewModels;

namespace Lucky.Hr.SiteManager.Controllers
{
    public class NavController : BaseAdminController
    {
        private INavService _navService;
        private IHrDbContext _context;
        private IOperationService _operationService;
        private INavOperationService _navOperationService;
        public NavController(IHrDbContext context, INavService navService,IOperationService navOperation,INavOperationService navOperationService)
        {
            _context = context;
            _navService = navService;
            _operationService = navOperation;
            _navOperationService = navOperationService;
        }
        // GET: Nav
        public ActionResult Index(int pageIndex = 1, string keyword = "")
        {
            var spec = SpecificationBuilder.Create<Nav>();
            spec.Equals(a => a.State, 1);
            if (keyword != "")
                spec.Like(a => a.NavName, keyword);
            var pagelist = _navService.GetPaged(spec.Predicate, a => a.NavId, pageIndex, 20);
            var models = pagelist.Select(a => { return a.ToModel(); }).ToPagedList(pageIndex, 20, pagelist.TotalCount);
            return View(models);
        }

        // GET: Nav/Details/5
        public ActionResult Details(string id)
        {
            var entity = _navService.Single(a => a.NavId == id);
            var navoperlist = _navOperationService.GetQuery(a => a.NavId == id);
            var operlist = _operationService.GetList().Select(a => new SelectListItem() {Text = a.OperationName, Value = a.OperationId.ToString(), Selected = navoperlist.Any(b => a.OperationId == b.OperationId)}).ToList();
            var model = entity.ToModel();

            model.NavOperationItems = operlist;
            ;
            return View(model);
        }

        // GET: Nav/Create
        public ActionResult Create()
        {
            
            NavViewModel model=new NavViewModel();
            GetViewModel(model);
            return View(model);
        }

        // POST: Nav/Create
        [HttpPost]
        public ActionResult Create(NavViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var entity = model.ToEntity();
                    _navService.Add(entity);
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        [HttpPost]
        public ActionResult OpertionCreate(FormCollection from)
        {
            var s = from["ParentId"];
            if (s.Length > 0)
            {
                string navid = from["NavID"];
                var array = s.Split(',');
                _navOperationService.Delete(a=>a.NavId==navid);
               
                foreach (string str in array)
                {
                    NavOperation entity=new NavOperation();
                    entity.NavId = navid;
                    entity.OperationId = Convert.ToInt32(str);
                    _navOperationService.Add(entity);
                }
              
            }
            return RedirectToAction("Details", new {id = from["NavID"]});
        }
        // GET: Nav/Edit/5
        public ActionResult Edit(string id)
        {
            try
            {
                var entity = _navService.Single(a => a.NavId == id);
                var model = entity.ToModel();
                GetViewModel(model);

                return View(model);
            }
            catch (Exception)
            {
                
            }
            return View();
        }

        // POST: Nav/Edit/5
        [HttpPost]
        public ActionResult Edit(NavViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var entity = model.ToEntity();
                    _navService.Update(entity);
                }

                return RedirectToAction("Index");

            }
            catch
            {
                return View(model);
            }
        }

        // GET: Nav/Delete/5
        public ActionResult Delete(string id)
        {
            _navService.Delete(a=>a.NavId==id);
            var redirectUrl = new UrlHelper(Request.RequestContext).Action("Index", "Nav");
            return Json(new { Url = redirectUrl }, JsonRequestBehavior.AllowGet);
        }


        #region 私有函数

        private void GetViewModel(NavViewModel model)
        {
            model.ParentItems = _navService.GetQuery(a=>a.NavId.Length<=6).ToList().Select(a => new ListItemEntity()
            {
                ID = a.NavId,
                ParentID = a.ParentId,
                Title = a.NavName,
                Selected = a.ParentId==model.ParentId
                
            }).ToList();
            model.ParentItems.Add(new ListItemEntity(){ID = "-1",ParentID = "",Title = "----根节点----"});
        }
        #endregion

        #region Ajax 调用

        public JsonResult GetNavNextID(string id)
        {
            string temid,res;
            if (id.Length == 6||id.Length==3)
                temid = id;
            else temid = "";
            string e = _navService.GetQuery().Where(a => a.ParentId == temid).ToList().Max(a => a.NavId);
            if (e == null) e = temid + "000";//二级菜单没有下级时 e 为null
            if(e.Length==9)
                res = StringHelper.GetID(e, 6, 3, 3);
            else if(e.Length==6)
            res = StringHelper.GetID(e, 3, 3, 3);
            else res = StringHelper.GetID(e, 0, 3, 3);
            return Json(new {ID=res},JsonRequestBehavior.AllowGet);
        }
        public ActionResult ValidateNavName(string navId, string navName)
        {
            var user = _navService.Single(a => a.NavId != navId && a.NavName == navName);
            if (user == null)
                return Json(true, JsonRequestBehavior.AllowGet);
            return Json("导航名称已经存在！", JsonRequestBehavior.AllowGet);
        }   
        #endregion
    }
}
