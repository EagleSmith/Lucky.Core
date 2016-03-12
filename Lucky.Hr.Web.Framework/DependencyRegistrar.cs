using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Core;
using Autofac.Integration.Mvc;
using Lucky.Hr.Caching;
using Lucky.Hr.Core.Cache.RedisCache;
using Lucky.Hr.Core.Events.EventsBus;
using Lucky.Hr.Core.Infrastructure.DependencyManagement;
using Lucky.Hr.Core.Logging;
using Lucky.Hr.Core.Plugins;
using Lucky.Hr.IService;
using Lucky.Hr.Service;
using StackExchange.Redis.Extensions.Core;
using StackExchange.Redis.Extensions.Newtonsoft;


namespace Lucky.Hr.Web.Framework
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
            builder.RegisterModule(new CacheModule());
           
            builder.RegisterType<DefaultCacheManager>().Keyed<ICacheManager>("RedisCacheManager")
                .WithParameters(
                new[] { new ResolvedParameter((pi, i) => pi.ParameterType == typeof(Type), (pi, i) => GetType()), new ResolvedParameter((pi, i) => pi.ParameterType == typeof(ICacheHolder), (pi, i) => i.ResolveNamed<ICacheHolder>("RedisCache")) }
                ).SingleInstance();
            builder.RegisterType<RedisCacheHolder>().Keyed<ICacheHolder>("RedisCache").SingleInstance();
            builder.RegisterType<DefaultCacheHolder>().As<ICacheHolder>().SingleInstance();
            builder.RegisterType<DefaultCacheContextAccessor>().As<ICacheContextAccessor>().SingleInstance();
            builder.RegisterType<DefaultParallelCacheContext>().As<IParallelCacheContext>().SingleInstance();
            builder.RegisterType<DefaultAsyncTokenProvider>().As<IAsyncTokenProvider>().SingleInstance();
            
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
            builder.RegisterType<HrDbContext>().As<IHrDbContext>().InstancePerLifetimeScope();
            builder.RegisterType<NewsContext>().As<INewsContext>().InstancePerLifetimeScope();

            builder.RegisterType<AchievementRepository>().As<IAchievementRepository>().InstancePerLifetimeScope();
            builder.RegisterType<AreaRepository>().As<IAreaRepository>().InstancePerLifetimeScope();
            builder.RegisterType<CertRepository>().As<ICertRepository>().InstancePerLifetimeScope();
            builder.RegisterType<CertCategoryRepository>().As<ICertCategoryRepository>().InstancePerLifetimeScope();
            builder.RegisterType<DepartmentRepository>().As<IDepartmentRepository>().InstancePerLifetimeScope();
            builder.RegisterType<DepartmentRoleRepository>().As<IDepartmentRoleRepository>().InstancePerLifetimeScope();
            builder.RegisterType<DistributorRepository>().As<IDistributorRepository>().InstancePerLifetimeScope();
            builder.RegisterType<DistributorConfigRepository>().As<IDistributorConfigRepository>().InstancePerLifetimeScope();
            builder.RegisterType<EducationRepository>().As<IEducationRepository>().InstancePerLifetimeScope();
            builder.RegisterType<IndustryRepository>().As<IIndustryRepository>().InstancePerLifetimeScope();
            builder.RegisterType<JobCategoryRepository>().As<IJobCategoryRepository>().InstancePerLifetimeScope();
            builder.RegisterType<LanguageRepository>().As<ILanguageRepository>().InstancePerLifetimeScope();
            builder.RegisterType<LanguageCategoryRepository>().As<ILanguageCategoryRepository>().InstancePerLifetimeScope();
            builder.RegisterType<ManagerRepository>().As<IManagerRepository>().InstancePerLifetimeScope();
            builder.RegisterType<ManagerFileRepository>().As<IManagerFileRepository>().InstancePerLifetimeScope();
            builder.RegisterType<ManagerLogRepository>().As<IManagerLogRepository>().InstancePerLifetimeScope();
            builder.RegisterType<ManagerRecordRepository>().As<IManagerRecordRepository>().InstancePerLifetimeScope();
            builder.RegisterType<NavRepository>().As<INavRepository>().InstancePerLifetimeScope();
            builder.RegisterType<NavOperationRepository>().As<INavOperationRepository>().InstancePerLifetimeScope();
            builder.RegisterType<NewsRepository>().As<INewsRepository>().InstancePerLifetimeScope();
            builder.RegisterType<NewsTypeRepository>().As<INewsTypeRepository>().InstancePerLifetimeScope();
            builder.RegisterType<OperationRepository>().As<IOperationRepository>().InstancePerLifetimeScope();
            builder.RegisterType<OtherRepository>().As<IOtherRepository>().InstancePerLifetimeScope();
            builder.RegisterType<PersonalRepository>().As<IPersonalRepository>().InstancePerLifetimeScope();
            builder.RegisterType<PracticeRepository>().As<IPracticeRepository>().InstancePerLifetimeScope();
            builder.RegisterType<ProjectRepository>().As<IProjectRepository>().InstancePerLifetimeScope();
            builder.RegisterType<ResumeRepository>().As<IResumeRepository>().InstancePerLifetimeScope();
            builder.RegisterType<ResumeAreaRepository>().As<IResumeAreaRepository>().InstancePerLifetimeScope();
            builder.RegisterType<ResumeIndustryRepository>().As<IResumeIndustryRepository>().InstancePerLifetimeScope();
            builder.RegisterType<ResumeJobCategoryRepository>().As<IResumeJobCategoryRepository>().InstancePerLifetimeScope();
            builder.RegisterType<RoleRepository>().As<IRoleRepository>().InstancePerLifetimeScope();
            builder.RegisterType<RoleNavRepository>().As<IRoleNavRepository>().InstancePerLifetimeScope();
            builder.RegisterType<SkillRepository>().As<ISkillRepository>().InstancePerLifetimeScope();
            builder.RegisterType<SkillCategoryRepository>().As<ISkillCategoryRepository>().InstancePerLifetimeScope();
            builder.RegisterType<WorkRepository>().As<IWorkRepository>().InstancePerLifetimeScope();
            //新闻
            builder.RegisterType<CategoryRepository>().As<ICategoryRepository>().InstancePerLifetimeScope();
            builder.RegisterType<LinksRepository>().As<ILinksRepository>().InstancePerLifetimeScope();
            builder.RegisterType<NewsArticlesRepository>().As<INewsArticlesRepository>().InstancePerLifetimeScope();
            builder.RegisterType<NewsArticleTextRepository>().As<INewsArticleTextRepository>().InstancePerLifetimeScope();

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
