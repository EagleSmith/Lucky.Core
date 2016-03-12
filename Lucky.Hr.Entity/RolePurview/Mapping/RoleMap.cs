using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Lucky.Hr.Entity.Mapping
{
    public class RoleMap : EntityTypeConfiguration<Role>
    {
        public RoleMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.RoleName)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.DistributorId).HasColumnName("DistributorId");
            this.Property(t => t.RoleName).HasColumnName("RoleName");
            this.Property(t => t.IsSystem).HasColumnName("IsSystem");
            this.Property(t => t.Sort).HasColumnName("Sort");
        }
    }
}
