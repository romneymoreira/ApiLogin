using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDev.Auth.Api.Domain;

namespace PDev.Auth.Api.Mappings
{
    public class FeatureConfiguration : EntityTypeConfiguration<Feature>
    {
        public FeatureConfiguration()
        {
            this.HasKey(p => p.Id);
            this.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(150);
            this.Property(p => p.Controller)
                .IsRequired()
                .HasMaxLength(50);
            

            this.Property(p => p.Link).IsOptional().HasMaxLength(150);
            this.Property(p => p.Icone).IsOptional().HasMaxLength(20);
            this.Property(p => p.Situacao).IsOptional().HasMaxLength(1);
            this.Property(p => p.Ordenacao).IsOptional();

            //this.Property(p => p.IdFeaturePai).IsOptional();
            this.HasMany(p => p.FeatureFilho).WithOptional(p => p.FeaturePai).HasForeignKey(p => p.IdFeaturePai);

            this.HasMany(p => p.Actions)
                .WithRequired()
                .HasForeignKey(p => p.IdFeature);
        }
    }
}
