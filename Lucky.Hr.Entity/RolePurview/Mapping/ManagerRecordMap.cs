using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Lucky.Hr.Entity.Mapping
{
    public class ManagerRecordMap : EntityTypeConfiguration<ManagerRecord>
    {
        public ManagerRecordMap()
        {
            // Primary Key
            this.HasKey(t => t.ManagerId);

            // Properties
            this.Property(t => t.ManagerId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Post)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.HeadImage)
                .IsRequired()
                .HasMaxLength(200);

            this.Property(t => t.CardId)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Native)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Graduation)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Professional)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.AreaId)
                .IsRequired()
                .HasMaxLength(10);

            this.Property(t => t.Street)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.Mobile)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.Phone)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.Email)
                .IsRequired()
                .HasMaxLength(500);

            this.Property(t => t.Qq)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.WeiXin)
                .IsRequired()
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("ManagerRecord");
            this.Property(t => t.ManagerId).HasColumnName("ManagerId");
            this.Property(t => t.Post).HasColumnName("Post");
            this.Property(t => t.HeadImage).HasColumnName("HeadImage");
            this.Property(t => t.Sex).HasColumnName("Sex");
            this.Property(t => t.Birthday).HasColumnName("Birthday");
            this.Property(t => t.CardId).HasColumnName("CardId");
            this.Property(t => t.Native).HasColumnName("Native");
            this.Property(t => t.Graduation).HasColumnName("Graduation");
            this.Property(t => t.Education).HasColumnName("Education");
            this.Property(t => t.Professional).HasColumnName("Professional");
            this.Property(t => t.EntryDate).HasColumnName("EntryDate");
            this.Property(t => t.AreaId).HasColumnName("AreaId");
            this.Property(t => t.Street).HasColumnName("Street");
            this.Property(t => t.Mobile).HasColumnName("Mobile");
            this.Property(t => t.Phone).HasColumnName("Phone");
            this.Property(t => t.Email).HasColumnName("Email");
            this.Property(t => t.Qq).HasColumnName("Qq");
            this.Property(t => t.WeiXin).HasColumnName("WeiXin");
            this.Property(t => t.Salary).HasColumnName("Salary");

            // Relationships
            this.HasRequired(t => t.Manager)
                .WithOptional(t => t.ManagerRecord);

        }
    }
}
