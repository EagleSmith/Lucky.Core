using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Lucky.Entity.Mapping
{
    public class NavOperationMap : EntityTypeConfiguration<NavOperation>
    {
        public NavOperationMap()
        {
            // Primary Key
            this.HasKey(t => new { t.NavId, t.OperationId });

            // Properties
            this.Property(t => t.ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.NavId)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.OperationId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("NavOperation");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.NavId).HasColumnName("NavId");
            this.Property(t => t.OperationId).HasColumnName("OperationId");

            // Relationships
            this.HasRequired(t => t.Nav)
                .WithMany(t => t.NavOperations)
                .HasForeignKey(d => d.NavId);
            this.HasRequired(t => t.Operation)
                .WithMany(t => t.NavOperations)
                .HasForeignKey(d => d.OperationId);

        }
    }
}
