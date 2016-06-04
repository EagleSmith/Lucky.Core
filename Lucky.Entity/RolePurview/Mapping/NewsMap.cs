using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Lucky.Entity.Mapping
{
    public class NewsMap : EntityTypeConfiguration<News>
    {
        public NewsMap()
        {
            // Primary Key
            this.HasKey(t => t.NewsId);

            // Properties
            this.Property(t => t.NewsTitle)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.Image)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.NewsContent)
                .IsRequired();

            this.Property(t => t.AddFullName)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("News");
            this.Property(t => t.NewsId).HasColumnName("NewsId");
            this.Property(t => t.NewsTypeId).HasColumnName("NewsTypeId");
            this.Property(t => t.DistributorId).HasColumnName("DistributorId");
            this.Property(t => t.NewsTitle).HasColumnName("NewsTitle");
            this.Property(t => t.Image).HasColumnName("Image");
            this.Property(t => t.NewsContent).HasColumnName("NewsContent");
            this.Property(t => t.Praise).HasColumnName("Praise");
            this.Property(t => t.Reply).HasColumnName("Reply");
            this.Property(t => t.AddManagerId).HasColumnName("AddManagerId");
            this.Property(t => t.AddFullName).HasColumnName("AddFullName");
            this.Property(t => t.AddDate).HasColumnName("AddDate");

            // Relationships
            this.HasRequired(t => t.NewsType)
                .WithMany(t => t.News)
                .HasForeignKey(d => d.NewsTypeId);

        }
    }
}
