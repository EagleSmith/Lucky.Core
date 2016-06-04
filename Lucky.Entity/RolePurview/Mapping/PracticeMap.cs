using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Lucky.Entity.Mapping
{
    public class PracticeMap : EntityTypeConfiguration<Practice>
    {
        public PracticeMap()
        {
            // Primary Key
            this.HasKey(t => t.PracticeId);

            // Properties
            this.Property(t => t.PracticeName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Intro)
                .IsRequired()
                .HasMaxLength(2000);

            // Table & Column Mappings
            this.ToTable("Practice");
            this.Property(t => t.PracticeId).HasColumnName("PracticeId");
            this.Property(t => t.ResumeId).HasColumnName("ResumeId");
            this.Property(t => t.PracticeName).HasColumnName("PracticeName");
            this.Property(t => t.StartDate).HasColumnName("StartDate");
            this.Property(t => t.EndDate).HasColumnName("EndDate");
            this.Property(t => t.PracticeNow).HasColumnName("PracticeNow");
            this.Property(t => t.Intro).HasColumnName("Intro");
            this.Property(t => t.AddDate).HasColumnName("AddDate");
        }
    }
}
