using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;

namespace Lucky.Core.Logging
{
    public class LoggingModule:Module
    {
        public LoggingModule()
        {
            
        }

        protected override void Load(ContainerBuilder moduleBuilder)
        {
            moduleBuilder.RegisterType<HrLogger>().As<ILogger>().InstancePerLifetimeScope();
        }
    }
}
