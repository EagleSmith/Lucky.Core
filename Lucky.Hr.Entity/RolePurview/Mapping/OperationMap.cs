using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Lucky.Hr.Entity.Mapping
{
    public class OperationMap : EntityTypeConfiguration<Operation>
    {
        public OperationMap()
        {
            // Primary Key
            this.HasKey(t => t.OperationId);

            // Properties
            this.Property(t => t.OperationName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.SystemName)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Operation");
            this.Property(t => t.OperationId).HasColumnName("OperationId");
            this.Property(t => t.OperationName).HasColumnName("OperationName");
            this.Property(t => t.SystemName).HasColumnName("SystemName");
            this.Property(t => t.Sort).HasColumnName("Sort");
            this.Property(t => t.OperationValue).HasColumnName("OperationValue");
        }
    }
}
