namespace FreelancePool.Data
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;

    using FreelancePool.Data.Common.Models;
    using FreelancePool.Data.Models;

    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        private static readonly MethodInfo SetIsDeletedQueryFilterMethod =
            typeof(ApplicationDbContext).GetMethod(
                nameof(SetIsDeletedQueryFilter),
                BindingFlags.NonPublic | BindingFlags.Static);

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Setting> Settings { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Project> Projects { get; set; }

        public DbSet<Recommendation> Recommendations { get; set; }

        public DbSet<Message> Messages { get; set; }

        public DbSet<Post> Posts { get; set; }

        public DbSet<CategoryUser> CategoriesUsers { get; set; }

        public DbSet<CategoryProject> CategoriesProjects { get; set; }

        public DbSet<ProjectOfferUser> ProjectOffersUsers { get; set; }

        public DbSet<UserCandidateProject> UsersCandidateProjects { get; set; }

        public DbSet<CategoryPost> CategoriesPosts { get; set; }

        public override int SaveChanges() => this.SaveChanges(true);

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            this.ApplyAuditInfoRules();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
            this.SaveChangesAsync(true, cancellationToken);

        public override Task<int> SaveChangesAsync(
            bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default)
        {
            this.ApplyAuditInfoRules();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Needed for Identity models configuration
            base.OnModelCreating(builder);

            ConfigureUserIdentityRelations(builder);

            EntityIndexesConfiguration.Configure(builder);

            var entityTypes = builder.Model.GetEntityTypes().ToList();

            // Set global query filter for not deleted entities only
            var deletableEntityTypes = entityTypes
                .Where(et => et.ClrType != null && typeof(IDeletableEntity).IsAssignableFrom(et.ClrType));
            foreach (var deletableEntityType in deletableEntityTypes)
            {
                var method = SetIsDeletedQueryFilterMethod.MakeGenericMethod(deletableEntityType.ClrType);
                method.Invoke(null, new object[] { builder });
            }

            // Disable cascade delete
            var foreignKeys = entityTypes
                .SelectMany(e => e.GetForeignKeys().Where(f => f.DeleteBehavior == DeleteBehavior.Cascade));
            foreach (var foreignKey in foreignKeys)
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }

        private static void ConfigureUserIdentityRelations(ModelBuilder builder)
        {
            builder.Entity<ApplicationUser>()
                .HasMany(e => e.Claims)
                .WithOne()
                .HasForeignKey(e => e.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ApplicationUser>()
                .HasMany(e => e.Logins)
                .WithOne()
                .HasForeignKey(e => e.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ApplicationUser>()
                .HasMany(e => e.Roles)
                .WithOne()
                .HasForeignKey(e => e.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            // CategoriesUsers
            builder
                .Entity<CategoryUser>(categoryUser =>
                {
                    categoryUser
                    .HasKey(cu => new { cu.CategoryId, cu.UserId });

                    categoryUser
                    .HasOne(cu => cu.Category)
                    .WithMany(c => c.CategoryUsers)
                    .HasForeignKey(cu => cu.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);

                    categoryUser
                    .HasOne(cu => cu.User)
                    .WithMany(u => u.UserCategories)
                    .HasForeignKey(cu => cu.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
                });

            // CategoryProject
            builder
                .Entity<CategoryProject>(categoryProject =>
                {
                    categoryProject
                    .HasKey(cp => new { cp.CategoryId, cp.ProjectId });

                    categoryProject
                    .HasOne(cp => cp.Category)
                    .WithMany(c => c.CategoryProjects)
                    .HasForeignKey(cp => cp.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);

                    categoryProject
                    .HasOne(cp => cp.Project)
                    .WithMany(p => p.ProjectCategories)
                    .HasForeignKey(cp => cp.ProjectId)
                    .OnDelete(DeleteBehavior.Restrict);
                });

            // ProjectOfferUser
            builder.Entity<ProjectOfferUser>(projectOfferUser =>
            {
                projectOfferUser
                .HasKey(pu => new { pu.UserId, pu.ProjectId });

                projectOfferUser
                .HasOne(po => po.User)
                .WithMany(u => u.ProjectsOffered)
                .HasForeignKey(po => po.UserId)
                .OnDelete(DeleteBehavior.Restrict);

                projectOfferUser
                .HasOne(po => po.Project)
                .WithMany(p => p.SuggestedUsers)
                .HasForeignKey(po => po.ProjectId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            //UserCandidateProject
            builder.Entity<UserCandidateProject>(usersCandidateProjects =>
            {
                usersCandidateProjects
                .HasKey(ucp => new { ucp.UserId, ucp.ProjectId });

                usersCandidateProjects
                .HasOne(ucp => ucp.User)
                .WithMany(u => u.ProjectsApplied)
                .HasForeignKey(ucp => ucp.UserId)
                .OnDelete(DeleteBehavior.Restrict);

                usersCandidateProjects
                .HasOne(ucp => ucp.Project)
                .WithMany(p => p.AppliedUsers)
                .HasForeignKey(ucp => ucp.ProjectId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            // CategoryPost
            builder.Entity<CategoryPost>(categoryPost =>
            {
                categoryPost
                .HasKey(cp => new { cp.CategoryId, cp.PostId });

                categoryPost
                .HasOne(cp => cp.Category)
                .WithMany(c => c.CategoryPosts)
                .HasForeignKey(cp => cp.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

                categoryPost
                .HasOne(cp => cp.Post)
                .WithMany(p => p.PostCategories)
                .HasForeignKey(cp => cp.PostId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            // Recommandations
            builder.Entity<Recommendation>(recommandation =>
            {
                recommandation
                .HasOne(r => r.Author)
                .WithMany(a => a.RecommendationsMade)
                .HasForeignKey(r => r.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);

                recommandation
                .HasOne(r => r.Recipient)
                .WithMany(u => u.Recommendations)
                .HasForeignKey(r => r.RecipientId)
                .OnDelete(DeleteBehavior.Restrict);
            });
        }

        private static void SetIsDeletedQueryFilter<T>(ModelBuilder builder)
            where T : class, IDeletableEntity
        {
            builder.Entity<T>().HasQueryFilter(e => !e.IsDeleted);
        }

        private void ApplyAuditInfoRules()
        {
            var changedEntries = this.ChangeTracker
                .Entries()
                .Where(e =>
                    e.Entity is IAuditInfo &&
                    (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entry in changedEntries)
            {
                var entity = (IAuditInfo)entry.Entity;
                if (entry.State == EntityState.Added && entity.CreatedOn == default)
                {
                    entity.CreatedOn = DateTime.UtcNow;
                }
                else
                {
                    entity.ModifiedOn = DateTime.UtcNow;
                }
            }
        }
    }
}
