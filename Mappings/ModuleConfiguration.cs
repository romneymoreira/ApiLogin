using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PDev.Auth.Api.Mappings
{
    public class ModuleConfiguration : EntityTypeConfiguration<Domain.Module>
    {
        public ModuleConfiguration()
        {
            this.HasKey(p => p.Id);

            this.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(40);

            this.Property(p => p.KeyModule);

            this.HasMany(p => p.Features)
                .WithRequired()
                .HasForeignKey(p => p.IdModule);
        }
    }
}
