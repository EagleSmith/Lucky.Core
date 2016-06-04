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
using Lucky.Hr.IService;
using Lucky.Hr.ViewModels;
using Lucky.Hr.ViewModels.Models.News;

namespace Lucky.Hr.SiteManager.Controllers
{
    public class NewsArticlesController : BaseAdminController
    {
        // GET: NewsArticles
        private INewsContext _context;
        private INewsArticlesService _newsArticlesRepository;
        private ICategoryService _categoryRepository;
        private ILogger _logger;
        private INewsArticleTextService _articleText;
        public NewsArticlesController(ILogger logger,INewsContext context, INewsArticlesService newsArticlesRepository,ICategoryService categoryRepository, INewsArticleTextService articleText)
        {
            _logger = logger;
            _context = context;
            _newsArticlesRepository = newsArticlesRepository;
            _categoryRepository = categoryRepository;
            _articleText = articleText;
        }
        public ActionResult Index(int pageIndex = 1, string keyword = "")
        {

            var spec = SpecificationBuilder.Create<NewsArticle>();
            if (keyword != "")
                spec.Like(a => a.Title, keyword);
            var pagelist = _newsArticlesRepository.GetPaged(spec.Predicate, a => a.CreateDate, pageIndex, 20);
            var models = pagelist.Select(a => { return a.ToModel(); }).ToPagedList(pageIndex, 20, pagelist.TotalCount);
            return View(models);
        }

        public ActionResult Details(string id)
        {
            var entity = _newsArticlesRepository.Single(a => a.ArticleID == new Guid(id));
            var model = entity.ToModel();
            model.CategoryTitle = CategoryTitle(entity.CategoryID);
           // GetViewModel(model);
            return View(model);
        }

        private string CategoryTitle(string id)
        {
            var entity = _categoryRepository.Single(a => a.CategoryID == id);
            if (entity!=null)
            {
                return entity.Title;
            }
            return "";
        }

        public ActionResult Create()
        {
            NewsArticlesViewModel model = new NewsArticlesViewModel();
            model.CreateDate = DateTime.Now;
            model.UpdateDate = DateTime.Now;
            GetViewModel(model);
            return View(model);
        }
        [HttpPost]
        public ActionResult Create(NewsArticlesViewModel model)
        {
            try
            {
                model.ArticleID = Guid.NewGuid();
                if (ModelState.IsValid)
                {
                    var entity = model.ToEntity();
                    var text = new NewsArticleText();
                    text.ArticleID = model.ArticleID;
                    text.ArticleText = model.ArticleText;
                    entity.NewsArticleTexts.Add(text);
                    _newsArticlesRepository.Add(entity);
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Edit(Guid id)
        {
            try
            {
                var entity = _newsArticlesRepository.Single(a => a.ArticleID ==id );
                var model = entity.ToModel();
                GetViewModel(model);
                var entityText = _articleText.Single(a => a.ArticleID == id);
                if (entityText != null)
                {
                    model.ArticleText = entityText.ArticleText;
                }
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }
            return View();
        }
        [HttpPost]
        public ActionResult Edit(NewsArticlesViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var entity = model.ToEntity();
                    Guid id = model.ArticleID;
                    var textentity = _articleText.Single(a => a.ArticleID == id);
                    textentity.ArticleID = entity.ArticleID;
                    textentity.ArticleText = model.ArticleText;
                    
                    _newsArticlesRepository.Update(entity,false);
                    _articleText.Update(textentity,false);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                
            }

            return RedirectToAction("Index");
        }
        
        public ActionResult DeleteMore(string ids)
        {
            try
            {
                _newsArticlesRepository.DeleteMore(ids);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }
            
            var redirectUrl = new UrlHelper(Request.RequestContext).Action("Index", "NewsArticles");
            return Json(new { Url = redirectUrl }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Delete(Guid id)
        {
            _newsArticlesRepository.Delete(a => a.ArticleID == id);
            var redirectUrl = new UrlHelper(Request.RequestContext).Action("Index", "NewsArticles");
            return Json(new { Url = redirectUrl }, JsonRequestBehavior.AllowGet);
        }

        #region 私有函数

        private void GetViewModel(NewsArticlesViewModel model)
        {
            model.CategoryItems = _categoryRepository.GetList().Select(a => new ListItemEntity()
            {
                ID = a.CategoryID,
                ParentID = a.ParentID,
                Title = a.Title,
                Selected = a.ParentID == model.CategoryID

            }).ToList();
        }
        #endregion
    }
}