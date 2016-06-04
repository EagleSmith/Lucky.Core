using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Lucky.Entity;
using Lucky.ViewModels.Models.News;
using Lucky.ViewModels.Models.SiteManager;

namespace Lucky.ViewModels
{
    public static class MappingExtensions
    {
        #region Area
        public static Area ToEntity(this AreaViewModel model)
        {
            return model.MapTo<AreaViewModel, Area>();
        }
        public static Area ToEntity(this AreaViewModel model, Area entity)
        {
            return model.MapTo(entity);
        }
        public static AreaViewModel ToModel(this Area entity)
        {
            return entity.MapTo<Area, AreaViewModel>();
        }
        #endregion

        #region Distributor

        public static Distributor ToEntity(this DistributorViewModel model)
        {
            return model.MapTo<DistributorViewModel, Distributor>();
        }

        public static Distributor ToEntity(this DistributorViewModel model, Distributor entity)
        {
            return model.MapTo(entity);
        }

        public static DistributorViewModel ToModel(this Distributor entity)
        {
            return entity.MapTo<Distributor, DistributorViewModel>();
        }
        #endregion

        #region Department

        public static Department ToEntity(this DepartmentViewModel model)
        {
            return model.MapTo<DepartmentViewModel, Department>();
        }

        public static Department ToEntity(this DepartmentViewModel model, Department entity)
        {
            return model.MapTo( entity);
        }

        public static DepartmentViewModel ToModel(this Department entity)
        {
            return entity.MapTo<Department, DepartmentViewModel>();
        }
        #endregion

        #region Nav
        public static Nav ToEntity(this NavViewModel model)
        {
            return model.MapTo<NavViewModel, Nav>();
        }

        public static Nav ToEntity(this NavViewModel model, Nav entity)
        {
            return model.MapTo( entity);
        }

        public static NavViewModel ToModel(this Nav entity)
        {
            return entity.MapTo<Nav, NavViewModel>();
        }

        public static NavOperationViewModel ToNavOperationModel(this Nav entity)
        {
            return entity.MapTo<Nav, NavOperationViewModel>();
        }
        #endregion

        #region AspNetUsers

        public static Manager ToEntity(this AspNetUsersViewModel model)
        {
            return model.MapTo<AspNetUsersViewModel, Manager>();
        }

        public static Manager ToEntity(this AspNetUsersViewModel model, Manager entity)
        {
            return model.MapTo(entity);
        }

        public static AspNetUsersViewModel ToModel(this Manager entity)
        {
            return entity.MapTo<Manager, AspNetUsersViewModel>();
        }
        #endregion

        #region AspNetRoles

        public static Role ToEntity(this AspNetRolesViewModel model)
        {
            return model.MapTo<AspNetRolesViewModel, Role>();
        }

        public static Role ToEntity(this AspNetRolesViewModel model, Role entity)
        {
            return model.MapTo(entity);
        }

        public static AspNetRolesViewModel ToModel(this Role entity)
        {
            return entity.MapTo<Role, AspNetRolesViewModel>();
        }
        #endregion

        #region Operation

        public static Operation ToEntity(this OperationViewModel model)
        {
            return model.MapTo<OperationViewModel, Operation>();
        }

        public static Operation ToEntity(this OperationViewModel model, Operation entity)
        {
            return model.MapTo(entity);
        }

        public static OperationViewModel ToModel(this Operation entity)
        {
            return entity.MapTo<Operation, OperationViewModel>();
        }
        #endregion

        #region NewsArticles

        public static NewsArticle ToEntity(this NewsArticlesViewModel model)
        {
            return model.MapTo<NewsArticlesViewModel, NewsArticle>();
        }

        public static NewsArticle ToEntity(this NewsArticlesViewModel model, NewsArticle entity)
        {
            return model.MapTo( entity);
        }

        public static NewsArticlesViewModel ToModel(this NewsArticle entity)
        {
            return entity.MapTo<NewsArticle, NewsArticlesViewModel>();
        }
        #endregion

        #region Category

        public static Category ToEntity(this CategoryViewModel model)
        {
            return model.MapTo<CategoryViewModel, Category>();
        }

        public static Category ToEntity(this CategoryViewModel model, Category entity)
        {
            return model.MapTo( entity);
        }

        public static CategoryViewModel ToModel(this Category entity)
        {
            return entity.MapTo<Category, CategoryViewModel>();
        }
        #endregion

        #region Links

        public static Link ToEntity(this LinksViewModel model)
        {
            return model.MapTo<LinksViewModel, Link>();
        }

        public static Link ToEntity(this LinksViewModel model, Link entity)
        {
            return model.MapTo(entity);
        }

        public static LinksViewModel ToModel(this Link entity)
        {
            return entity.MapTo<Link, LinksViewModel>();
        }
        #endregion
    }
}
