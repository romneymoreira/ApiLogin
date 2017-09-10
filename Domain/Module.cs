using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PDev.Auth.Api.Domain
{
    public class Module
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string KeyModule { get; private set; }

        public ICollection<Feature> Features { get; private set; }

        protected Module() { }

        public Module(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(name);
            this.Name = name;
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