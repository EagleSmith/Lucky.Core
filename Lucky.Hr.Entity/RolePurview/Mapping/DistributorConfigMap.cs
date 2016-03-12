using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Lucky.Hr.Entity.Mapping
{
    public class DistributorConfigMap : EntityTypeConfiguration<DistributorConfig>
    {
        public DistributorConfigMap()
        {
            // Primary Key
            this.HasKey(t => t.DistributorId);

            // Properties
            this.Property(t => t.DistributorId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Logo)
                .IsRequired()
                .HasMaxLength(200);

            // Table & Column Mappings
            this.ToTable("DistributorConfig");
            this.Property(t => t.DistributorId).HasColumnName("DistributorId");
            this.Property(t => t.PageSize).HasColumnName("PageSize");
            this.Property(t => t.Logo).HasColumnName("Logo");

            // Relationships
            this.HasRequired(t => t.Distributor)
                .WithOptional(t => t.DistributorConfig);

        }
    }
}
