using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Lucky.Entity.Mapping
{
    public class JobCategoryMap : EntityTypeConfiguration<JobCategory>
    {
        public JobCategoryMap()
        {
            // Primary Key
            this.HasKey(t => t.CategoryId);

            // Properties
            this.Property(t => t.CategoryId)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.ParentId)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.CategoryName)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("JobCategory");
            this.Property(t => t.CategoryId).HasColumnName("CategoryId");
            this.Property(t => t.ParentId).HasColumnName("ParentId");
            this.Property(t => t.CategoryName).HasColumnName("CategoryName");
            this.Property(t => t.Layer).HasColumnName("Layer");
            this.Property(t => t.Sort).HasColumnName("Sort");
        }
    }
}
