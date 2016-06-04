using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Lucky.Entity.Mapping
{
    public class EducationMap : EntityTypeConfiguration<Education>
    {
        public EducationMap()
        {
            // Primary Key
            this.HasKey(t => t.EducationId);

            // Properties
            this.Property(t => t.SchoolName)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.Major)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Course)
                .IsRequired()
                .HasMaxLength(500);

            this.Property(t => t.Duty)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Cert)
                .IsRequired()
                .HasMaxLength(500);

            // Table & Column Mappings
            this.ToTable("Education");
            this.Property(t => t.EducationId).HasColumnName("EducationId");
            this.Property(t => t.ResumeId).HasColumnName("ResumeId");
            this.Property(t => t.EducationType).HasColumnName("EducationType");
            this.Property(t => t.SchoolName).HasColumnName("SchoolName");
            this.Property(t => t.StartDate).HasColumnName("StartDate");
            this.Property(t => t.EndDate).HasColumnName("EndDate");
            this.Property(t => t.InSchool).HasColumnName("InSchool");
            this.Property(t => t.Degree).HasColumnName("Degree");
            this.Property(t => t.Major).HasColumnName("Major");
            this.Property(t => t.Course).HasColumnName("Course");
            this.Property(t => t.Duty).HasColumnName("Duty");
            this.Property(t => t.Cert).HasColumnName("Cert");
            this.Property(t => t.AddDate).HasColumnName("AddDate");

            // Relationships
            this.HasRequired(t => t.Resume)
                .WithMany(t => t.Educations)
                .HasForeignKey(d => d.ResumeId);

        }
    }
}
