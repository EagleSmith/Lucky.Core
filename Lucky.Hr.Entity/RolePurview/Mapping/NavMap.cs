using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Lucky.Hr.Entity.Mapping
{
    public class NavMap : EntityTypeConfiguration<Nav>
    {
        public NavMap()
        {
            // Primary Key
            this.HasKey(t => t.NavId);

            // Properties
            this.Property(t => t.NavId)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.ParentId)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.NavName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.SystemName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Logo)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.Url)
                .IsRequired()
                .HasMaxLength(200);

            this.Property(t => t.ControllerName)
                .IsRequired()
                .HasMaxLength(200);

            this.Property(t => t.ActionName)
                .IsRequired()
                .HasMaxLength(200);

            // Table & Column Mappings
            this.ToTable("Nav");
            this.Property(t => t.NavId).HasColumnName("NavId");
            this.Property(t => t.ParentId).HasColumnName("ParentId");
            this.Property(t => t.NavType).HasColumnName("NavType");
            this.Property(t => t.NavName).HasColumnName("NavName");
            this.Property(t => t.SystemName).HasColumnName("SystemName");
            this.Property(t => t.Logo).HasColumnName("Logo");
            this.Property(t => t.Url).HasColumnName("Url");
            this.Property(t => t.ControllerName).HasColumnName("ControllerName");
            this.Property(t => t.ActionName).HasColumnName("ActionName");
            this.Property(t => t.IsExpend).HasColumnName("IsExpend");
            this.Property(t => t.State).HasColumnName("State");
            this.Property(t => t.Sort).HasColumnName("Sort");
        }
    }
}
