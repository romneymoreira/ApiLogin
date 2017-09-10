using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PDev.Auth.Api.Domain
{
    public class Feature
    {
        public int Id { get; private set; }
        public int IdModule { get; private set; }
        public int? IdFeaturePai { get; private set; }
        public string Name { get; private set; }
        public string Controller { get; private set; }
        public string Link { get; private set; }
        public string Icone { get; private set; }
        public string Situacao { get; private set; }
        public int Ordenacao { get; private set; }

        public virtual Feature FeaturePai { get; private set; }
        public virtual ICollection<UserFeature> UserFeatures { get; private set; }
        public virtual ICollection<Feature> FeatureFilho { get; private set; }
        public virtual ICollection<FeatureAction> Actions { get; private set; }
        public virtual ICollection<Role> Roles { get; set; }

        protected Feature() { }

        public Feature(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(name);
            if (name.Length > 80)
                throw new ArgumentException("O nome da Funcionalidade deve ter no máximo 80 caracteres", "name");

            this.Name = name;
        }

        public void AddAction(FeatureAction action)
        {
            if (Actions == null)
                Actions = new List<FeatureAction>();
            if (action == null)
                throw new ArgumentNullException("action");

            if (Actions.All(f => f.Id != action.Id))
                Actions.Add(action);
        }

        public void RemoveAction(int actionId)
        {
            if (Actions == null)
                return;

            var action = Actions.FirstOrDefault(f => f.Id == actionId);
            if (action != null)
                Actions.Remove(action);
        }
    }
}