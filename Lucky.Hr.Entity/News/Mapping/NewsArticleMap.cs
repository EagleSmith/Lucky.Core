using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Lucky.Entity.Mapping
{
    public class NewsArticleMap : EntityTypeConfiguration<NewsArticle>
    {
        public NewsArticleMap()
        {
            // Primary Key
            this.HasKey(t => t.ArticleID);

            // Properties
            this.Property(t => t.CategoryID)
                .IsRequired()
                .HasMaxLength(30);

            this.Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(250);

            this.Property(t => t.Summarize)
                .HasMaxLength(500);

            this.Property(t => t.Source)
                .HasMaxLength(150);

            this.Property(t => t.Author)
                .HasMaxLength(50);

            this.Property(t => t.Editor)
                .HasMaxLength(50);

            this.Property(t => t.KeyWord)
                .HasMaxLength(150);

            this.Property(t => t.ImgUrl)
                .HasMaxLength(150);

            this.Property(t => t.UserID)
                .HasMaxLength(128);

            // Table & Column Mappings
            this.ToTable("NewsArticles");
            this.Property(t => t.ArticleID).HasColumnName("ArticleID");
            this.Property(t => t.CategoryID).HasColumnName("CategoryID");
            this.Property(t => t.Title).HasColumnName("Title");
            this.Property(t => t.Summarize).HasColumnName("Summarize");
            this.Property(t => t.Source).HasColumnName("Source");
            this.Property(t => t.Author).HasColumnName("Author");
            this.Property(t => t.Editor).HasColumnName("Editor");
            this.Property(t => t.KeyWord).HasColumnName("KeyWord");
            this.Property(t => t.IsTop).HasColumnName("IsTop");
            this.Property(t => t.IsHot).HasColumnName("IsHot");
            this.Property(t => t.IsComment).HasColumnName("IsComment");
            this.Property(t => t.IsLock).HasColumnName("IsLock");
            this.Property(t => t.IsCommend).HasColumnName("IsCommend");
            this.Property(t => t.IsSlide).HasColumnName("IsSlide");
            this.Property(t => t.ImgUrl).HasColumnName("ImgUrl");
            this.Property(t => t.ClickNum).HasColumnName("ClickNum");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");
            this.Property(t => t.OrgID).HasColumnName("OrgID");
            this.Property(t => t.UserID).HasColumnName("UserID");

            // Relationships
            this.HasRequired(t => t.Category)
                .WithMany(t => t.NewsArticles)
                .HasForeignKey(d => d.CategoryID);


        }
    }
}
