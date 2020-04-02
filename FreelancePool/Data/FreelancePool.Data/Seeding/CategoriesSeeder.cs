namespace FreelancePool.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using FreelancePool.Common;
    using FreelancePool.Data.Models;

    internal class CategoriesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Categories.Any())
            {
                return;
            }

            await dbContext.Categories.AddRangeAsync(
                new Category
                {
                    Name = GlobalConstants.WritingCategoryName,
                    IconURL = "https://res.cloudinary.com/freelancepool/image/upload/v1585310442/Categories/1663291_emshqn.svg",
                },
                new Category
                {
                    Name = GlobalConstants.SoftwareDevCategoryName,
                    IconURL = "https://res.cloudinary.com/freelancepool/image/upload/v1585310372/Categories/1197460_ajxkz1.svg",
                },
                new Category
                {
                    Name = GlobalConstants.HomeCareCategoryName,
                    IconURL = "https://res.cloudinary.com/freelancepool/image/upload/v1585310717/Categories/1997442_fsnfdx.svg",
                },
                new Category
                {
                    Name = GlobalConstants.SalesAndMarketingCategoryName,
                    IconURL = "https://res.cloudinary.com/freelancepool/image/upload/v1585310822/Categories/2221198_a0buc2.png",
                },
                new Category
                {
                    Name = GlobalConstants.ArtCategoryName,
                    IconURL = "https://res.cloudinary.com/freelancepool/image/upload/v1585312864/Categories/562633_srp4bp.svg",
                },
                new Category
                {
                    Name = GlobalConstants.EngineeringCategoryName,
                    IconURL = "https://res.cloudinary.com/freelancepool/image/upload/v1585311010/Categories/1544051_cw1rac.svg",
                },
                new Category
                {
                    Name = GlobalConstants.ConsultingCategoryName,
                    IconURL = "https://res.cloudinary.com/freelancepool/image/upload/v1585312960/Categories/1924237_r1iyy7.svg",
                },
                new Category
                {
                    Name = GlobalConstants.SocialCareCategoryName,
                    IconURL = "https://res.cloudinary.com/freelancepool/image/upload/v1585313079/Categories/829141_mi9kpi.svg",
                },
                new Category
                {
                    Name = GlobalConstants.WellBeingCategoryName,
                    IconURL = "https://res.cloudinary.com/freelancepool/image/upload/v1585313466/Categories/1721094_tpv35p.svg",
                });
        }
    }
}
