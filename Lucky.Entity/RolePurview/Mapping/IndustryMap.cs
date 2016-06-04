using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Lucky.Entity.Mapping
{
    public class IndustryMap : EntityTypeConfiguration<Industry>
    {
        public IndustryMap()
        {
            // Primary Key
            this.HasKey(t => t.IndustryId);

            // Properties
            this.Property(t => t.IndustryId)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.ParentId)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.IndustryName)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Industry");
            this.Property(t => t.IndustryId).HasColumnName("IndustryId");
            this.Property(t => t.ParentId).HasColumnName("ParentId");
            this.Property(t => t.IndustryName).HasColumnName("IndustryName");
            this.Property(t => t.Sort).HasColumnName("Sort");
            this.Property(t => t.Layer).HasColumnName("Layer");
        }
    }
}
