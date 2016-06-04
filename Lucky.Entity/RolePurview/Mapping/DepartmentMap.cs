using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Lucky.Entity.Mapping
{
    public class DepartmentMap : EntityTypeConfiguration<Department>
    {
        public DepartmentMap()
        {
            // Primary Key
            this.HasKey(t => t.DepartmentId);

            // Properties
            this.Property(t => t.DepartmentId)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.ParentId)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.DepartmentName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Description)
                .IsRequired()
                .HasMaxLength(500);

            // Table & Column Mappings
            this.ToTable("Department");
            this.Property(t => t.DepartmentId).HasColumnName("DepartmentId");
            this.Property(t => t.DistributorId).HasColumnName("DistributorId");
            this.Property(t => t.ParentId).HasColumnName("ParentId");
            this.Property(t => t.DepartmentName).HasColumnName("DepartmentName");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.State).HasColumnName("State");
            this.Property(t => t.Sort).HasColumnName("Sort");

            // Relationships
            this.HasRequired(t => t.Distributor)
                .WithMany(t => t.Departments)
                .HasForeignKey(d => d.DistributorId);
            this.HasRequired(t => t.Parent)
                .WithMany(t => t.Departments)
                .HasForeignKey(d => d.ParentId);

        }
    }
}
