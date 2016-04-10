using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Lucky.Core.Infrastructure;

using Lucky.Entity;

using Lucky.Hr.ViewModels.Models.News;
using Lucky.Hr.ViewModels.Models.SiteManager;

namespace Lucky.Hr.ViewModels
{
    public class AutoMapperStartupTask : IStartupTask
    {
        public void Execute()
        {
            Mapper.CreateMap<Area, AreaViewModel>()
                .ForMember(vm => vm.AreaItems, entity => entity.Ignore());
            Mapper.CreateMap<AreaViewModel, Area>();
            Mapper.CreateMap<Distributor, DistributorViewModel>();
            Mapper.CreateMap<DistributorViewModel, Distributor>()
                .ForMember(entity => entity.Departments, vm => vm.Ignore());
            Mapper.CreateMap<Department, DepartmentViewModel>()
                .ForMember(vm => vm.DistributorName, entity => entity.MapFrom(a => a.Distributor.DistributionName));
                
            Mapper.CreateMap<DepartmentViewModel, Department>();

            Mapper.CreateMap<Nav, NavViewModel>();

            Mapper.CreateMap<NavViewModel, Nav>();

            Mapper.CreateMap<Entity.Manager, AspNetUsersViewModel>();

            Mapper.CreateMap<AspNetUsersViewModel, Manager>();

            Mapper.CreateMap<Role, AspNetRolesViewModel>();

            Mapper.CreateMap<AspNetRolesViewModel, Role>();

            Mapper.CreateMap<Operation, OperationViewModel>();

            Mapper.CreateMap<OperationViewModel, Operation>();

            Mapper.CreateMap<Nav, NavOperationViewModel>();

            #region News
            Mapper.CreateMap<NewsArticle, NewsArticlesViewModel>();

            Mapper.CreateMap<NewsArticlesViewModel, NewsArticle>();

            Mapper.CreateMap<Category, CategoryViewModel>();

            Mapper.CreateMap<CategoryViewModel, Category>();

            Mapper.CreateMap<Link, LinksViewModel>();

            Mapper.CreateMap<LinksViewModel, Link>();
            #endregion

        }

        public int Order
        {
            get { return 0; }
        }
    }
}
