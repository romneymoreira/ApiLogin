using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDev.Auth.Api.Domain;

namespace PDev.Auth.Api.Mappings
{
    public class UserConfiguration : EntityTypeConfiguration<User>
    {
        public UserConfiguration()
        {
            this.HasKey(p => p.Id);

            this.Property(p => p.UserName)
                .HasMaxLength(25)
                .IsRequired();

            this.Property(p => p.Token)
                .HasMaxLength(200);

            this.Property(p => p.Password)
                .HasMaxLength(128)
                .IsRequired();

            this.Property(p => p.Name)
                .HasMaxLength(120)
                .IsRequired();

            this.Property(p => p.Email)
                .HasMaxLength(40)
                .IsRequired();

            this.Property(p => p.Mobile)
                .HasMaxLength(11);

            this.HasMany(p => p.Roles)
                .WithMany(p => p.Users)
                .Map(m =>
                {
                    m.MapLeftKey("User_Id");
                    m.MapRightKey("Role_Id");
                    m.ToTable("UserRole");
                });

            //this.HasMany(p => p.UserFeatures)
            //    .WithRequired()
            //    .HasForeignKey(p => p.UserId);
        }
    }
}
