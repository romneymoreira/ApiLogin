using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PDev.Auth.Api.Domain
{
    public class Role
    {
        public int Id { get; private set; }
        public int IdDepartment { get; private set; }
        public string Name { get; private set; }
        public string Identifier { get; private set; }

        public virtual Department Department { get; private set; }
        public virtual ICollection<Feature> Features { get; private set; }
        public virtual ICollection<User> Users { get; set; }

        protected Role() { }

        public Role(string name, Department department)
        {
            SetName(name);
            SetDepartment(department);
        }

        public void SetName(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");
            this.Name = name;
        }

        public void SetIdentifier(string identifier)
        {
            if (string.IsNullOrEmpty(identifier))
                throw new ArgumentNullException("identifier");
            this.Identifier = identifier.ToUpper();
        }

        public void SetDepartment(Department department)
        {
            if (department == null)
                throw new ArgumentNullException("department");
            this.Department = department;
        }

        public void AddFeature(Feature feature)
        {
            if (Features == null)
                Features = new List<Feature>();
            if (feature == null)
                throw new ArgumentNullException("feature");

            if (Features.All(f => f.Id != feature.Id))
                Features.Add(feature);
        }

        public void RemoveFeature(int featureId)
        {
            if (Features == null)
                return;

            var feature = Features.FirstOrDefault(f => f.Id == featureId);
            if (feature != null)
                Features.Remove(feature);
        }
    }
}