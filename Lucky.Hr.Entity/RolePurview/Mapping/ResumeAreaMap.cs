using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Lucky.Hr.Entity.Mapping
{
    public class ResumeAreaMap : EntityTypeConfiguration<ResumeArea>
    {
        public ResumeAreaMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.AreaId)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("ResumeArea");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.ResumeId).HasColumnName("ResumeId");
            this.Property(t => t.AreaId).HasColumnName("AreaId");

            // Relationships
            this.HasRequired(t => t.Area)
                .WithMany(t => t.ResumeAreas)
                .HasForeignKey(d => d.AreaId);
            this.HasRequired(t => t.Resume)
                .WithMany(t => t.ResumeAreas)
                .HasForeignKey(d => d.ResumeId);

        }
    }
}
