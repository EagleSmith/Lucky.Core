using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lucky.Core;
using Lucky.Core.Data.Specification;
using Lucky.Core.Logging;
using Lucky.Core.Utility;
using Lucky.Entity;
using Lucky.IService;
using Lucky.ViewModels;
using Lucky.ViewModels.Models.SiteManager;

namespace Lucky.Hr.SiteManager.Controllers
{
    public class AspNetRolesController : BaseAdminController
    {
        private IHrDbContext _context;
        private IRoleService _roleService;
        private IDistributorService _distributorService;
        private INavService _navService;
        private IRoleNavService _roleNavService;
        private ILogger _logger;
        public AspNetRolesController(IHrDbContext context,ILogger logger,IRoleService roleService,IDistributorService distributor,INavService navService,IRoleNavService roleNavService)
        {
            _context = context;
            _roleService = roleService;
            _distributorService = distributor;
            _navService = navService;
            _roleNavService = roleNavService;
            _logger = logger;
        }
        // GET: AspNetRoles
        public ActionResult Index(int pageIndex = 1, string keyword = "")
        {
            var spec = SpecificationBuilder.Create<Role>();
            if (keyword != "")
                spec.Like(a => a.RoleName, keyword);
            var pagelist = _roleService.GetPaged(spec.Predicate, a => a.Id, pageIndex, 20);
            var models = pagelist.Select(a => { return a.ToModel(); }).ToPagedList(pageIndex, 20, pagelist.TotalCount);
            return View(models);

        }

        // GET: AspNetRoles/Details/5
        public ActionResult Details(string id)
        {
            var entity = _roleService.Single(a => a.Id == id);
            var model = entity.ToModel();
            int disid = entity.DistributorId;
            var disentity = _distributorService.Single(a => a.DistributorId == disid);
            model.DistributorName = disentity!=null?disentity.DistributionName:"";
            model.OperationViewModels = _roleService.GetNavOperationViewModels(id);
            return View(model);
        }

        // GET: AspNetRoles/Create
        public ActionResult Create()
        {
            AspNetRolesViewModel model=new AspNetRolesViewModel();
            GetViewModel(model);
            model.Id = Guid.NewGuid().ToString();
            return View(model);
        }

        // POST: AspNetRoles/Create
        [HttpPost]
        public ActionResult Create(AspNetRolesViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var entity = model.ToEntity();
                    entity.Name = model.RoleName;
                    _roleService.Add(entity);
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: AspNetRoles/Edit/5
        public ActionResult Edit(string id)
        {
            var entity = _roleService.Single(a => a.Id == id);
            var model = entity.ToModel();
            GetViewModel(model);
            return View(model);
        }

        // POST: AspNetRoles/Edit/5
        [HttpPost]
        public ActionResult Edit(AspNetRolesViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var entity = model.ToEntity();
                    entity.Name = model.RoleName;
                    _roleService.Update(entity);
                }


                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: AspNetRoles/Delete/5
        public ActionResult Delete(string id)
        {
            _roleService.Delete(a => a.Id == id);
            var redirectUrl = new UrlHelper(Request.RequestContext).Action("Index", "AspNetRoles");
            return Json(new { Url = redirectUrl }, JsonRequestBehavior.AllowGet);
            
        }

        public ActionResult RoleOperation(string id)
        {
            IEnumerable<NavOperationViewModel> models = _roleService.GetNavOperationViewModels(id);
            ViewBag.RoleId = id;
            return View(models);
        }
        [HttpPost]
        public ActionResult RoleOperation(FormCollection from)
        {
            List<RoleNav> list = new List<RoleNav>();
            var l = from;
            string roleid = from["RoleId"];
            try
            {
                
                from.Remove("RoleId");
                _roleNavService.Delete(a => a.RoleId == roleid, false);
                if (from.Keys.Count > 0)
                {
                    foreach (string k in from.Keys)
                    {
                        var arr = from[k].Split(',');
                        for (int i = 0; i < arr.Length; i++)
                        {
                            RoleNav entity = new RoleNav();
                            entity.NavId = k;
                            entity.RoleId = roleid;
                            entity.OperationId = Convert.ToInt32(arr[i]);
                            list.Add(entity);

                        }
                    }
                }
                _roleNavService.AddRange(list, false);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.Debug(ex,"添加角色权限出现异常！");
            }
            
            return RedirectToAction("Details",new{id=roleid});
        }
        #region 私有函数

        private void GetViewModel(AspNetRolesViewModel model)
        {
            
            model.DistributorListItems = _distributorService.GetList().Select(a=>new ListItemEntity()
            {
                ID = a.DistributorId.ToString(),
                ParentID = "",
                Title = a.DistributionName,
                Selected = a.DistributorId==model.DistributorId
            }).ToList();
        }

        #endregion
    }
}
