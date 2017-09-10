using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PDev.Auth.Api.Domain
{
    public class UserFeature
    {
        public int UserId { get; private set; }
        public int FeatureId { get; private set; }
        public bool CanAccess { get; private set; }
        public virtual Feature Feature { get; private set; }
        public virtual User User { get; private set; }
        public virtual ICollection<FeatureAction> ExcludedActions { get; set; }

        protected UserFeature() { }
        public UserFeature(Feature feature, bool canAccess)
        {
            if (feature == null)
                throw new ArgumentException("feature", "Entidade 'Feature' deve ser válida e diferente de NULL");

            this.Feature = feature;
            this.CanAccess = canAccess;
        }

        public void BlockAction(FeatureAction action)
        {
            if (ExcludedActions == null)
                ExcludedActions = new List<FeatureAction>();
            if (action == null)
                throw new ArgumentNullException("action");

            if (ExcludedActions.All(f => f.Id != action.Id))
                ExcludedActions.Add(action);
        }

        public void UnblockAction(int featureActionId)
        {
            if (ExcludedActions == null)
                return;

            var action = ExcludedActions.FirstOrDefault(f => f.Id == featureActionId);
            if (action != null)
                ExcludedActions.Remove(action);
        }

        public void ChangePermission(bool canAccess)
        {
            this.CanAccess = canAccess;
        }
    }
}