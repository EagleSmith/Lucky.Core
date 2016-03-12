using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Lucky.Hr.Entity.Mapping
{
    public class CertMap : EntityTypeConfiguration<Cert>
    {
        public CertMap()
        {
            // Primary Key
            this.HasKey(t => t.CertId);

            // Properties
            this.Property(t => t.Cert1)
                .IsRequired()
                .HasMaxLength(500);

            // Table & Column Mappings
            this.ToTable("Cert");
            this.Property(t => t.CertId).HasColumnName("CertId");
            this.Property(t => t.ResumeId).HasColumnName("ResumeId");
            this.Property(t => t.Cert1).HasColumnName("Cert");
            this.Property(t => t.GetDate).HasColumnName("GetDate");
            this.Property(t => t.AddDate).HasColumnName("AddDate");

            // Relationships
            this.HasRequired(t => t.Resume)
                .WithMany(t => t.Certs)
                .HasForeignKey(d => d.ResumeId);

        }
    }
}
