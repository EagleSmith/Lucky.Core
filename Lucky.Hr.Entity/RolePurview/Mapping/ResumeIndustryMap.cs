using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Lucky.Hr.Entity.Mapping
{
    public class ResumeIndustryMap : EntityTypeConfiguration<ResumeIndustry>
    {
        public ResumeIndustryMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.IndustryId)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("ResumeIndustry");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.ResumeId).HasColumnName("ResumeId");
            this.Property(t => t.IndustryId).HasColumnName("IndustryId");

            // Relationships
            this.HasRequired(t => t.Industry)
                .WithMany(t => t.ResumeIndustries)
                .HasForeignKey(d => d.IndustryId);
            this.HasRequired(t => t.Resume)
                .WithMany(t => t.ResumeIndustries)
                .HasForeignKey(d => d.ResumeId);

        }
    }
}
