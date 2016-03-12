using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Lucky.Hr.Entity.Mapping
{
    public class AchievementMap : EntityTypeConfiguration<Achievement>
    {
        public AchievementMap()
        {
            // Primary Key
            this.HasKey(t => t.AchievementId);

            // Properties
            this.Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Intro)
                .IsRequired()
                .HasMaxLength(2000);

            this.Property(t => t.Photo)
                .IsRequired()
                .HasMaxLength(1000);

            // Table & Column Mappings
            this.ToTable("Achievement");
            this.Property(t => t.AchievementId).HasColumnName("AchievementId");
            this.Property(t => t.ResumeId).HasColumnName("ResumeId");
            this.Property(t => t.Title).HasColumnName("Title");
            this.Property(t => t.Intro).HasColumnName("Intro");
            this.Property(t => t.Photo).HasColumnName("Photo");
            this.Property(t => t.AddDate).HasColumnName("AddDate");

            // Relationships
            this.HasRequired(t => t.Resume)
                .WithMany(t => t.Achievements)
                .HasForeignKey(d => d.ResumeId);

        }
    }
}
