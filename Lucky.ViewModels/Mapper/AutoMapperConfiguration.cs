using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Lucky.Entity;
using Lucky.ViewModels.Models.News;
using Lucky.ViewModels.Models.SiteManager;

namespace Lucky.ViewModels.Mapper
{
    public class AutoMapperConfiguration
    {
        private static MapperConfiguration _mapperConfiguration;
        private static IMapper _mapper;

        public static void Init()
        {
            _mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Area, AreaViewModel>()
                .ForMember(vm => vm.AreaItems, entity => entity.Ignore());
                cfg.CreateMap<AreaViewModel, Area>();
                cfg.CreateMap<Distributor, DistributorViewModel>();
                cfg.CreateMap<DistributorViewModel, Distributor>()
                    .ForMember(entity => entity.Departments, vm => vm.Ignore());
                cfg.CreateMap<Department, DepartmentViewModel>()
                    .ForMember(vm => vm.DistributorName, entity => entity.MapFrom(a => a.Distributor.DistributionName));

                cfg.CreateMap<DepartmentViewModel, Department>();

                cfg.CreateMap<Nav, NavViewModel>();

                cfg.CreateMap<NavViewModel, Nav>();

                cfg.CreateMap<Entity.Manager, AspNetUsersViewModel>();

                cfg.CreateMap<AspNetUsersViewModel, Manager>();

                cfg.CreateMap<Role, AspNetRolesViewModel>();

                cfg.CreateMap<AspNetRolesViewModel, Role>();

                cfg.CreateMap<Operation, OperationViewModel>();

                cfg.CreateMap<OperationViewModel, Operation>();

                cfg.CreateMap<Nav, NavOperationViewModel>();

                #region News
                cfg.CreateMap<NewsArticle, NewsArticlesViewModel>();

                cfg.CreateMap<NewsArticlesViewModel, NewsArticle>();

                cfg.CreateMap<Category, CategoryViewModel>();

                cfg.CreateMap<CategoryViewModel, Category>();

                cfg.CreateMap<Link, LinksViewModel>();

                cfg.CreateMap<LinksViewModel, Link>();
                #endregion
            });
            _mapper = _mapperConfiguration.CreateMapper();
        }
        /// <summary>
        /// Mapper
        /// </summary>
        public static IMapper Mapper
        {
            get
            {
                return _mapper;
            }
        }
        /// <summary>
        /// Mapper configuration
        /// </summary>
        public static MapperConfiguration MapperConfiguration
        {
            get
            {
                return _mapperConfiguration;
            }
        }
    }
}
