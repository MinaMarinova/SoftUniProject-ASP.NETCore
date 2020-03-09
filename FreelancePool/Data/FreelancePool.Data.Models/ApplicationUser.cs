﻿// ReSharper disable VirtualMemberCallInConstructor
namespace FreelancePool.Data.Models
{
    using System;
    using System.Collections.Generic;
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
        }

        public int Stars { get; set; }

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
    }
}
