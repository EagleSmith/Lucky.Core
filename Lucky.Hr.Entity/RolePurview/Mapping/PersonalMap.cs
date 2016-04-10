using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Lucky.Entity.Mapping
{
    public class PersonalMap : EntityTypeConfiguration<Personal>
    {
        public PersonalMap()
        {
            // Primary Key
            this.HasKey(t => t.PersonalId);

            // Properties
            this.Property(t => t.Mobile)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Email)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.QQ)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Password)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.prevLoginIp)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.LastLoginIp)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.Token)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Personal");
            this.Property(t => t.PersonalId).HasColumnName("PersonalId");
            this.Property(t => t.Mobile).HasColumnName("Mobile");
            this.Property(t => t.ValidMobile).HasColumnName("ValidMobile");
            this.Property(t => t.Email).HasColumnName("Email");
            this.Property(t => t.ValidEmail).HasColumnName("ValidEmail");
            this.Property(t => t.QQ).HasColumnName("QQ");
            this.Property(t => t.Password).HasColumnName("Password");
            this.Property(t => t.RegisterDate).HasColumnName("RegisterDate");
            this.Property(t => t.LoginCount).HasColumnName("LoginCount");
            this.Property(t => t.prevLoginIp).HasColumnName("prevLoginIp");
            this.Property(t => t.PrevLoginDate).HasColumnName("PrevLoginDate");
            this.Property(t => t.LastLoginIp).HasColumnName("LastLoginIp");
            this.Property(t => t.LastLoginDate).HasColumnName("LastLoginDate");
            this.Property(t => t.LastModifyDate).HasColumnName("LastModifyDate");
            this.Property(t => t.Token).HasColumnName("Token");
            this.Property(t => t.IsOpen).HasColumnName("IsOpen");
        }
    }
}
