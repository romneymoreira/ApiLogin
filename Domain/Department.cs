using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PDev.Auth.Api.Domain
{
    public class Department
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public virtual ICollection<Role> Roles { get; private set; }

        public Department(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");
            this.Name = name;
        }

        public void AddRole(Role role)
        {
            if (Roles == null)
                Roles = new List<Role>();
            if (role == null)
                throw new ArgumentNullException("role");

            if (Roles.All(r => r.Id != role.Id))
                Roles.Add(role);
        }

        public void RemoveRole(int roleId)
        {
            if (Roles == null)
                return;

            var role = Roles.FirstOrDefault(r => r.Id == roleId);
            if (role != null)
                Roles.Remove(role);
        }
    }
}