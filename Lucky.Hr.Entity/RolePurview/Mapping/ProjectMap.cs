using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Lucky.Hr.Entity.Mapping
{
    public class ProjectMap : EntityTypeConfiguration<Project>
    {
        public ProjectMap()
        {
            // Primary Key
            this.HasKey(t => t.ProjectId);

            // Properties
            this.Property(t => t.ProjectName)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.Duty)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.ProjectIntro)
                .IsRequired()
                .HasMaxLength(500);

            this.Property(t => t.ProjectExperience)
                .IsRequired()
                .HasMaxLength(1000);

            // Table & Column Mappings
            this.ToTable("Project");
            this.Property(t => t.ProjectId).HasColumnName("ProjectId");
            this.Property(t => t.ResumeId).HasColumnName("ResumeId");
            this.Property(t => t.ProjectName).HasColumnName("ProjectName");
            this.Property(t => t.StartDate).HasColumnName("StartDate");
            this.Property(t => t.EndDate).HasColumnName("EndDate");
            this.Property(t => t.ProjectNow).HasColumnName("ProjectNow");
            this.Property(t => t.Duty).HasColumnName("Duty");
            this.Property(t => t.ProjectIntro).HasColumnName("ProjectIntro");
            this.Property(t => t.ProjectExperience).HasColumnName("ProjectExperience");
            this.Property(t => t.AddDate).HasColumnName("AddDate");

            // Relationships
            this.HasRequired(t => t.Resume)
                .WithMany(t => t.Projects)
                .HasForeignKey(d => d.ResumeId);

        }
    }
}
