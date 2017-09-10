using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDev.Auth.Api.Domain;

namespace PDev.Auth.Api.Mappings
{
    public class RoleConfiguration : EntityTypeConfiguration<Role>
    {
        public RoleConfiguration()
        {
            this.HasKey(p => p.Id);
            this.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);
            this.Property(p => p.Identifier)
                .HasMaxLength(100);

            this.HasMany(p => p.Features)
                .WithMany(p => p.Roles)
                .Map(m =>
                {
                    m.MapLeftKey("Role_Id");
                    m.MapRightKey("Feature_Id");
                    m.ToTable("RoleFeature");
                });

        }
    }
}
