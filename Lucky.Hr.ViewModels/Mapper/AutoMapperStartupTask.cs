using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Lucky.Core.Infrastructure;

using Lucky.Entity;
using Lucky.Hr.ViewModels.Mapper;
using Lucky.Hr.ViewModels.Models.News;
using Lucky.Hr.ViewModels.Models.SiteManager;

namespace Lucky.Hr.ViewModels
{
    public class AutoMapperStartupTask : IStartupTask
    {
        public void Execute()
        {
            AutoMapperConfiguration.Init();
        }

        public int Order
        {
            get { return 0; }
        }
    }
}
