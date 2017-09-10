using System.Data.Entity.ModelConfiguration;
using PDev.Auth.Api.Domain;

namespace PDev.Auth.Api.Mappings
{
    public class UserFeatureConfiguration : EntityTypeConfiguration<UserFeature>
    {
        public UserFeatureConfiguration()
        {
            this.HasKey(p => new
            {
                p.UserId,
                p.FeatureId
            });

            this.HasMany(p => p.ExcludedActions)
                .WithMany()
                .Map(m =>
                {
                    m.ToTable("UserFeatureAction");
                    m.MapLeftKey("UserId", "FeatureId");
                    m.MapRightKey("FeatureActionId");
                });

            HasRequired(x => x.Feature).WithMany(x => x.UserFeatures).HasForeignKey(p => p.FeatureId);
            HasRequired(x => x.User).WithMany(x => x.UserFeatures).HasForeignKey(p => p.UserId);
        }
    }
}
