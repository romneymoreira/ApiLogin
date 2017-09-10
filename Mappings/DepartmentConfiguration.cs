using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDev.Auth.Api.Domain;

namespace PDev.Auth.Api.Mappings
{
    public class DepartmentConfiguration : EntityTypeConfiguration<Department>
    {
        public DepartmentConfiguration()
        {
            this.HasKey(p => p.Id);
            this.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(50);

            this.HasMany(p => p.Roles)
                .WithRequired(p => p.Department)
                .HasForeignKey(p => p.IdDepartment);
        }
    }
}
