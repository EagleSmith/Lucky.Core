using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lucky.Core;
using Lucky.Core.Data.Specification;
using Lucky.Hr.Entity;
using Lucky.Hr.IService;
using Lucky.Hr.ViewModels;
using Lucky.Hr.ViewModels.Models.SiteManager;

namespace Lucky.Hr.SiteManager.Controllers
{
    public class OperationController : BaseAdminController
    {
        private IHrDbContext _context;
        private IOperationService _operationService;
        public OperationController(IHrDbContext context,IOperationService operationService)
        {
            _context = context;
            _operationService = operationService;
        }
        // GET: Operation
        public ActionResult Index(int pageIndex = 1, string keyword = "")
        {
            var spec = SpecificationBuilder.Create<Operation>();
            if (keyword != "")
                spec.Like(a => a.OperationName, keyword);
            var pagelist = _operationService.GetPaged(spec.Predicate, a => a.OperationId, pageIndex, 20);
            var models = pagelist.Select(a => { return a.ToModel(); }).ToPagedList(pageIndex, 20, pagelist.TotalCount);
            return View(models);
        }

        // GET: Operation/Details/5
        public ActionResult Details(int id)
        {
            var entity = _operationService.Single(a => a.OperationId == id);
            var model = entity.ToModel();
            return View(model);
        }

        // GET: Operation/Create
        public ActionResult Create()
        {
            var model = new OperationViewModel();
            return View(model);
        }

        // POST: Operation/Create
        [HttpPost]
        public ActionResult Create(OperationViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var entity = model.ToEntity();
                    _operationService.Add(entity);
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Operation/Edit/5
        public ActionResult Edit(int id)
        {
            var entity = _operationService.Single(a => a.OperationId == id);
            var model = entity.ToModel();
            return View(model);
        }

        // POST: Operation/Edit/5
        [HttpPost]
        public ActionResult Edit(OperationViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var entity = model.ToEntity();
                    _operationService.Update(entity);
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Operation/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                _operationService.Delete(a => a.OperationId == id);
                var redirectUrl = new UrlHelper(Request.RequestContext).Action("Index", "Operation");
                return Json(new { Url = redirectUrl }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        
    }
}
