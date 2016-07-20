using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Core;
using Autofac.Integration.Mvc;
using Lucky.Core.Cache;
using Lucky.Core.Cache.RedisCache;
using Lucky.Core.Data.Dapper;
using Lucky.Core.Events.EventsBus;
using Lucky.Core.Infrastructure.DependencyManagement;
using Lucky.Core.Logging;
using Lucky.Core.Plugins;
using Lucky.IService;
using Lucky.Service;
using StackExchange.Redis.Extensions.Core;
using StackExchange.Redis.Extensions.Newtonsoft;


namespace Lucky.Web.Framework
{
    /// <summary>
    /// 所有需要注入的接口全在此注入
    /// </summary>
    public class DependencyRegistrar:IDependencyRegistrar
    {
        public void Register(Autofac.ContainerBuilder builder, Core.Infrastructure.ITypeFinder typeFinder)
        {
            //注册控制器
            builder.RegisterControllers(typeFinder.GetAssemblies().ToArray());
            //插件
            builder.RegisterType<PluginFinder>().As<IPluginFinder>().InstancePerLifetimeScope();

            #region 缓存注入
            //本地内存缓存
            builder.RegisterModule(new CacheModule());
            builder.RegisterType<DefaultCacheHolder>().As<ICacheHolder>().SingleInstance();
            //分布式Redis 缓存
            builder.RegisterType<RedisCacheManager>().As<IRedisCacheManager>()
                .WithParameters(
                new[] { new ResolvedParameter((pi, i) => pi.ParameterType == typeof(Type), (pi, i) => GetType()), new ResolvedParameter((pi, i) => pi.ParameterType == typeof(IRedisCacheHolder), (pi, i) => i.Resolve<IRedisCacheHolder>()) }
                ).SingleInstance();
            builder.RegisterType<RedisCacheHolder>().As<IRedisCacheHolder>().SingleInstance();

            builder.RegisterType<DefaultCacheContextAccessor>().As<ICacheContextAccessor>().SingleInstance();
            builder.RegisterType<DefaultParallelCacheContext>().As<IParallelCacheContext>().SingleInstance();
            builder.RegisterType<DefaultAsyncTokenProvider>().As<IAsyncTokenProvider>().SingleInstance();
            builder.RegisterType<RedisSignals>().As<IRedisSignals>().SingleInstance();

            RegisterVolatileProvider<Signals, ISignals>(builder);
            RegisterVolatileProvider<RedisSignals, IRedisSignals>(builder);

            builder.RegisterType<NewtonsoftSerializer>().As<ISerializer>().SingleInstance();
            builder.RegisterType<StackExchangeRedisCacheClient>().As<ICacheClient>().SingleInstance();
            #endregion

            #region 日志模块
            builder.RegisterModule(new LoggingModule());
            #endregion

            #region 注册事件的使用者发布者

            var consumers = typeFinder.FindClassesOfType(typeof(IConsumer<>)).ToList();
            foreach (var consumer in consumers)
            {
                builder.RegisterType(consumer)
                    .As(consumer.FindInterfaces((type, criteria) =>
                    {
                        var isMatch = type.IsGenericType && ((Type)criteria).IsAssignableFrom(type.GetGenericTypeDefinition());
                        return isMatch;
                    }, typeof(IConsumer<>)))
                    .InstancePerLifetimeScope();
            }
            builder.RegisterType<EventPublisher>().As<IEventPublisher>().SingleInstance();
            builder.RegisterType<SubscriptionService>().As<ISubscriptionService>().SingleInstance();
            #endregion

            #region 注册数据操作
            builder.RegisterType<DapperContext>().As<IDbContext>().WithParameter("connectionName", "LuckyNewsContext").InstancePerLifetimeScope();
            builder.RegisterType<HrDbContext>().As<IHrDbContext>().InstancePerLifetimeScope();
            builder.RegisterType<NewsContext>().As<INewsContext>().InstancePerLifetimeScope();

            builder.RegisterType<AchievementService>().As<IAchievementService>().InstancePerLifetimeScope();
            builder.RegisterType<AreaService>().As<IAreaService>().InstancePerLifetimeScope();
            builder.RegisterType<CertService>().As<ICertService>().InstancePerLifetimeScope();
            builder.RegisterType<CertCategoryService>().As<ICertCategoryService>().InstancePerLifetimeScope();
            builder.RegisterType<DepartmentService>().As<IDepartmentService>().InstancePerLifetimeScope();
            builder.RegisterType<DepartmentRoleService>().As<IDepartmentRoleService>().InstancePerLifetimeScope();
            builder.RegisterType<DistributorService>().As<IDistributorService>().InstancePerLifetimeScope();
            builder.RegisterType<DistributorConfigService>().As<IDistributorConfigService>().InstancePerLifetimeScope();
            builder.RegisterType<EducationService>().As<IEducationService>().InstancePerLifetimeScope();
            builder.RegisterType<IndustryService>().As<IIndustryService>().InstancePerLifetimeScope();
            builder.RegisterType<JobCategoryService>().As<IJobCategoryService>().InstancePerLifetimeScope();
            builder.RegisterType<LanguageService>().As<ILanguageService>().InstancePerLifetimeScope();
            builder.RegisterType<LanguageCategoryService>().As<ILanguageCategoryService>().InstancePerLifetimeScope();
            builder.RegisterType<ManagerService>().As<IManagerService>().InstancePerLifetimeScope();
            builder.RegisterType<ManagerFileService>().As<IManagerFileService>().InstancePerLifetimeScope();
            builder.RegisterType<ManagerLogService>().As<IManagerLogService>().InstancePerLifetimeScope();
            builder.RegisterType<ManagerRecordService>().As<IManagerRecordService>().InstancePerLifetimeScope();
            builder.RegisterType<NavService>().As<INavService>().InstancePerLifetimeScope();
            builder.RegisterType<NavOperationService>().As<INavOperationService>().InstancePerLifetimeScope();
            builder.RegisterType<NewsService>().As<INewsService>().InstancePerLifetimeScope();
            builder.RegisterType<NewsTypeService>().As<INewsTypeService>().InstancePerLifetimeScope();
            builder.RegisterType<OperationService>().As<IOperationService>().InstancePerLifetimeScope();
            builder.RegisterType<OtherService>().As<IOtherService>().InstancePerLifetimeScope();
            builder.RegisterType<PersonalService>().As<IPersonalService>().InstancePerLifetimeScope();
            builder.RegisterType<PracticeService>().As<IPracticeService>().InstancePerLifetimeScope();
            builder.RegisterType<ProjectService>().As<IProjectService>().InstancePerLifetimeScope();
            builder.RegisterType<ResumeService>().As<IResumeService>().InstancePerLifetimeScope();
            builder.RegisterType<ResumeAreaService>().As<IResumeAreaService>().InstancePerLifetimeScope();
            builder.RegisterType<ResumeIndustryService>().As<IResumeIndustryService>().InstancePerLifetimeScope();
            builder.RegisterType<ResumeJobCategoryService>().As<IResumeJobCategoryService>().InstancePerLifetimeScope();
            builder.RegisterType<RoleService>().As<IRoleService>().InstancePerLifetimeScope();
            builder.RegisterType<RoleNavService>().As<IRoleNavService>().InstancePerLifetimeScope();
            builder.RegisterType<SkillService>().As<ISkillService>().InstancePerLifetimeScope();
            builder.RegisterType<SkillCategoryService>().As<ISkillCategoryService>().InstancePerLifetimeScope();
            builder.RegisterType<WorkService>().As<IWorkService>().InstancePerLifetimeScope();
            //新闻
            builder.RegisterType<CategoryService>().As<ICategoryService>().InstancePerLifetimeScope();
            builder.RegisterType<LinksService>().As<ILinksService>().InstancePerLifetimeScope();
            builder.RegisterType<NewsArticlesService>().As<INewsArticlesService>().InstancePerLifetimeScope();
            builder.RegisterType<NewsArticleTextService>().As<INewsArticleTextService>().InstancePerLifetimeScope();

            #endregion
        }

        public int Order
        {
            get { return 0; }
        }
        private void RegisterVolatileProvider<TRegister, TService>(ContainerBuilder builder) where TService : IVolatileProvider
        {
            builder.RegisterType<TRegister>()
                .As<TService>()
                .As<IVolatileProvider>()
                .SingleInstance();
        }
    }
}
