using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;

namespace Lucky.Hr.Core.Infrastructure.DependencyManagement
{
    public interface IDependencyRegistrar
    {
        /// <summary>
        /// 此方法在通过ContainerBuilder注册依赖关系
        /// </summary>
        /// <param name="builder">容器管理者类</param>
        /// <param name="typeFinder">类型查找器接口</param>
        void Register(ContainerBuilder builder, ITypeFinder typeFinder);

        int Order { get; }
    }
}
