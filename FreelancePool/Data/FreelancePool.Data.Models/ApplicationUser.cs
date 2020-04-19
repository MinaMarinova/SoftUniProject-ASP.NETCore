// ReSharper disable VirtualMemberCallInConstructor
namespace FreelancePool.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using FreelancePool.Data.Common.Models;
    using Microsoft.AspNetCore.Identity;

    public class ApplicationUser : IdentityUser, IAuditInfo, IDeletableEntity
    {
        public ApplicationUser()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Roles = new HashSet<IdentityUserRole<string>>();
            this.Claims = new HashSet<IdentityUserClaim<string>>();
            this.Logins = new HashSet<IdentityUserLogin<string>>();
            this.Recommendations = new HashSet<Recommendation>();
            this.RecommendationsMade = new HashSet<Recommendation>();
            this.UserCategories = new HashSet<CategoryUser>();
            this.ProjectsPosted = new HashSet<Project>();
            this.ProjectsCompleted = new HashSet<Project>();
            this.ProjectsOffered = new HashSet<ProjectOfferUser>();
            this.ProjectsApplied = new HashSet<UserCandidateProject>();
        }

        [NotMapped]
        public string EncryptedId { get; set; }

        [StringLength(30, MinimumLength = 3)]
        [Required]
        public override string UserName { get; set; }

        [MaxLength(10000)]
        public string Summary { get; set; }

        [Required]
        [EmailAddress]
        public override string Email { get; set; }

        [Required]
        public override string PasswordHash { get; set; }

        public int Stars { get; set; }

        [Required]
        [Url]
        public string PhotoUrl { get; set; }

        // Audit info
        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        // Deletable entity
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public virtual ICollection<IdentityUserRole<string>> Roles { get; set; }

        public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }

        public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }

        [InverseProperty("Recipient")]
        public virtual ICollection<Recommendation> Recommendations { get; set; }

        [InverseProperty("Author")]
        public virtual ICollection<Recommendation> RecommendationsMade { get; set; }

        public virtual ICollection<CategoryUser> UserCategories { get; set; }

        [InverseProperty("Author")]
        public virtual ICollection<Project> ProjectsPosted { get; set; }

        [InverseProperty("Executor")]
        public virtual ICollection<Project> ProjectsCompleted { get; set; }

        public virtual ICollection<ProjectOfferUser> ProjectsOffered { get; set; }

        public virtual ICollection<UserCandidateProject> ProjectsApplied { get; set; }
    }
}
