using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Lucky.Entity.Mapping
{
    public class AreaMap : EntityTypeConfiguration<Area>
    {
        public AreaMap()
        {
            // Primary Key
            this.HasKey(t => t.AreaId);

            // Properties
            this.Property(t => t.AreaId)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.ParentId)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.AreaName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.FullName)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Area");
            this.Property(t => t.AreaId).HasColumnName("AreaId");
            this.Property(t => t.ParentId).HasColumnName("ParentId");
            this.Property(t => t.AreaName).HasColumnName("AreaName");
            this.Property(t => t.FullName).HasColumnName("FullName");
            this.Property(t => t.Layer).HasColumnName("Layer");
        }
    }
}
