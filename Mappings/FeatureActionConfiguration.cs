using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDev.Auth.Api.Domain;

namespace PDev.Auth.Api.Mappings
{
    public class FeatureActionConfiguration : EntityTypeConfiguration<FeatureAction>
    {
        public FeatureActionConfiguration()
        {
            this.HasKey(p => p.Id);
            this.Property(p => p.Action)
                .IsRequired()
                .HasMaxLength(50);
        }
    }
}
