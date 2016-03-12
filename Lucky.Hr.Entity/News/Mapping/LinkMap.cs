using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Lucky.Hr.Entity.Mapping
{
    public class LinkMap : EntityTypeConfiguration<Link>
    {
        public LinkMap()
        {
            // Primary Key
            this.HasKey(t => t.LinkID);

            // Properties
            this.Property(t => t.Title)
                .HasMaxLength(150);

            this.Property(t => t.UserName)
                .HasMaxLength(50);

            this.Property(t => t.UserTel)
                .HasMaxLength(50);

            this.Property(t => t.UserEmail)
                .HasMaxLength(150);

            this.Property(t => t.WebUrl)
                .IsRequired()
                .HasMaxLength(150);

            this.Property(t => t.ImageUrl)
                .HasMaxLength(150);

            // Table & Column Mappings
            this.ToTable("Links");
            this.Property(t => t.LinkID).HasColumnName("LinkID");
            this.Property(t => t.Title).HasColumnName("Title");
            this.Property(t => t.UserName).HasColumnName("UserName");
            this.Property(t => t.UserTel).HasColumnName("UserTel");
            this.Property(t => t.UserEmail).HasColumnName("UserEmail");
            this.Property(t => t.IsImage).HasColumnName("IsImage");
            this.Property(t => t.DisplayOrder).HasColumnName("DisplayOrder");
            this.Property(t => t.WebUrl).HasColumnName("WebUrl");
            this.Property(t => t.ImageUrl).HasColumnName("ImageUrl");
            this.Property(t => t.IsLock).HasColumnName("IsLock");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
        }
    }
}
