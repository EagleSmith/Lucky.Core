using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lucky.Hr.Core.Configuration;
using Lucky.Hr.Core.Infrastructure.DependencyManagement;

namespace Lucky.Hr.Core.Infrastructure
{
    public interface IEngine
    {
        ContainerManager ContainerManager { get; }

        /// <summary>
        /// 初始化组件和插件的NOP环境
        /// </summary>
        /// <param name="config">Config</param>
        void Initialize(HrConfig config);

        T Resolve<T>() where T : class;

        object Resolve(Type type);

        T[] ResolveAll<T>();
    }
}
