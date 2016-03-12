using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Lucky.Hr.Entity.Mapping
{
    public class WorkMap : EntityTypeConfiguration<Work>
    {
        public WorkMap()
        {
            // Primary Key
            this.HasKey(t => t.WorkId);

            // Properties
            this.Property(t => t.CompanyName)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.IndustryId)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.JobName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.CategoryId)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.ManageDempartment)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.ReportMan)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.WorkContent)
                .IsRequired()
                .HasMaxLength(1000);

            this.Property(t => t.LeaveReason)
                .IsRequired()
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("Work");
            this.Property(t => t.WorkId).HasColumnName("WorkId");
            this.Property(t => t.ResumeId).HasColumnName("ResumeId");
            this.Property(t => t.CompanyName).HasColumnName("CompanyName");
            this.Property(t => t.CompanyProperty).HasColumnName("CompanyProperty");
            this.Property(t => t.CompanySize).HasColumnName("CompanySize");
            this.Property(t => t.IndustryId).HasColumnName("IndustryId");
            this.Property(t => t.StartDate).HasColumnName("StartDate");
            this.Property(t => t.EndDate).HasColumnName("EndDate");
            this.Property(t => t.WorkNow).HasColumnName("WorkNow");
            this.Property(t => t.JobName).HasColumnName("JobName");
            this.Property(t => t.JobType).HasColumnName("JobType");
            this.Property(t => t.CategoryId).HasColumnName("CategoryId");
            this.Property(t => t.Joblevel).HasColumnName("Joblevel");
            this.Property(t => t.ManageDempartment).HasColumnName("ManageDempartment");
            this.Property(t => t.SubordinateSize).HasColumnName("SubordinateSize");
            this.Property(t => t.ReportMan).HasColumnName("ReportMan");
            this.Property(t => t.Salary).HasColumnName("Salary");
            this.Property(t => t.SalarySecrecy).HasColumnName("SalarySecrecy");
            this.Property(t => t.WorkContent).HasColumnName("WorkContent");
            this.Property(t => t.LeaveReason).HasColumnName("LeaveReason");
            this.Property(t => t.AddDate).HasColumnName("AddDate");

            // Relationships
            this.HasRequired(t => t.Resume)
                .WithMany(t => t.Works)
                .HasForeignKey(d => d.ResumeId);

        }
    }
}
