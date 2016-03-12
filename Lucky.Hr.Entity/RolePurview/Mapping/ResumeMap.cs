using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Lucky.Hr.Entity.Mapping
{
    public class ResumeMap : EntityTypeConfiguration<Resume>
    {
        public ResumeMap()
        {
            // Primary Key
            this.HasKey(t => t.ResumeId);

            // Properties
            this.Property(t => t.Fullname)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.CurrentAreaId)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.NativeAreaId)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.JobIntention)
                .IsRequired()
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("Resume");
            this.Property(t => t.ResumeId).HasColumnName("ResumeId");
            this.Property(t => t.PersonalId).HasColumnName("PersonalId");
            this.Property(t => t.Fullname).HasColumnName("Fullname");
            this.Property(t => t.HidFullname).HasColumnName("HidFullname");
            this.Property(t => t.Sex).HasColumnName("Sex");
            this.Property(t => t.BirthDay).HasColumnName("BirthDay");
            this.Property(t => t.CurrentAreaId).HasColumnName("CurrentAreaId");
            this.Property(t => t.WorkState).HasColumnName("WorkState");
            this.Property(t => t.StartWork).HasColumnName("StartWork");
            this.Property(t => t.NativeAreaId).HasColumnName("NativeAreaId");
            this.Property(t => t.Marriage).HasColumnName("Marriage");
            this.Property(t => t.Political).HasColumnName("Political");
            this.Property(t => t.Height).HasColumnName("Height");
            this.Property(t => t.Weight).HasColumnName("Weight");
            this.Property(t => t.Jobtype).HasColumnName("Jobtype");
            this.Property(t => t.JobIntention).HasColumnName("JobIntention");
            this.Property(t => t.Parttime).HasColumnName("Parttime");
            this.Property(t => t.JobLevel).HasColumnName("JobLevel");
            this.Property(t => t.LowLevel).HasColumnName("LowLevel");
            this.Property(t => t.Salary).HasColumnName("Salary");
            this.Property(t => t.LowSalary).HasColumnName("LowSalary");
            this.Property(t => t.Negotiable).HasColumnName("Negotiable");

            // Relationships
            this.HasRequired(t => t.Personal)
                .WithMany(t => t.Resumes)
                .HasForeignKey(d => d.PersonalId);

        }
    }
}
