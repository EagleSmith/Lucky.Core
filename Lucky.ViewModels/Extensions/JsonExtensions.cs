using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Newtonsoft.Json;

namespace Lucky.ViewModels.Extensions
{
    public static class JsonExtensions
    {
        public static HtmlString Json<T, TData>(this HtmlHelper<T> helper, TData data)
        {
            return Json(helper, data, new RouteValueDictionary());
        }

        public static HtmlString Json<T, TData>(this HtmlHelper<T> helper, TData data, object htmlAttributes)
        {
            return Json(helper, data, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        public static HtmlString Json<T, TData>(this HtmlHelper<T> helper, TData data, IDictionary<string, object> htmlAttributes)
        {
            var builder = new TagBuilder("script");
            builder.Attributes["type"] = "application/json";
            builder.MergeAttributes(htmlAttributes);
            builder.InnerHtml =
                (data is JsonString
                    ? data.ToString()
                    : JsonConvert.SerializeObject(data))
                .Replace("<", "\u003C").Replace(">", "\u003E");

            return helper.Tag(builder);
        }

       
        public static HtmlString Tag(this HtmlHelper htmlHelper, TagBuilder tagBuilder)
        {
            return htmlHelper.Raw(tagBuilder.ToString()) as HtmlString;
        }
    }

    public class JsonString
    {
        public JsonString(object value)
            : this(value.ToString())
        {

        }

        public JsonString(string value)
        {
            Value = value;
        }

        public string Value { get; private set; }

        public override string ToString()
        {
            return Value;
        }
    }
}
