using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lucky.Hr.Core.Configuration;

namespace Lucky.Hr.Core.Infrastructure.DependencyManagement
{
    /// <summary>
    /// 配置依赖反转容器
    /// </summary>
    public class ContainerConfigurer
    {
        public virtual void Configure(IEngine engine, ContainerManager containerManager, HrConfig configuration)
        {
            //其他依赖
            containerManager.AddComponentInstance<HrConfig>(configuration, "Hr.configuration");
            containerManager.AddComponentInstance<IEngine>(engine, "Hr.engine");
            containerManager.AddComponentInstance<ContainerConfigurer>(this, "Hr.containerConfigurer");

            //类型查找
            containerManager.AddComponent<ITypeFinder, WebAppTypeFinder>("Hr.typeFinder");

            //注册其他组件的依赖关系
            var typeFinder = containerManager.Resolve<ITypeFinder>();
            containerManager.UpdateContainer(x =>
            {
                var drTypes = typeFinder.FindClassesOfType<IDependencyRegistrar>();
                var drInstances = drTypes.Select(drType => (IDependencyRegistrar)Activator.CreateInstance(drType)).ToList();
                //排序
                drInstances = drInstances.AsQueryable().OrderBy(t => t.Order).ToList();
                foreach (var dependencyRegistrar in drInstances)
                    dependencyRegistrar.Register(x, typeFinder);
            });
        }
    }
}
