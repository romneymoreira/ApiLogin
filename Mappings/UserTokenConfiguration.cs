using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDev.Auth.Api.Domain;

namespace PDev.Auth.Api.Mappings
{
    public class UserTokenConfiguration : EntityTypeConfiguration<UserToken>
    {
        public UserTokenConfiguration()
        {
            this.HasKey(p => p.Id);
            this.Property(p => p.AccessToken)
                .IsRequired()
                .HasMaxLength(2000);

            this.Property(p => p.UserName)
                .IsRequired()
                .HasMaxLength(25);

            this.Property(p => p.Expiration)
                .IsRequired();
        }
    }
}
