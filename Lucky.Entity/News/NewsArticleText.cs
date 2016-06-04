using System;
using System.Collections.Generic;

namespace Lucky.Entity
{
    public partial class NewsArticleText
    {
        public long ArticleTextID { get; set; }
        public System.Guid ArticleID { get; set; }
        public string ArticleText { get; set; }
        public string NoHtml { get; set; }
        public virtual NewsArticle NewsArticle { get; set; }
    }
}
