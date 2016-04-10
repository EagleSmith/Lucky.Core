using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Lucky.Web.Framework.HtmlExtensions
{
    public static class CalendarExtensions
    {
        private static string defaultFormat = "yyyy-MM-dd";

        /// <summary>
        /// 使用特定的名称生成控件
        /// </summary>
        /// <param name="helper">HtmlHelper对象</param>
        /// <param name="name">控件名称</param>
        /// <returns>Html文本</returns>
        public static MvcHtmlString Calendar(this HtmlHelper helper, string name)
        {
            return Calendar(helper, name, defaultFormat);
        }

        /// <summary>
        /// 使用特定的名称生成控件
        /// </summary>
        /// <param name="helper">HtmlHelper对象</param>
        /// <param name="name">控件名称</param>
        /// <param name="format">显示格式</param>
        /// <returns>Html文本</returns>
        public static MvcHtmlString Calendar(this HtmlHelper helper, string name, string format)
        {
            return MvcHtmlString.Create(GenerateHtml(name, null, format));
        }

        /// <summary>
        /// 使用特定的名称和初始值生成控件
        /// </summary>
        /// <param name="helper">HtmlHelper对象</param>
        /// <param name="name">控件名称</param>
        /// <param name="date">要显示的日期时间</param>
        /// <returns>Html文本</returns>
        public static MvcHtmlString Calendar(this HtmlHelper helper, string name, DateTime date)
        {
            return Calendar(helper, name, date, defaultFormat);
        }

        /// <summary>
        /// 使用特定的名称和初始值生成控件
        /// </summary>
        /// <param name="helper">HtmlHelper对象</param>
        /// <param name="name">控件名称</param>
        /// <param name="date">要显示的日期时间</param>
        /// <param name="format">显示格式</param>
        /// <returns>Html文本</returns>
        public static MvcHtmlString Calendar(this HtmlHelper helper, string name, DateTime date, string format)
        {
            return MvcHtmlString.Create(GenerateHtml(name, date, format));
        }

        /// <summary>
        /// 通过lambda表达式生成控件
        /// </summary>
        /// <param name="helper">HtmlHelper对象</param>
        /// <param name="expression">lambda表达式，指定要显示的属性及其所属对象</param>
        /// <returns>Html文本</returns>
        public static MvcHtmlString CalendarFor<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression)
        {
            return CalendarFor(helper, expression, defaultFormat);
        }

        /// <summary>
        /// 通过lambda表达式生成控件
        /// </summary>
        /// <param name="helper">HtmlHelper对象</param>
        /// <param name="expression">lambda表达式，指定要显示的属性及其所属对象</param>
        /// <param name="format">显示格式</param>
        /// <returns>Html文本</returns>
        public static MvcHtmlString CalendarFor<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression, string format)
        {
            string name = ExpressionHelper.GetExpressionText(expression);
            DateTime value;

            object data = ModelMetadata.FromLambdaExpression<TModel, TProperty>(expression, helper.ViewData).Model;
            if (data != null && DateTime.TryParse(data.ToString(), out value))
            {
                return MvcHtmlString.Create(GenerateHtml(name, value, format));
            }
            else
            {
                return MvcHtmlString.Create(GenerateHtml(name, null, format));
            }
        }

        public static MvcHtmlString DatePickerFor<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes, string format = "yyyy-MM-dd")
        {
            string name = ExpressionHelper.GetExpressionText(expression);
            DateTime value;
            IDictionary<string, object> HtmlAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            TagBuilder builder = new TagBuilder("input");
            builder.MergeAttribute("type", "datetime");
            builder.MergeAttribute("id", name);
            builder.MergeAttribute("name", name);
            builder.MergeAttributes(HtmlAttributes);
            builder.MergeAttribute("data-toggle", "datepicker");
            var validation = helper.GetUnobtrusiveValidationAttributes(name);
            foreach (KeyValuePair<string, Object> attribute in validation)
            {
                builder.MergeAttribute(attribute.Key, attribute.Value.ToString());
            }
            object data = ModelMetadata.FromLambdaExpression<TModel, TProperty>(expression, helper.ViewData).Model;
            string date;
            
            if (data != null && DateTime.TryParse(data.ToString(), out value))
            {
                date=(value.ToString(format));
            }
            else
            {
                date=("");
            }
            builder.MergeAttribute("value", date);
           // string tem = "<input class=\"form-control text-box single-line\" "+str+"  data-toggle=\"datepicker\" id=\"" + name + "\" name=\"" + name + "\" type=\"datetime\" value=\"" + date + "\" />";
            return MvcHtmlString.Create(builder.ToString(TagRenderMode.SelfClosing));
        }

        /// <summary>
        /// 通过lambda表达式获取要显示的日期时间
        /// </summary>
        /// <param name="helper">HtmlHelper对象</param>
        /// <param name="expression">lambda表达式，指定要显示的属性及其所属对象</param>
        /// <param name="format">显示格式</param>
        /// <returns>Html文本</returns>
        public static MvcHtmlString CalendarDisplayFor<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression, string format)
        {
            string name = ExpressionHelper.GetExpressionText(expression);
            DateTime value;

            object data = ModelMetadata.FromLambdaExpression<TModel, TProperty>(expression, helper.ViewData).Model;
            if (data != null && DateTime.TryParse(data.ToString(), out value))
            {
                return MvcHtmlString.Create(value.ToString(format));
            }
            else
            {
                return new MvcHtmlString("");
            }
        }

        /// <summary>
        /// 通过lambda表达式获取要显示的日期时间
        /// </summary>
        /// <param name="helper">HtmlHelper对象</param>
        /// <param name="expression">lambda表达式，指定要显示的属性及其所属对象</param>
        /// <returns>Html文本</returns>
        public static MvcHtmlString CalendarDisplayFor<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression)
        {
            return CalendarDisplayFor(helper, expression, defaultFormat);
        }

        /// <summary>
        /// 生成输入框的Html
        /// </summary>
        /// <param name="name">calendar的名称</param>
        /// <param name="date">calendar的值</param>
        /// <returns>html文本</returns>
        private static string GenerateHtml(string name, DateTime? date, string format)
        {
            if (date != null)
            {
                return "<div class=\"input-prepend input-group\"><input  type=\"text\" id=\"" + name + "\" name=\"" + name + "\" onfocus=\"WdatePicker({skin:'whyGreen',isShowWeek:true,dateFmt:'" + format + "'})\" class=\"span10 form-control\" value=\"" + date.Value.ToString(format) + "\" /><span class=\"add-on input-group-addon\"><i class=\"icon-calendar\"></i></span></div>";
            }
            else
            {
                return "<div class=\"input-prepend\"><input type=\"text\" id=\"" + name + "\" name=\"" + name + "\" onfocus=\"WdatePicker({skin:'whyGreen',isShowWeek:true,dateFmt:'" + format + "'})\" class=\"span10 form-control\" value=\"\" /><span class=\"add-on\"><i class=\"icon-calendar\"></i></span></div>";
            }
        }
    }
}
