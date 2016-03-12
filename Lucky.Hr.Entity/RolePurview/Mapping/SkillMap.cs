using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Lucky.Hr.Entity.Mapping
{
    public class SkillMap : EntityTypeConfiguration<Skill>
    {
        public SkillMap()
        {
            // Primary Key
            this.HasKey(t => t.SkillId);

            // Properties
            this.Property(t => t.SkillName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.SkillLevel)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Skill");
            this.Property(t => t.SkillId).HasColumnName("SkillId");
            this.Property(t => t.ResumeId).HasColumnName("ResumeId");
            this.Property(t => t.SkillName).HasColumnName("SkillName");
            this.Property(t => t.SkillLevel).HasColumnName("SkillLevel");
            this.Property(t => t.AddDate).HasColumnName("AddDate");

            // Relationships
            this.HasRequired(t => t.Resume)
                .WithMany(t => t.Skills)
                .HasForeignKey(d => d.ResumeId);

        }
    }
}
