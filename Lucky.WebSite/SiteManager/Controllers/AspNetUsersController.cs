using System;
using System.Collections.Generic;

using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lucky.Core;
using Lucky.Core.Data.Specification;
using Lucky.Entity;
using Lucky.IService;
using Lucky.Service;
using Lucky.ViewModels;
using Lucky.ViewModels.Models.SiteManager;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace Lucky.Hr.SiteManager.Controllers
{
    public class AspNetUsersController : BaseAdminController
    {
        private IHrDbContext _context;
        private IManagerService _managerService;
        public AspNetUsersController(IHrDbContext context,IManagerService managerService)
        {
            _context = context;
            _managerService = managerService;
        }
        // GET: AspNetUsers
        public ActionResult Index(int pageIndex = 1, string keyword = "")
        {
            var spec = SpecificationBuilder.Create<Manager>();
            if (keyword != "")
                spec.Like(a => a.FullName, keyword);
            var pagelist = _managerService.GetPaged(spec.Predicate, a => a.AddDate, pageIndex,20);
            var models = pagelist.Select(a=> { return a.ToModel(); }).ToPagedList(pageIndex,20,pagelist.TotalCount);
            return View(models);
        }

        // GET: AspNetUsers/Details/5
        public ActionResult Details(string id)
        {
            var entity = _managerService.Single(a => a.Id == id);
            var model = entity.ToModel();
            ViewBag.ChangePassWord = new ChangePasswordViewModel();
            return View(model);
        }

        // GET: AspNetUsers/Create
        public ActionResult Create()
        {
            var model = new AspNetUsersViewModel();
            model.AddDate = DateTime.Now;
            model.LockoutEndDateUtc = DateTime.Now;
            return View(model);
        }

        // POST: AspNetUsers/Create
        [HttpPost]
        public ActionResult Create(AspNetUsersViewModel model)
        {
            try
            {
                model.Id = Guid.NewGuid().ToString();
                model.LastLoginDate = DateTime.Now;
                model.LastLoginIp = "127.0.0.1";
                model.LastModify = DateTime.Now;
                model.AddDate = DateTime.Now;
                model.EmailConfirmed = false;
                model.PhoneNumberConfirmed = false;
                model.TwoFactorEnabled = false;

                if (ModelState.IsValid)
                {
                    var entity = model.ToEntity();
                    var pass = model.Password;
                    entity.LockoutEnabled = false;
                    entity.TwoFactorEnabled = false;
                    var result = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>().Create(entity, pass);
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }
                if (!ModelState.IsValid) return View();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: AspNetUsers/Edit/5
        public ActionResult Edit(string id)
        {
            var entity = _managerService.Single(a => a.Id == id);
            var model = entity.ToModel();
            return View(model);
        }

        // POST: AspNetUsers/Edit/5
        [HttpPost]
        public ActionResult Edit(AspNetUsersViewModel model)
        {
            try
            {
               
                
                model.EmailConfirmed = false;
                model.PhoneNumberConfirmed = false;
                model.TwoFactorEnabled = false;
                ModelState.Remove("PassWord");
                if (ModelState.IsValid)
                {
                    string id = model.Id;
                    var entity = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(id);
                    entity = model.ToEntity(entity);
                    entity.LockoutEnabled = false;
                    entity.TwoFactorEnabled = false;
                    var result = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>().Update(entity);
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }
                if (!ModelState.IsValid) return View();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: AspNetUsers/Delete/5
        public ActionResult Delete(int id)
        {
            return RedirectToAction("Index");
        }

        // POST: AspNetUsers/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        public ActionResult ChangePassword(string id)
        {
            ViewBag.ID = id;
            return PartialView("_ChangePassword");
        }
        [HttpPost]
        public ActionResult ChangePassword(string id,ChangePasswordViewModel model)
        {
            try
            {
                ChangePasswordViewModelFluentValidation validator = new ChangePasswordViewModelFluentValidation();
                FluentValidation.Results.ValidationResult result = validator.Validate(model);

                if (!result.IsValid)
                {
                    result.Errors.ToList().ForEach(error =>
                    {
                        ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                    });
                }

                if (ModelState.IsValid)
                {
                    return Json(new { success = true });
                }
            }
            catch (Exception)
            {
                
                throw;
            }
            return PartialView("_ChangePassword", model);//Json(false,JsonRequestBehavior.AllowGet); // RedirectToAction("Details", new {id = id});;////null
        }
    }
}
