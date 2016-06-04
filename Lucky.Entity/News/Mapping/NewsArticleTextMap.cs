using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Lucky.Entity.Mapping
{
    public class NewsArticleTextMap : EntityTypeConfiguration<NewsArticleText>
    {
        public NewsArticleTextMap()
        {
            // Primary Key
            this.HasKey(t => t.ArticleTextID);

            // Properties
            // Table & Column Mappings
            this.ToTable("NewsArticleText");
            this.Property(t => t.ArticleTextID).HasColumnName("ArticleTextID");
            this.Property(t => t.ArticleID).HasColumnName("ArticleID");
            this.Property(t => t.ArticleText).HasColumnName("ArticleText");
            this.Property(t => t.NoHtml).HasColumnName("NoHtml");

            // Relationships
            this.HasRequired(t => t.NewsArticle)
                .WithMany(t => t.NewsArticleTexts)
                .HasForeignKey(d => d.ArticleID);

        }
    }
}
