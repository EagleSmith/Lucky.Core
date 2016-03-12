using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using FluentValidation.Internal;
using FluentValidation.Mvc;
using FluentValidation.Validators;

namespace Lucky.Hr.ViewModels
{
    public class RemoteValidator : PropertyValidator
    {
        public string Url { get; private set; }
        public string HttpMethod { get; private set; }
        public string AdditionalFields { get; private set; }

        public RemoteValidator(string errorMessage,
            string action,
            string controller,
            HttpVerbs HttpVerb = HttpVerbs.Get,
            string additionalFields = "")
            : base(errorMessage)
        {
            var httpContext = HttpContext.Current;

            if (httpContext == null)
            {
                var request = new HttpRequest("/", "http://www.luckearth.cn", "");
                var response = new HttpResponse(new StringWriter());
                httpContext = new HttpContext(request, response);
            }

            var httpContextBase = new HttpContextWrapper(httpContext);
            var routeData = new RouteData();
            var requestContext = new RequestContext(httpContextBase, routeData);

            var helper = new UrlHelper(requestContext);

            Url = helper.Action(action, controller);
            HttpMethod = HttpVerb.ToString();
            AdditionalFields = additionalFields;
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            //This is not a server side validation rule. So, should not effect at the server side.  
            return true;
        }
    }

    public class RemoteFluentValidationPropertyValidator : FluentValidationPropertyValidator
    {
        private RemoteValidator RemoteValidator
        {
            get { return (RemoteValidator)Validator; }
        }
        public RemoteFluentValidationPropertyValidator(ModelMetadata metadata,
            ControllerContext controllerContext,
            PropertyRule propertyDescription,
            IPropertyValidator validator)
            : base(metadata, controllerContext, propertyDescription, validator)
        {
            ShouldValidate = false;
        }
        public override IEnumerable<ModelClientValidationRule> GetClientValidationRules()
        {
            if (!ShouldGenerateClientSideRules()) yield break;
            var formatter = new MessageFormatter().AppendPropertyName(base.Rule.DisplayName.ResourceName);
            string message = formatter.BuildMessage(RemoteValidator.ErrorMessageSource.GetString());
            yield return new ModelClientValidationRemoteRule(message, RemoteValidator.Url, RemoteValidator.HttpMethod, RemoteValidator.AdditionalFields);
        }
    }
}
