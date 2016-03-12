using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Lucky.Hr.Entity.Mapping
{
    public class ResumeJobCategoryMap : EntityTypeConfiguration<ResumeJobCategory>
    {
        public ResumeJobCategoryMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.CategoryId)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("ResumeJobCategory");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.ResumeId).HasColumnName("ResumeId");
            this.Property(t => t.CategoryId).HasColumnName("CategoryId");

            // Relationships
            this.HasRequired(t => t.JobCategory)
                .WithMany(t => t.ResumeJobCategories)
                .HasForeignKey(d => d.CategoryId);
            this.HasRequired(t => t.Resume)
                .WithMany(t => t.ResumeJobCategories)
                .HasForeignKey(d => d.ResumeId);

        }
    }
}
