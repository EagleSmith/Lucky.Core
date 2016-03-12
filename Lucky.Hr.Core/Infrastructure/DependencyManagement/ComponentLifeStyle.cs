using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lucky.Hr.Core.Infrastructure.DependencyManagement
{
    public enum ComponentLifeStyle
    {
        //单例
        Singleton = 0,
        //临时
        Transient = 1,
        //生命周期作用域
        LifetimeScope = 2
    }
}
