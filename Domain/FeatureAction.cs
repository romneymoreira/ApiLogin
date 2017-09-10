using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PDev.Auth.Api.Domain
{
    public class FeatureAction
    {
        public int Id { get; private set; }
        public int IdFeature { get; private set; }
        public string Action { get; private set; }

        protected FeatureAction() { }

        public FeatureAction(string action)
        {
            if (string.IsNullOrEmpty(action))
                throw new ArgumentNullException("action");

            this.Action = action;
        }
    }
}