using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Lucky.Hr.Entity.Mapping
{
    public class ManagerMap : EntityTypeConfiguration<Manager>
    {
        public ManagerMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.DepartmentId)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.UserName)
                .IsRequired()
                .HasMaxLength(50);


            this.Property(t => t.FullName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.LastLoginIp)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.Token)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.AddFullName)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("AspNetUsers");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.DistributorId).HasColumnName("DistributorId");
            this.Property(t => t.DepartmentId).HasColumnName("DepartmentId");
            this.Property(t => t.UserName).HasColumnName("UserName");
            this.Property(t => t.FullName).HasColumnName("FullName");
            this.Property(t => t.IsSuper).HasColumnName("IsSuper");
            this.Property(t => t.IsLock).HasColumnName("IsLock");
            this.Property(t => t.LoginCount).HasColumnName("LoginCount");
            this.Property(t => t.LastLoginDate).HasColumnName("LastLoginDate");
            this.Property(t => t.LastLoginIp).HasColumnName("LastLoginIp");
            this.Property(t => t.LastModify).HasColumnName("LastModify");
            this.Property(t => t.Token).HasColumnName("Token");
            this.Property(t => t.State).HasColumnName("State");
            this.Property(t => t.BehaviorRemind).HasColumnName("BehaviorRemind");
            this.Property(t => t.AddManagerId).HasColumnName("AddManagerId");
            this.Property(t => t.AddFullName).HasColumnName("AddFullName");
            this.Property(t => t.AddDate).HasColumnName("AddDate");
            // Relationships
            this.HasRequired(t => t.Distributor)
                .WithMany(t => t.Managers)
                .HasForeignKey(d => d.DistributorId);

        }
    }
}
