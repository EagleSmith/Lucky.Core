using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lucky.Entity;
using Lucky.Entity.Mapping;

using Lucky.IService;

namespace Lucky.Service
{
    public class NewsContext : MainContext, INewsContext
    {
        public NewsContext(): base("name=LuckyNewsContext")
        {
            
        }
        
        public DbSet<Category> Categories { get; set; }
        public DbSet<Link> Links { get; set; }
        public DbSet<NewsArticle> NewsArticles { get; set; }
        public DbSet<NewsArticleText> NewsArticleTexts { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new CategoryMap());
            modelBuilder.Configurations.Add(new LinkMap());
            modelBuilder.Configurations.Add(new NewsArticleMap());
            modelBuilder.Configurations.Add(new NewsArticleTextMap());
        }
    }
}
