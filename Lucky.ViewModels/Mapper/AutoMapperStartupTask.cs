using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Lucky.Core.Infrastructure;

using Lucky.Entity;
using Lucky.ViewModels.Mapper;
using Lucky.ViewModels.Models.News;
using Lucky.ViewModels.Models.SiteManager;

namespace Lucky.ViewModels
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
