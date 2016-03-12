using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Lucky.Hr.Web.Framework.HtmlExtensions
{
    public static class CheckBoxListExtensions
    {
        public static MvcHtmlString CheckBoxList(this HtmlHelper helper, string name, IEnumerable<SelectListItem> selectList)
        {
            return CheckBoxList(helper, name, selectList, new { });
        }
        public static MvcHtmlString CheckBoxList(this HtmlHelper helper, string name, IEnumerable<SelectListItem> selectList, object htmlAttributes)
        {

            IDictionary<string, object> HtmlAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);

            HashSet<string> set = new HashSet<string>();
            List<SelectListItem> list = new List<SelectListItem>();
            HtmlAttributes.Add("type", "checkbox");
            HtmlAttributes.Add("id", name);
            HtmlAttributes.Add("name", name);

            StringBuilder stringBuilder = new StringBuilder();

            foreach (SelectListItem selectItem in selectList)
            {
                IDictionary<string, object> newHtmlAttributes = HtmlAttributes.DeepCopy();
                newHtmlAttributes.Add("value", selectItem.Value);
                if (selectItem.Selected)
                {
                    newHtmlAttributes.Add("checked", "checked");
                }

                TagBuilder tagBuilder = new TagBuilder("input");
                tagBuilder.MergeAttributes<string, object>(newHtmlAttributes);
                string inputAllHtml = tagBuilder.ToString(TagRenderMode.SelfClosing);
                stringBuilder.AppendFormat(@"<label style=""margin:0 0 10px 10px;""> {0}  {1}</label>",
                   inputAllHtml, selectItem.Text);
            }
            return MvcHtmlString.Create(stringBuilder.ToString());

        }
        private static IDictionary<string, object> DeepCopy(this IDictionary<string, object> ht)
        {
            Dictionary<string, object> _ht = new Dictionary<string, object>();

            foreach (var p in ht)
            {
                _ht.Add(p.Key, p.Value);
            }
            return _ht;
        }
    }
}
