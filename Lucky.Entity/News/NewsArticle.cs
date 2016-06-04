using System;
using System.Collections.Generic;

namespace Lucky.Entity
{
    public partial class NewsArticle
    {
        public NewsArticle()
        {
            this.NewsArticleTexts = new List<NewsArticleText>();
        }

        public System.Guid ArticleID { get; set; }
        public string CategoryID { get; set; }
        public string Title { get; set; }
        public string Summarize { get; set; }
        public string Source { get; set; }
        public string Author { get; set; }
        public string Editor { get; set; }
        public string KeyWord { get; set; }
        public bool IsTop { get; set; }
        public bool IsHot { get; set; }
        public bool IsComment { get; set; }
        public bool IsLock { get; set; }
        public bool IsCommend { get; set; }
        public bool IsSlide { get; set; }
        public string ImgUrl { get; set; }
        public int ClickNum { get; set; }
        public System.DateTime CreateDate { get; set; }
        public System.DateTime UpdateDate { get; set; }
        public Nullable<int> OrgID { get; set; }
        public string UserID { get; set; }
        public virtual Category Category { get; set; }
        public virtual ICollection<NewsArticleText> NewsArticleTexts { get; set; }
    }
}
