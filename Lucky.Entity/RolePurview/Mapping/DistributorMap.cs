using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Lucky.Entity.Mapping
{
    public class DistributorMap : EntityTypeConfiguration<Distributor>
    {
        public DistributorMap()
        {
            // Primary Key
            this.HasKey(t => t.DistributorId);

            // Properties
            this.Property(t => t.Path)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.DistributionName)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.AreaId)
                .IsRequired()
                .HasMaxLength(10);

            this.Property(t => t.Street)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.Phone)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.Fax)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.Email)
                .IsRequired()
                .HasMaxLength(500);

            this.Property(t => t.HomePage)
                .IsRequired()
                .HasMaxLength(200);

            this.Property(t => t.WeiXin)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.BankAccount)
                .IsRequired()
                .HasMaxLength(1000);

            this.Property(t => t.Remark)
                .IsRequired()
                .HasMaxLength(2000);

            // Table & Column Mappings
            this.ToTable("Distributor");
            this.Property(t => t.DistributorId).HasColumnName("DistributorId");
            this.Property(t => t.ParentId).HasColumnName("ParentId");
            this.Property(t => t.Path).HasColumnName("Path");
            this.Property(t => t.DistributionName).HasColumnName("DistributionName");
            this.Property(t => t.AreaId).HasColumnName("AreaId");
            this.Property(t => t.Street).HasColumnName("Street");
            this.Property(t => t.Lng).HasColumnName("Lng");
            this.Property(t => t.Lat).HasColumnName("Lat");
            this.Property(t => t.Phone).HasColumnName("Phone");
            this.Property(t => t.Fax).HasColumnName("Fax");
            this.Property(t => t.Email).HasColumnName("Email");
            this.Property(t => t.HomePage).HasColumnName("HomePage");
            this.Property(t => t.WeiXin).HasColumnName("WeiXin");
            this.Property(t => t.BankAccount).HasColumnName("BankAccount");
            this.Property(t => t.Remark).HasColumnName("Remark");
            this.Property(t => t.IsLock).HasColumnName("IsLock");
            this.Property(t => t.State).HasColumnName("State");
        }
    }
}
