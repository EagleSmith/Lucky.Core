using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using FluentValidation.Mvc;
using Lucky.Core.Infrastructure;
using Lucky.ViewModels;
using Lucky.Web.Framework;
using Lucky.Web.Framework.MVC;
using Lucky.Web.Framework.Themes;

namespace Lucky.Hr.WebSite
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.NameIdentifier;

            EngineContext.Initialize(false);
            //注入相关接口
            var dependencyResolver = new HrDependencyResolver();
            DependencyResolver.SetResolver(dependencyResolver);  


            AreaRegistration.RegisterAllAreas();
            //验证信息注册
           
            FluentValidationModelValidationFactory validationFactory = (metadata, context, rule, validator) => new RemoteFluentValidationPropertyValidator(metadata, context, rule, validator);
            var validationProvider = new FluentValidationModelValidatorProvider(new LuckyValidatorFactory());
            validationProvider.Add(typeof(RemoteValidator), validationFactory);
            ModelValidatorProviders.Providers.Add(validationProvider);
            DataAnnotationsModelValidatorProvider.AddImplicitRequiredAttributeForValueTypes = false;

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new ThemeViewEngine());
        }
    }
}
