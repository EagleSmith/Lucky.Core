using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Lucky.Entity.Mapping
{
    public class NewsTypeMap : EntityTypeConfiguration<NewsType>
    {
        public NewsTypeMap()
        {
            // Primary Key
            this.HasKey(t => t.NewsTypeId);

            // Properties
            this.Property(t => t.TypeName)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("NewsType");
            this.Property(t => t.NewsTypeId).HasColumnName("NewsTypeId");
            this.Property(t => t.DistributorId).HasColumnName("DistributorId");
            this.Property(t => t.TypeName).HasColumnName("TypeName");
            this.Property(t => t.Sort).HasColumnName("Sort");

            // Relationships
            this.HasRequired(t => t.Distributor)
                .WithMany(t => t.NewsTypes)
                .HasForeignKey(d => d.DistributorId);

        }
    }
}
