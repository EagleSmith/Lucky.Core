using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Lucky.Entity.Mapping
{
    public class OtherMap : EntityTypeConfiguration<Other>
    {
        public OtherMap()
        {
            // Primary Key
            this.HasKey(t => t.OtherId);

            // Properties
            this.Property(t => t.OtherId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Intro)
                .IsRequired()
                .HasMaxLength(2000);

            // Table & Column Mappings
            this.ToTable("Other");
            this.Property(t => t.OtherId).HasColumnName("OtherId");
            this.Property(t => t.ResumeId).HasColumnName("ResumeId");
            this.Property(t => t.Title).HasColumnName("Title");
            this.Property(t => t.Intro).HasColumnName("Intro");
            this.Property(t => t.AddDate).HasColumnName("AddDate");

            // Relationships
            this.HasRequired(t => t.Resume)
                .WithMany(t => t.Other)
                .HasForeignKey(d => d.ResumeId);

        }
    }
}
