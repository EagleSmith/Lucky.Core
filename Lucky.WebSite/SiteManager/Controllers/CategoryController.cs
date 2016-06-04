using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lucky.Core;
using Lucky.Core.Data.Specification;
using Lucky.Core.Utility;
using Lucky.Entity;
using Lucky.IService;
using Lucky.ViewModels;
using Lucky.ViewModels.Models.News;

namespace Lucky.Hr.SiteManager.Controllers
{
    public class CategoryController : BaseAdminController
    {
        private INewsContext _context;
        private ICategoryService _categoryRepository;
        public CategoryController(INewsContext context, ICategoryService categoryRepository)
        {
            _context = context;
            _categoryRepository = categoryRepository;
        }
        // GET: Category
        public ActionResult Index(int pageIndex = 1, string keyword = "")
        {
            var spec = SpecificationBuilder.Create<Category>();
            if (keyword != "")
                spec.Like(a => a.Title, keyword);
            var pagelist = _categoryRepository.GetPaged(spec.Predicate, a => a.CategoryID, pageIndex, 20);
            var models = pagelist.Select(a => { return a.ToModel(); }).ToPagedList(pageIndex, 20, pagelist.TotalCount);
            return View(models);
        }

        public ActionResult Details(string id)
        {
            var entity = _categoryRepository.Single(a => a.CategoryID == id);
           // var navoperlist = _categoryRepository.GetQuery(a => a.NavId == id);
          //  var operlist = _categoryRepository.GetList().Select(a => new SelectListItem() { Text = a.OperationName, Value = a.OperationId.ToString(), Selected = navoperlist.Any(b => a.OperationId == b.OperationId) }).ToList();
            var model = entity.ToModel();

           // model.NavOperationItems = operlist;
            ;
            return View(model);
        }

        public ActionResult Create()
        {
            CategoryViewModel model=new CategoryViewModel();
            model.CreateDate = DateTime.Now;
            GetViewModel(model);
            return View(model);
        }
        [HttpPost]
        public ActionResult Create(CategoryViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var entity = model.ToEntity();
                    _categoryRepository.Add(entity);
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Edit(string id)
        {
            try
            {
                var entity = _categoryRepository.Single(a => a.CategoryID == id);
                var model = entity.ToModel();
                GetViewModel(model);

                return View(model);
            }
            catch (Exception)
            {

            }
            return View();
        }
        [HttpPost]
        public ActionResult Edit(CategoryViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var entity = model.ToEntity();
                    _categoryRepository.Update(entity);
                }

                return RedirectToAction("Index");

            }
            catch
            {
                return View(model);
            }
        }

        public ActionResult Delete(string id)
        {
            _categoryRepository.Delete(a => a.CategoryID == id);
            var redirectUrl = new UrlHelper(Request.RequestContext).Action("Index", "Category");
            return Json(new { Url = redirectUrl }, JsonRequestBehavior.AllowGet);
        }
        #region 私有函数

        private void GetViewModel(CategoryViewModel model)
        {
            model.ParentItems = _categoryRepository.GetList().Select(a => new ListItemEntity()
            {
                ID = a.CategoryID,
                ParentID = a.ParentID,
                Title = a.Title,
                Selected = a.ParentID == model.ParentID

            }).ToList();
            model.ParentItems.Add(new ListItemEntity() { ID = "-1", ParentID = "", Title = "----根节点----" });
        }
        #endregion
        #region Ajax 调用
        public JsonResult GetCategoryNextID(string id)
        {
            string temid, res;
            if (id.Length == 12 || id.Length == 8||id.Length==4)
                temid = id;
            else temid = "-1";
            string e = _categoryRepository.GetQuery().Where(a => a.ParentID == temid).ToList().Max(a => a.CategoryID);
            if (e == null) e = temid + "0000";//二级菜单没有下级时 e 为null
            if (e.Length == 16)
                res = StringHelper.GetID(e, 12, 4, 4);
            else if (e.Length == 12)
                res = StringHelper.GetID(e, 8, 4, 4);
            else if (e.Length == 8)
                res = StringHelper.GetID(e, 4, 4, 4);
            else 
                res = StringHelper.GetID(e, 0, 4, 4);
            return Json(new { ID = res }, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}