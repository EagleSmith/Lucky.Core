// =================================================================== 
// 项目说明
//====================================================================
// 幸运草工作室 @ CopyRight 2014-2020。
// 文件： NewsArticlesRespository.cs
// 项目名称： 
// 创建时间：2015/3/3
// 负责人：丁富升
// ===================================================================
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using Lucky.Hr.Caching;
using Lucky.Hr.Core;

using Lucky.Hr.Core.Data;
using Lucky.Hr.Core.Data.Dapper;
using Lucky.Hr.Entity;

using Lucky.Hr.IService;
using Lucky.Hr.ViewModels;
using Lucky.Hr.ViewModels.Models.News;

namespace Lucky.Hr.Service
{
    public  class NewsArticlesRepository  :EntityRepository<NewsArticle>,INewsArticlesRepository
    {
        private ICacheManager _cacheManager;
        private IDbContext _dbContext;
      public NewsArticlesRepository(INewsContext context, ICacheManager cacheManager, IDbContext dappercontext) :base(context)
      {
          _cacheManager = cacheManager;
          _dbContext = dappercontext;
      }

        public List<NewsArticlesViewModel> GetArticlesViewModels()
        {
            string key = "Article_List";
            //return _dbContext.Query<NewsArticlesViewModel>("ArticleListNatively", CommandType.StoredProcedure).ToList();
            return _cacheManager.Get(key, acx => GetList().Select(a => a.ToModel()).ToList());
            return GetList().Select(a => a.ToModel()).ToList();
        }
        public void DeleteMore(string ids)
      {
          //var temps = ids.Split(',');
          //using (IDbContext context=IContext.UseTransaction(true))
          //{
          //    foreach (var s in temps)
          //    {
          //        context.Sql("Delete from NewsArticleText where [ArticleID]=@0").Parameters(s).Execute();
          //        context.Sql("Delete from NewsArticles where [ArticleID]=@0").Parameters(s).Execute();
          //    }
          //    context.Commit();
          //}
      }
    }
}