using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Lucky.Hr.IService;
using Lucky.Hr.Service;
using Lucky.Hr.ViewModels;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace Lucky.Hr.SiteManager.Controllers
{
    public class HomeController : BaseAdminController
    {
        private INavRepository _navRepository;
        public HomeController(INavRepository nav)
        {
            _navRepository = nav;
        }

        #region KindeEditor
        [HttpPost]
        public ActionResult UploadImage()
        {
            string savePath = "/UploadImages/";
            string saveUrl = "/UploadImages/";
            string fileTypes = "gif,jpg,jpeg,png,bmp";
            int maxSize = 1000000;

            Hashtable hash = new Hashtable();

            HttpPostedFileBase file = Request.Files["imgFile"];
            if (file == null)
            {
                hash = new Hashtable();
                hash["error"] = 1;
                hash["message"] = "请选择文件";
                return Json(hash, "text/html;charset=UTF-8");
            }

            string dirPath = Server.MapPath(savePath);
            if (!Directory.Exists(dirPath))
            {
                hash = new Hashtable();
                hash["error"] = 1;
                hash["message"] = "上传目录不存在";
                return Json(hash, "text/html;charset=UTF-8");
            }

            string fileName = file.FileName;
            string fileExt = Path.GetExtension(fileName).ToLower();

            ArrayList fileTypeList = ArrayList.Adapter(fileTypes.Split(','));

            if (file.InputStream == null || file.InputStream.Length > maxSize)
            {
                hash = new Hashtable();
                hash["error"] = 1;
                hash["message"] = "上传文件大小超过限制";
                return Json(hash, "text/html;charset=UTF-8");
            }

            if (string.IsNullOrEmpty(fileExt) || Array.IndexOf(fileTypes.Split(','), fileExt.Substring(1).ToLower()) == -1)
            {
                hash = new Hashtable();
                hash["error"] = 1;
                hash["message"] = "上传文件扩展名是不允许的扩展名";
                return Json(hash, "text/html;charset=UTF-8");
            }

            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmss_ffff", DateTimeFormatInfo.InvariantInfo) + fileExt;
            string filePath = dirPath + newFileName;
            file.SaveAs(filePath);
            string fileUrl = saveUrl + newFileName;

            hash = new Hashtable();
            hash["error"] = 0;
            hash["url"] = fileUrl;

            return Json(hash, "text/html;charset=UTF-8");
        }
        public ActionResult ProcessRequest()
        {
            //String aspxUrl = context.Request.Path.Substring(0, context.Request.Path.LastIndexOf("/") + 1);

            //根目录路径，相对路径
            String rootPath = "/UploadImages/";
            //根目录URL，可以指定绝对路径，
            String rootUrl = "/UploadImages/";
            //图片扩展名
            String fileTypes = "gif,jpg,jpeg,png,bmp";

            String currentPath = "";
            String currentUrl = "";
            String currentDirPath = "";
            String moveupDirPath = "";

            //根据path参数，设置各路径和URL
            String path = Request.QueryString["path"];
            path = String.IsNullOrEmpty(path) ? "" : path;
            if (path == "")
            {
                currentPath = Server.MapPath(rootPath);
                currentUrl = rootUrl;
                currentDirPath = "";
                moveupDirPath = "";
            }
            else
            {
                currentPath = Server.MapPath(rootPath) + path;
                currentUrl = rootUrl + path;
                currentDirPath = path;
                moveupDirPath = Regex.Replace(currentDirPath, @"(.*?)[^\/]+\/$", "$1");
            }

            //排序形式，name or size or type
            String order = Request.QueryString["order"];
            order = String.IsNullOrEmpty(order) ? "" : order.ToLower();

            //不允许使用..移动到上一级目录
            if (Regex.IsMatch(path, @"\.\."))
            {
                Response.Write("Access is not allowed.");
                Response.End();
            }
            //最后一个字符不是/
            if (path != "" && !path.EndsWith("/"))
            {
                Response.Write("Parameter is not valid.");
                Response.End();
            }
            //目录不存在或不是目录
            if (!Directory.Exists(currentPath))
            {
                Response.Write("Directory does not exist.");
                Response.End();
            }

            //遍历目录取得文件信息
            string[] dirList = Directory.GetDirectories(currentPath);
            string[] fileList = Directory.GetFiles(currentPath);

            switch (order)
            {
                case "size":
                    Array.Sort(dirList, new NameSorter());
                    Array.Sort(fileList, new SizeSorter());
                    break;
                case "type":
                    Array.Sort(dirList, new NameSorter());
                    Array.Sort(fileList, new TypeSorter());
                    break;
                case "name":
                default:
                    Array.Sort(dirList, new NameSorter());
                    Array.Sort(fileList, new NameSorter());
                    break;
            }

            Hashtable result = new Hashtable();
            result["moveup_dir_path"] = moveupDirPath;
            result["current_dir_path"] = currentDirPath;
            result["current_url"] = currentUrl;
            result["total_count"] = dirList.Length + fileList.Length;
            List<Hashtable> dirFileList = new List<Hashtable>();
            result["file_list"] = dirFileList;
            for (int i = 0; i < dirList.Length; i++)
            {
                DirectoryInfo dir = new DirectoryInfo(dirList[i]);
                Hashtable hash = new Hashtable();
                hash["is_dir"] = true;
                hash["has_file"] = (dir.GetFileSystemInfos().Length > 0);
                hash["filesize"] = 0;
                hash["is_photo"] = false;
                hash["filetype"] = "";
                hash["filename"] = dir.Name;
                hash["datetime"] = dir.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss");
                dirFileList.Add(hash);
            }
            for (int i = 0; i < fileList.Length; i++)
            {
                FileInfo file = new FileInfo(fileList[i]);
                Hashtable hash = new Hashtable();
                hash["is_dir"] = false;
                hash["has_file"] = false;
                hash["filesize"] = file.Length;
                hash["is_photo"] = (Array.IndexOf(fileTypes.Split(','), file.Extension.Substring(1).ToLower()) >= 0);
                hash["filetype"] = file.Extension.Substring(1);
                hash["filename"] = file.Name;
                hash["datetime"] = file.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss");
                dirFileList.Add(hash);
            }
            //Response.AddHeader("Content-Type", "application/json; charset=UTF-8");
            //context.Response.Write(JsonMapper.ToJson(result));
            //context.Response.End();
            return Json(result, "text/html;charset=UTF-8", JsonRequestBehavior.AllowGet);
        }
        public class NameSorter : IComparer
        {
            public int Compare(object x, object y)
            {
                if (x == null && y == null)
                {
                    return 0;
                }
                if (x == null)
                {
                    return -1;
                }
                if (y == null)
                {
                    return 1;
                }
                FileInfo xInfo = new FileInfo(x.ToString());
                FileInfo yInfo = new FileInfo(y.ToString());

                return xInfo.FullName.CompareTo(yInfo.FullName);
            }
        }

        public class SizeSorter : IComparer
        {
            public int Compare(object x, object y)
            {
                if (x == null && y == null)
                {
                    return 0;
                }
                if (x == null)
                {
                    return -1;
                }
                if (y == null)
                {
                    return 1;
                }
                FileInfo xInfo = new FileInfo(x.ToString());
                FileInfo yInfo = new FileInfo(y.ToString());

                return xInfo.Length.CompareTo(yInfo.Length);
            }
        }

        public class TypeSorter : IComparer
        {
            public int Compare(object x, object y)
            {
                if (x == null && y == null)
                {
                    return 0;
                }
                if (x == null)
                {
                    return -1;
                }
                if (y == null)
                {
                    return 1;
                }
                FileInfo xInfo = new FileInfo(x.ToString());
                FileInfo yInfo = new FileInfo(y.ToString());

                return xInfo.Extension.CompareTo(yInfo.Extension);
            }
        }
        #endregion

        public async Task<ActionResult> Index()
        {
            var _req = User.Identity;
            //var current_user_role = RoleManager..GetRolesForUser(User.Identity.Name);
            //var _t=_req.
            var s=AuthenticationManager.User.Identities;
            var ci = User.Identity as ClaimsIdentity;
            var role = ci.FindFirst(ClaimTypes.Role);
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            ViewBag.NavList = _navRepository.GetQuery(a=>a.State==1).ToList().Select(a=> { return a.ToModel(); });
            return View();
        }
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }
        private ApplicationRoleManager _roleManager;
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }

        public ActionResult Main()
        {
            return View();
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Help()
        {
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public JsonResult GetMessage()
        {
            var items = new List<object>();
            items.Add(new { url = "ShowMessage/1", target = "", title = "系统升级", type = 0, date = DateTime.Now });
            var result = new { count = 1, items = items };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult AddShortcutMenu(string id, string url, string title)
        {
            var result = new { status = 0, message = "操作成功!", url = "", callback = default(string), data = default(string) };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DelShortcutMenu(string id, string url, string title)
        {
            var result = new { status = 0, message = "操作成功!", url = "", callback = default(string), data = default(string) };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult IndexMenu()
        {
            var menu = new List<object>();
            var tlist1 = new List<object>();
            tlist1.Add(new { url = "/vshop/HomePageTemplate", id = "decoration", title = "首页装饰", flag = false });
            tlist1.Add(new { url = "/vshop/OtherTemplate", id = "otherStyle", title = "其他样式", flag = false });
            tlist1.Add(new { url = "/vshop/MenuSettings", id = "menuset", title = "菜单设置", flag = false });
            var list1 = new List<object>();
            list1.Add(new { name = "", tlist = tlist1 });
            menu.Add(new { list = list1, id = "la", name = "旺铺装扮" });

            var tlist2 = new List<object>();
            tlist2.Add(new { url = "/vshop/Goods/ClassifyList", id = "groundmanage", title = "分组管理", flag = false });
            tlist2.Add(new { url = "/vshop/Goods/GoodsList", id = "goodslist", title = "商品列表", flag = false });
            tlist2.Add(new { url = "/vshop/Goods/Tags", id = "goodslabel", title = "商品标签", flag = false });
            var list2 = new List<object>();
            list2.Add(new { name = "", tlist = tlist2 });
            menu.Add(new { list = list2, id = "lb", name = "商品管理" });

            var tlist3 = new List<object>();
            tlist3.Add(new { url = "/vshop/Order/OrderList", id = "ordermanage", title = "订单管理", flag = false });
            tlist3.Add(new { url = "/vshop/Order/RefundOrderList", id = "activist", title = "维权管理", flag = false });
            tlist3.Add(new { url = "/vshop/Order/OrderEvaluation", id = "commentsmanage", title = "评论管理", flag = false });
            tlist3.Add(new { url = "/vshop/Order/OrderDelivery", id = "deliverygoods", title = "发货管理", flag = false });
            var list3 = new List<object>();
            list3.Add(new { name = "", tlist = tlist3 });
            menu.Add(new { list = list3, id = "lc", name = "订单管理" });

            var tlist4 = new List<object>();
            tlist4.Add(new { url = "/vshop/SystemSet/MarKetingSet/WarmMarketing", id = "warnmarketing", title = "提醒营销", flag = false });
            tlist4.Add(new { url = "/vshop/SystemSet/MarKetingSet/VipMarketing", id = "paymarketing", title = "会员营销", flag = false });
            var list4 = new List<object>();
            list4.Add(new { name = "", tlist = tlist4 });
            menu.Add(new { list = list4, id = "yx", name = "营销管理" });

            var tlist51 = new List<object>();
            tlist51.Add(new { url = "/vshop/SystemSet/SimpleSet/SystemSettings", id = "baseinfo", title = "基本信息", flag = false });
            tlist51.Add(new { url = "/vshop/SystemSet/LogisticsOrderSet/OrderTask", id = "orderset", title = "订单设置", flag = false });
            var tlist52 = new List<object>();
            tlist52.Add(new { url = "/vshop/SystemSet/LogisticsOrderSet/AddressLibraryList", id = "addresslib", title = "地址库", flag = false });
            tlist52.Add(new { url = "/vshop/SystemSet/LogisticsOrderSet/FreightTemplateList", id = "freightmode", title = "运费模版", flag = false });
            tlist52.Add(new { url = "/vshop/SystemSet/LogisticsOrderSet/DeliveryTemplateList", id = "waybill", title = "运单模版设置", flag = false });
            var list5 = new List<object>();
            list5.Add(new { name = "", tlist = tlist51 });
            list5.Add(new { name = "营销管理", tlist = tlist52 });
            menu.Add(new { list = list5, id = "ld", name = "系统设置" });

            return Json(menu, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetOrderCount()
        {
            var order = new List<object>();
            var now = DateTime.Now.Date;
            var list1 = new List<List<long>>();
            for (var i = 7; i > 0; i--)
            {
                var item = new List<long>();
                item.Add(now.AddDays(-i).ToTimeStamp());
                item.Add(i + 3);
                list1.Add(item);
            }
            order.Add(new { name = "下单数", data = list1 });
            var list2 = new List<List<long>>();
            for (var i = 7; i > 0; i--)
            {
                var item = new List<long>();
                item.Add(now.AddDays(-i).ToTimeStamp());
                item.Add(i + 2);
                list2.Add(item);
            }
            order.Add(new { name = "成交数", data = list2 });
            var list3 = new List<List<long>>();
            for (var i = 7; i > 0; i--)
            {
                var item = new List<long>();
                item.Add(now.AddDays(-i).ToTimeStamp());
                item.Add(i + 1);
                list3.Add(item);
            }
            order.Add(new { name = "退单数", data = list3 });
            return Json(order, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetOrderTrade(int type)
        {
            var list = new List<decimal>();
            list.Add(1);
            list.Add(2);
            list.Add(3);
            list.Add(4);
            list.Add(5);
            list.Add(6);
            list.Add(7);
            return Json(list, JsonRequestBehavior.AllowGet);
        }
    }
    public static class ExtensionHelper
    {
        public static long ToTimeStamp(this DateTime input)
        {
            var time = input.ToUniversalTime();
            var ts = new TimeSpan(time.Ticks - 621355968000000000);
            return (long)ts.TotalMilliseconds;
        }
    }
}