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
using Lucky.Hr.Core;

using Lucky.Hr.Core.Data;

using Lucky.Hr.Entity;

using Lucky.Hr.IService;

namespace Lucky.Hr.Service
{
    public  class NewsArticlesRepository  :EntityRepository<NewsArticle>,INewsArticlesRepository
    {
      public NewsArticlesRepository(INewsContext context):base(context)
        {
            
        }


      public void DeleteMore(string ids)
      {
          var temps = ids.Split(',');
          using (IDbContext context=IContext.UseTransaction(true))
          {
              foreach (var s in temps)
              {
                  context.Sql("Delete from NewsArticleText where [ArticleID]=@0").Parameters(s).Execute();
                  context.Sql("Delete from NewsArticles where [ArticleID]=@0").Parameters(s).Execute();
              }
              context.Commit();
          }
      }
    }
}