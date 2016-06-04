using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Lucky.Entity.Mapping
{
    public class DepartmentRoleMap : EntityTypeConfiguration<DepartmentRole>
    {
        public DepartmentRoleMap()
        {
            // Primary Key
            this.HasKey(t => new { t.DepartmentId, t.RoleId });

            // Properties
            this.Property(t => t.ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.DepartmentId)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.RoleId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("DepartmentRole");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.DepartmentId).HasColumnName("DepartmentId");
            this.Property(t => t.RoleId).HasColumnName("RoleId");

            // Relationships
            this.HasRequired(t => t.Department)
                .WithMany(t => t.DepartmentRoles)
                .HasForeignKey(d => d.DepartmentId);
            this.HasRequired(t => t.Role)
                .WithMany(t => t.DepartmentRoles)
                .HasForeignKey(d => d.RoleId);

        }
    }
}
