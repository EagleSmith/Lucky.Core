using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Lucky.Hr.Entity.Mapping
{
    public class ManagerFileMap : EntityTypeConfiguration<ManagerFile>
    {
        public ManagerFileMap()
        {
            // Primary Key
            this.HasKey(t => t.ManagerFileId);

            // Properties
            this.Property(t => t.FileType)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.FileName)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.FullName)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("ManagerFile");
            this.Property(t => t.ManagerFileId).HasColumnName("ManagerFileId");
            this.Property(t => t.DistributorId).HasColumnName("DistributorId");
            this.Property(t => t.FileType).HasColumnName("FileType");
            this.Property(t => t.FileName).HasColumnName("FileName");
            this.Property(t => t.ManagerId).HasColumnName("ManagerId");
            this.Property(t => t.FullName).HasColumnName("FullName");
            this.Property(t => t.AddDate).HasColumnName("AddDate");

            // Relationships
            this.HasRequired(t => t.Manager)
                .WithMany(t => t.ManagerFiles)
                .HasForeignKey(d => d.ManagerId);

        }
    }
}
