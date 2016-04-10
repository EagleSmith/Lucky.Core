using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Lucky.Entity.Mapping
{
    public class LanguageMap : EntityTypeConfiguration<Language>
    {
        public LanguageMap()
        {
            // Primary Key
            this.HasKey(t => t.LanguageId);

            // Properties
            this.Property(t => t.LanguageType)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.SkillLevel)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Cert)
                .IsRequired()
                .HasMaxLength(500);

            // Table & Column Mappings
            this.ToTable("Language");
            this.Property(t => t.LanguageId).HasColumnName("LanguageId");
            this.Property(t => t.ResumeId).HasColumnName("ResumeId");
            this.Property(t => t.LanguageType).HasColumnName("LanguageType");
            this.Property(t => t.SkillLevel).HasColumnName("SkillLevel");
            this.Property(t => t.Cert).HasColumnName("Cert");
            this.Property(t => t.AddDate).HasColumnName("AddDate");

            // Relationships
            this.HasRequired(t => t.Resume)
                .WithMany(t => t.Languages)
                .HasForeignKey(d => d.ResumeId);

        }
    }
}
