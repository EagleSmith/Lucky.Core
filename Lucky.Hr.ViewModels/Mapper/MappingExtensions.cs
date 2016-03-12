using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Lucky.Hr.Entity;
using Lucky.Hr.ViewModels.Models.News;
using Lucky.Hr.ViewModels.Models.SiteManager;

namespace Lucky.Hr.ViewModels
{
    public static class MappingExtensions
    {
        #region Area
        public static Area ToEntity(this AreaViewModel model)
        {
            return Mapper.Map<AreaViewModel, Area>(model);
        }

        public static AreaViewModel ToModel(this Area entity)
        {
            return Mapper.Map<Area, AreaViewModel>(entity);
        }

        public static Area ToEntity(this AreaViewModel model, Area entity)
        {
            return Mapper.Map(model, entity);
        }

        #endregion

        #region Distributor

        public static Distributor ToEntity(this DistributorViewModel model)
        {
            return Mapper.Map<DistributorViewModel, Distributor>(model);
        }

        public static Distributor ToEntity(this DistributorViewModel model, Distributor entity)
        {
            return Mapper.Map(model, entity);
        }

        public static DistributorViewModel ToModel(this Distributor entity)
        {
            return Mapper.Map<Distributor, DistributorViewModel>(entity);
        }
        #endregion

        #region Department

        public static Department ToEntity(this DepartmentViewModel model)
        {
            return Mapper.Map<DepartmentViewModel, Department>(model);
        }

        public static Department ToEntity(this DepartmentViewModel model, Department entity)
        {
            return Mapper.Map(model, entity);
        }

        public static DepartmentViewModel ToModel(this Department entity)
        {
            return Mapper.Map<Department, DepartmentViewModel>(entity);
        }
        #endregion

        #region Nav
        public static Nav ToEntity(this NavViewModel model)
        {
            return Mapper.Map<NavViewModel, Nav>(model);
        }

        public static Nav ToEntity(this NavViewModel model, Nav entity)
        {
            return Mapper.Map(model, entity);
        }

        public static NavViewModel ToModel(this Nav entity)
        {
            return Mapper.Map<Nav, NavViewModel>(entity);
        }

        public static NavOperationViewModel ToNavOperationModel(this Nav entity)
        {
            return Mapper.Map<Nav, NavOperationViewModel>(entity);
        }
        #endregion

        #region AspNetUsers

        public static Manager ToEntity(this AspNetUsersViewModel model)
        {
            return Mapper.Map<AspNetUsersViewModel, Manager>(model);
        }

        public static Manager ToEntity(this AspNetUsersViewModel model, Manager entity)
        {
            return Mapper.Map(model, entity);
        }

        public static AspNetUsersViewModel ToModel(this Manager entity)
        {
            return Mapper.Map<Manager, AspNetUsersViewModel>(entity);
        }
        #endregion

        #region AspNetRoles

        public static Role ToEntity(this AspNetRolesViewModel model)
        {
            return Mapper.Map<AspNetRolesViewModel, Role>(model);
        }

        public static Role ToEntity(this AspNetRolesViewModel model, Role entity)
        {
            return Mapper.Map(model, entity);
        }

        public static AspNetRolesViewModel ToModel(this Role entity)
        {
            return Mapper.Map<Role, AspNetRolesViewModel>(entity);
        }
        #endregion

        #region Operation

        public static Operation ToEntity(this OperationViewModel model)
        {
            return Mapper.Map<OperationViewModel, Operation>(model);
        }

        public static Operation ToEntity(this OperationViewModel model, Operation entity)
        {
            return Mapper.Map(model, entity);
        }

        public static OperationViewModel ToModel(this Operation entity)
        {
            return Mapper.Map<Operation, OperationViewModel>(entity);
        }
        #endregion

        #region NewsArticles

        public static NewsArticle ToEntity(this NewsArticlesViewModel model)
        {
            return Mapper.Map<NewsArticlesViewModel, NewsArticle>(model);
        }

        public static NewsArticle ToEntity(this NewsArticlesViewModel model, NewsArticle entity)
        {
            return Mapper.Map(model, entity);
        }

        public static NewsArticlesViewModel ToModel(this NewsArticle entity)
        {
            return Mapper.Map<NewsArticle, NewsArticlesViewModel>(entity);
        }
        #endregion

        #region Category

        public static Category ToEntity(this CategoryViewModel model)
        {
            return Mapper.Map<CategoryViewModel, Category>(model);
        }

        public static Category ToEntity(this CategoryViewModel model, Category entity)
        {
            return Mapper.Map(model, entity);
        }

        public static CategoryViewModel ToModel(this Category entity)
        {
            return Mapper.Map<Category, CategoryViewModel>(entity);
        }
        #endregion

        #region Links

        public static Link ToEntity(this LinksViewModel model)
        {
            return Mapper.Map<LinksViewModel, Link>(model);
        }

        public static Link ToEntity(this LinksViewModel model, Link entity)
        {
            return Mapper.Map(model, entity);
        }

        public static LinksViewModel ToModel(this Link entity)
        {
            return Mapper.Map<Link, LinksViewModel>(entity);
        }
        #endregion
    }
}
