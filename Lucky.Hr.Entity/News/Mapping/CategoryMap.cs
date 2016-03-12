using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Lucky.Hr.Entity.Mapping
{
    public class CategoryMap : EntityTypeConfiguration<Category>
    {
        public CategoryMap()
        {
            // Primary Key
            this.HasKey(t => t.CategoryID);

            // Properties
            this.Property(t => t.CategoryID)
                .IsRequired()
                .HasMaxLength(30);

            this.Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(150);

            this.Property(t => t.Description)
                .HasMaxLength(50);

            this.Property(t => t.HyperLink)
                .HasMaxLength(250);

            this.Property(t => t.ParentID)
                .IsRequired()
                .HasMaxLength(30);

            this.Property(t => t.SortCode)
                .HasMaxLength(10);

            this.Property(t => t.CategoryType)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Category");
            this.Property(t => t.CategoryID).HasColumnName("CategoryID");
            this.Property(t => t.Title).HasColumnName("Title");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.HyperLink).HasColumnName("HyperLink");
            this.Property(t => t.ParentID).HasColumnName("ParentID");
            this.Property(t => t.DisplayOrder).HasColumnName("DisplayOrder");
            this.Property(t => t.SortCode).HasColumnName("SortCode");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.CategoryType).HasColumnName("CategoryType");
        }
    }
}
