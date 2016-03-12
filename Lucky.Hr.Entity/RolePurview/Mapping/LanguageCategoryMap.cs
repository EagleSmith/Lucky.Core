using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Lucky.Hr.Entity.Mapping
{
    public class LanguageCategoryMap : EntityTypeConfiguration<LanguageCategory>
    {
        public LanguageCategoryMap()
        {
            // Primary Key
            this.HasKey(t => new { t.CategoryId, t.ParentId, t.CategoryName, t.Layer, t.Sort });

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

            this.Property(t => t.Layer)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Sort)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("LanguageCategory");
            this.Property(t => t.CategoryId).HasColumnName("CategoryId");
            this.Property(t => t.ParentId).HasColumnName("ParentId");
            this.Property(t => t.CategoryName).HasColumnName("CategoryName");
            this.Property(t => t.Layer).HasColumnName("Layer");
            this.Property(t => t.Sort).HasColumnName("Sort");
        }
    }
}
