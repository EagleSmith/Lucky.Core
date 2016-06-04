using System.Web.Mvc;

namespace Lucky.Hr.SiteManager.Controllers
{
    public class UsersController : BaseAdminController
    {
        // GET: Users
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Edit()
        {
            return View();
        }

        public ActionResult Details()
        {
            return View();
        }
    }
}