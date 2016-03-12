using System.Linq;
using System.Web.Mvc;
using Lucky.Hr.Core.Utility;
using Lucky.Hr.IService;
using Lucky.Hr.ViewModels;
using Lucky.Hr.ViewModels.Models.SiteManager;

namespace Lucky.Hr.SiteManager.Controllers
{
    public class DepartmentController:BaseAdminController
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IDistributorRepository _distributorRepository;
        
        public DepartmentController(IDepartmentRepository department,IDistributorRepository distributor)
        {
            _departmentRepository = department;
            _distributorRepository = distributor;
        }
        // GET: Department
        public ActionResult Index(int pageIndex=1,string keyword="")
        {
            var models = _departmentRepository.GetList(pageIndex, keyword);
            return View(models);
        }

        public ActionResult Create()
        {
            var model = new DepartmentViewModel();
            model.Sort = 1;
            SetModel(model);
            return View(model);
        }

        public ActionResult Edit(string id)
        {
            var entity = _departmentRepository.Single(a => a.DepartmentId == id);
            var model = entity.ToModel();
            SetModel(model);
            return View(model);
        }
        [HttpPost]
        public ActionResult Create(DepartmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = model.ToEntity();
                _departmentRepository.Add(entity);
                return RedirectToAction("Index");
            }
            return View();
        }
        [HttpPost]
        public ActionResult Edit(DepartmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                string id = model.DepartmentId;
                var entity = _departmentRepository.Single(a => a.DepartmentId == id);
                entity = model.ToEntity(entity);
                _departmentRepository.Update(entity);
                return RedirectToAction("Index");
            }
            return View();
        }
        public ActionResult Detail(string id)
        {
            var entity = _departmentRepository.Single(a => a.DepartmentId == id);
            var model = entity.ToModel();
            model.ParentName = entity.Parent!=null?entity.Parent.DepartmentName:"无";
            return View(model);
        }

        public ActionResult Delete(string id)
        {
            var entity=_departmentRepository.Single(a => a.DepartmentId == id);
            if (entity != null)
            {
                _departmentRepository.Delete(entity);
                var redirectUrl = new UrlHelper(Request.RequestContext).Action("Index", "Department");
                return Json(new {Url = redirectUrl}, JsonRequestBehavior.AllowGet);
            }
            return RedirectToAction("Index");
        }
        #region 私有方法

        private void SetModel(DepartmentViewModel model)
        {
            model.DepartmentListItems = _departmentRepository.Find(a => true).Select(a => new ListItemEntity()
            {
                ID = a.DepartmentId,
                ParentID = a.ParentId,
                Title = a.DepartmentName,
                Selected = (a.DepartmentId==model.DepartmentId)
            }).ToList();
            model.DistributorListItems = _distributorRepository.Find(a=>true).Select(a => new SelectListItem()
            {
                Value = a.DistributorId.ToString(),
                Text = a.DistributionName,
                Selected = (a.DistributorId==model.DistributorId)
            }).ToList();
        }
        #endregion
    }
}