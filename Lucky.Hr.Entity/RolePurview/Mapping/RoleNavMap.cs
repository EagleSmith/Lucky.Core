using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Lucky.Hr.Entity.Mapping
{
    public class RoleNavMap : EntityTypeConfiguration<RoleNav>
    {
        public RoleNavMap()
        {
            // Primary Key
            this.HasKey(t => new { t.RoleId, t.NavId, t.OperationId });

            // Properties
            this.Property(t => t.RoleId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.NavId)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.OperationId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("RoleNav");
            this.Property(t => t.RoleId).HasColumnName("RoleId");
            this.Property(t => t.NavId).HasColumnName("NavId");
            this.Property(t => t.OperationId).HasColumnName("OperationId");

            // Relationships
            this.HasRequired(t => t.Nav)
                .WithMany(t => t.RoleNavs)
                .HasForeignKey(d => d.NavId);
            this.HasRequired(t => t.Operation)
                .WithMany(t => t.RoleNavs)
                .HasForeignKey(d => d.OperationId);
            this.HasRequired(t => t.Role)
                .WithMany(t => t.RoleNavs)
                .HasForeignKey(d => d.RoleId);

        }
    }
}
