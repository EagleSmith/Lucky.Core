using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Lucky.Entity.Mapping
{
    public class ManagerLogMap : EntityTypeConfiguration<ManagerLog>
    {
        public ManagerLogMap()
        {
            // Primary Key
            this.HasKey(t => t.ManagerLogId);

            // Properties
            this.Property(t => t.Remark)
                .IsRequired()
                .HasMaxLength(500);

            this.Property(t => t.FullName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.IpAddress)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("ManagerLog");
            this.Property(t => t.ManagerLogId).HasColumnName("ManagerLogId");
            this.Property(t => t.DistributorId).HasColumnName("DistributorId");
            this.Property(t => t.NavId).HasColumnName("NavId");
            this.Property(t => t.Operation).HasColumnName("Operation");
            this.Property(t => t.Remark).HasColumnName("Remark");
            this.Property(t => t.ManagerId).HasColumnName("ManagerId");
            this.Property(t => t.FullName).HasColumnName("FullName");
            this.Property(t => t.AddDate).HasColumnName("AddDate");
            this.Property(t => t.IpAddress).HasColumnName("IpAddress");

            // Relationships
            this.HasRequired(t => t.Manager)
                .WithMany(t => t.ManagerLogs)
                .HasForeignKey(d => d.ManagerId);

        }
    }
}
