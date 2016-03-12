using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Lucky.Hr.Core.Utility;

namespace Lucky.Hr.Web.Framework.HtmlExtensions
{
    public static class HtmlHelperExtensions
    {
        public static MvcHtmlString RegisterScript(this  HtmlHelper htmlHelper, string str)
        {
            string s = "<script>alert('" + str + "');</script>";
            return MvcHtmlString.Create(s);
        }
        public static MvcHtmlString Button(this HtmlHelper helper, string innerHtml, string action, string controller, object htmlAttributes)
        {
            var a = (new UrlHelper(helper.ViewContext.RequestContext)).Action(action, controller, null);
            var builder = new TagBuilder("button");
            builder.InnerHtml = innerHtml;
            builder.Attributes.Add("onclick", "location.href='" + a + "'");
            builder.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
            return MvcHtmlString.Create(builder.ToString());
        }
        public static MvcHtmlString Button(this HtmlHelper helper, string innerHtml, object htmlAttributes)
        {
            return Button(helper, innerHtml, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        public static MvcHtmlString DropdownlistTrueOrFalse<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes=null)
        {
            List<SelectListItem> slist = new List<SelectListItem>();
            slist.Add(new SelectListItem(){Value = "false",Text = "否",Selected = true});
            slist.Add(new SelectListItem() { Value = "true", Text = "是"});
            string name = ExpressionHelper.GetExpressionText(expression);
            return helper.DropDownList(name, slist, htmlAttributes);
        }
        public static MvcHtmlString Button(this HtmlHelper helper, string innerHtml, IDictionary<string, object> htmlAttributes)
        {
            var builder = new TagBuilder("button");
            builder.InnerHtml = innerHtml;
            builder.MergeAttributes(htmlAttributes);
            return MvcHtmlString.Create(builder.ToString());
        }

        public static MvcHtmlString DropDownListTreeFor<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression, List<ListItemEntity> list, string pid, bool flag = true, object htmlAttributes = null)
        {
            string name = ExpressionHelper.GetExpressionText(expression);
            return helper.DropDownListTree(name, list, pid, flag, htmlAttributes);
        }
        public static MvcHtmlString DropDownListTree(this HtmlHelper helper, string name, List<ListItemEntity> list, string pid, bool flag = true, object htmlAttributes = null)
        {
            List<SelectListItem> slist = new List<SelectListItem>();
            if (flag)
            {
                slist.Add(new SelectListItem() { Text = "-----请选择----", Value = "0000", Selected = true });
            }
            foreach (ListItemEntity entity in list.Where(a => a.ParentID == pid))
            {
                slist.Add(new SelectListItem() { Text = entity.Title, Value = entity.ID, Selected = entity.Selected });
                BuildTree(list, slist, entity.ID, 1);
            }

            return helper.DropDownList(name, slist, htmlAttributes);
        }
        private static void BuildTree(List<ListItemEntity> list, List<SelectListItem> slist, string pid, int i)
        {
            var _list = list.Where(a => a.ParentID == pid);
            foreach (ListItemEntity entity in _list)
            {
                string pre = " ";
                for (int j = 0; j < i; j++)
                {
                    if (j == 0)
                    {
                        pre = pre + "┣";
                    }
                    else
                    {
                        pre = "　" + pre;
                    }
                }
                slist.Add(new SelectListItem { Text = pre + entity.Title, Value = entity.ID, Selected = entity.Selected });
                BuildTree(list, slist, entity.ID, i + 1);
            }
        }

        public static MvcHtmlString TextTrueOrFalseFor<TM, TP>(this HtmlHelper<TM> helper, Expression<Func<TM, TP>> expression)
        {
            string str = ModelMetadata.FromLambdaExpression(expression, helper.ViewData).SimpleDisplayText;
            string temp = "";
            if (str == "False")
            {
                temp = "<text style='color:#FF0000'>否</text>";
            }
            else
            {
                temp = "<text style='color:#008000'>是</text>";
            }
            return MvcHtmlString.Create(temp);
        }
    }
    
}
