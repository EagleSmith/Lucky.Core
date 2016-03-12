using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lucky.Hr.Core.Data.UnitOfWork;
using Lucky.Hr.Entity;

namespace Lucky.Hr.IService
{
    public interface INewsContext : IMainContext
    {
        DbSet<Category> Categories { get; set; }
        DbSet<Link> Links { get; set; }
        DbSet<NewsArticle> NewsArticles { get; set; }
        DbSet<NewsArticleText> NewsArticleTexts { get; set; }
        
    }
}
