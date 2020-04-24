namespace FreelancePool.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using FreelancePool.Common;
    using FreelancePool.Data.Common.Repositories;
    using FreelancePool.Data.Models;
    using FreelancePool.Services.Data;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    internal class UsersSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var categoriesService = serviceProvider.GetService<ICategoriesService>();
            var categoryUsersRepository = serviceProvider.GetService<IRepository<CategoryUser>>();
            var configuration = serviceProvider.GetService<IConfiguration>();

            var userRoot = await userManager.FindByEmailAsync(configuration["RootUser:Email"]);

            if (userRoot == null)
            {
                var newUser = new ApplicationUser
                {
                    UserName = configuration["RootUser:UserName"],
                    Email = configuration["RootUser:Email"],
                    PhotoUrl = "https://res.cloudinary.com/freelancepool/image/upload/v1585084662/Categories/recruit_kqvbck.png",
                };

                var result = await userManager.CreateAsync(newUser, configuration["RootUser:Password"]);

                await userManager.AddToRoleAsync(newUser, GlobalConstants.AdministratorRoleName);
            }

            var userAdmin = await userManager.FindByEmailAsync("admin@admin.bg");

            if (userAdmin == null)
            {
                var newUser = new ApplicationUser
                {
                    UserName = "Admin",
                    Email = "admin@admin.bg",
                    PhotoUrl = "https://res.cloudinary.com/freelancepool/image/upload/v1585084662/Categories/recruit_kqvbck.png",
                };

                var result = await userManager.CreateAsync(newUser, "admin123");

                await userManager.AddToRoleAsync(newUser, GlobalConstants.AdministratorRoleName);
            }

            var userPaisii = await userManager.FindByEmailAsync("paisii@paisii.bg");

            if (userPaisii == null)
            {
                var newUser = new ApplicationUser
                {
                    UserName = "Паисий Хилендарски",
                    Email = "paisii@paisii.bg",
                    PhotoUrl = "https://res.cloudinary.com/freelancepool/image/upload/ar_1:1,b_rgb:262c35,bo_5px_solid_rgb:8d8888,c_fill,g_auto,r_max,w_1000/v1585344662/Users/1424-Paisii-Hilendarski.jpg",
                };

                var result = await userManager.CreateAsync(newUser, "paisii123");

                await AddUserToRole(userManager, newUser, result);

                await AddUserToCategoryAsync(newUser, GlobalConstants.WritingCategoryName, categoriesService, categoryUsersRepository);
            }

            var userTuring = await userManager.FindByEmailAsync("turing@turing.bg");

            if (userTuring == null)
            {
                var newUser = new ApplicationUser
                {
                    UserName = "Alan Turing",
                    Email = "turing@turing.bg",
                    PhotoUrl = "https://res.cloudinary.com/freelancepool/image/upload/ar_1:1,b_rgb:262c35,bo_5px_solid_rgb:636060,c_fill,g_auto,r_max,w_1000/v1585346960/Users/Turing_jdberk.jpg",
                };

                var result = await userManager.CreateAsync(newUser, "turing123");

                await AddUserToRole(userManager, newUser, result);

                await AddUserToCategoryAsync(newUser, GlobalConstants.SoftwareDevCategoryName, categoriesService, categoryUsersRepository);
            }

            var userMario = await userManager.FindByEmailAsync("mario@mario.bg");

            if (userMario == null)
            {
                var newUser = new ApplicationUser
                {
                    UserName = "Super Mario",
                    Email = "mario@mario.bg",
                    PhotoUrl = "https://res.cloudinary.com/freelancepool/image/upload/w_1000,c_fill,ar_1:1,g_auto,r_max,bo_5px_solid_red,b_rgb:262c35/v1585397552/Users/x_jpa78254-768x768_ftizdd.jpg",
                };

                var result = await userManager.CreateAsync(newUser, "mario123");

                await AddUserToRole(userManager, newUser, result);

                await AddUserToCategoryAsync(newUser, GlobalConstants.HomeCareCategoryName, categoriesService, categoryUsersRepository);
            }

            var userSam = await userManager.FindByEmailAsync("sam@sam.bg");

            if (userSam == null)
            {
                var newUser = new ApplicationUser
                {
                    UserName = "Uncle Sam",
                    Email = "sam@sam.bg",
                    PhotoUrl = "https://res.cloudinary.com/freelancepool/image/upload/ar_1:1,b_rgb:262c35,bo_5px_solid_rgb:eae3e3,c_fill,g_auto,r_max,w_1000/v1585401782/Users/244px-Uncle_Sam_pointing_finger_titybn.jpg",
                };

                var result = await userManager.CreateAsync(newUser, "sam123");

                await AddUserToRole(userManager, newUser, result);

                await AddUserToCategoryAsync(newUser, GlobalConstants.SalesAndMarketingCategoryName, categoriesService, categoryUsersRepository);
            }

            var userPicasso = await userManager.FindByEmailAsync("picasso@picasso.bg");

            if (userPicasso == null)
            {
                var newUser = new ApplicationUser
                {
                    UserName = "Pablo Picasso",
                    Email = "picasso@picasso.bg",
                    PhotoUrl = "https://res.cloudinary.com/freelancepool/image/upload/ar_1:1,b_rgb:262c35,bo_5px_solid_rgb:e5e0e0,c_fill,g_auto,r_max,w_1000/v1585401939/Users/800px-Pablo_picasso_1_czdusd.jpg",
                };

                var result = await userManager.CreateAsync(newUser, "picasso123");

                await AddUserToRole(userManager, newUser, result);

                await AddUserToCategoryAsync(newUser, GlobalConstants.ArtCategoryName, categoriesService, categoryUsersRepository);
            }

            var userLeonardo = await userManager.FindByEmailAsync("leonardo@leonardo.bg");

            if (userLeonardo == null)
            {
                var newUser = new ApplicationUser
                {
                    UserName = "Leonardo Da Vinci",
                    Email = "leonardo@leonardo.bg",
                    PhotoUrl = "https://res.cloudinary.com/freelancepool/image/upload/ar_1:1,b_rgb:262c35,bo_5px_solid_rgb:343232,c_fill,g_auto,r_max,w_1000/v1585400988/Users/leonardo-da-vinci_h4bhoe.jpg",
                };

                var result = await userManager.CreateAsync(newUser, "leonardo123");

                await AddUserToRole(userManager, newUser, result);

                await AddUserToCategoryAsync(newUser, GlobalConstants.ArtCategoryName, categoriesService, categoryUsersRepository);
                await AddUserToCategoryAsync(newUser, GlobalConstants.EngineeringCategoryName, categoriesService, categoryUsersRepository);
            }

            var userYerevan = await userManager.FindByEmailAsync("yerevan@yerevan.bg");

            if (userYerevan == null)
            {
                var newUser = new ApplicationUser
                {
                    UserName = "Radio Yerevan",
                    Email = "yerevan@yerevan.bg",
                    PhotoUrl = "https://res.cloudinary.com/freelancepool/image/upload/v1585084662/Categories/recruit_kqvbck.png",
                };

                var result = await userManager.CreateAsync(newUser, "yerevan123");

                await AddUserToRole(userManager, newUser, result);

                await AddUserToCategoryAsync(newUser, GlobalConstants.ConsultingCategoryName, categoriesService, categoryUsersRepository);
            }

            var userTeresa = await userManager.FindByEmailAsync("teresa@teresa.bg");

            if (userTeresa == null)
            {
                var newUser = new ApplicationUser
                {
                    UserName = "Mother Teresa",
                    Email = "teresa@teresa.bg",
                    PhotoUrl = "https://res.cloudinary.com/freelancepool/image/upload/ar_1:1,b_rgb:262c35,bo_5px_solid_rgb:ece5e5,c_fill,g_auto,r_max,w_1000/v1585403216/Users/04e24a02f7e21f6da6035e2e29f46b68_hxeeci.jpg",
                };

                var result = await userManager.CreateAsync(newUser, "teresa123");

                await AddUserToRole(userManager, newUser, result);

                await AddUserToCategoryAsync(newUser, GlobalConstants.SocialCareCategoryName, categoriesService, categoryUsersRepository);
            }

            var userBuddha = await userManager.FindByEmailAsync("buddha@buddha.bg");

            if (userBuddha == null)
            {
                var newUser = new ApplicationUser
                {
                    UserName = "Buddha",
                    Email = "buddha@buddha.bg",
                    PhotoUrl = "https://res.cloudinary.com/freelancepool/image/upload/ar_1:1,b_rgb:262c35,bo_5px_solid_rgb:f6f0f0,c_fill,g_auto,r_max,w_1000/v1585403331/Users/buda_resized_ckxpry.jpg",
                };

                var result = await userManager.CreateAsync(newUser, "buddha123");

                await AddUserToRole(userManager, newUser, result);

                await AddUserToCategoryAsync(newUser, GlobalConstants.WellBeingCategoryName, categoriesService, categoryUsersRepository);
            }

            var userShakespeare = await userManager.FindByEmailAsync("shakespeare@shakespeare.bg");

            if (userShakespeare == null)
            {
                var newUser = new ApplicationUser
                {
                    UserName = "William Shakespeare",
                    Email = "shakespeare@shakespeare.bg",
                    PhotoUrl = "https://res.cloudinary.com/freelancepool/image/upload/ar_1:1,b_rgb:262c35,bo_5px_solid_rgb:f0f0f0,c_fill,g_auto,r_max,w_1000/v1585403465/Users/shakespeare_william_z2inqd.jpg",
                };

                var result = await userManager.CreateAsync(newUser, "shakespeare123");

                await AddUserToRole(userManager, newUser, result);

                await AddUserToCategoryAsync(newUser, GlobalConstants.WritingCategoryName, categoriesService, categoryUsersRepository);
            }

            var userIvan = await userManager.FindByEmailAsync("ivan@ivan.bg");

            if (userIvan == null)
            {
                var newUser = new ApplicationUser
                {
                    UserName = "Ivan Popov",
                    Email = "ivan@ivan.bg",
                    PhotoUrl = "https://res.cloudinary.com/freelancepool/image/upload/v1587330707/Users/lfviodngibybka6aaa4y.png",
                };

                var result = await userManager.CreateAsync(newUser, "ivan123");

                if (!result.Succeeded)
                {
                    throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
                }
            }

            var userMilena = await userManager.FindByEmailAsync("milena@milena.bg");

            if (userMilena == null)
            {
                var newUser = new ApplicationUser
                {
                    UserName = "Milena Popova",
                    Email = "milena@milena.bg",
                    PhotoUrl = "https://res.cloudinary.com/freelancepool/image/upload/v1586636165/Users/socvgmgkat48pp9y9ugz.jpg",
                };

                var result = await userManager.CreateAsync(newUser, "milena123");

                if (!result.Succeeded)
                {
                    throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
                }
            }
        }

        private static async Task AddUserToRole(UserManager<ApplicationUser> userManager, ApplicationUser user, IdentityResult result)
        {
            if (!result.Succeeded)
            {
                throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
            }
            else
            {
                await userManager.AddToRoleAsync(user, GlobalConstants.FreelancerRoleName);
            }
        }

        private static async Task AddUserToCategoryAsync(ApplicationUser user, string categoryName, ICategoriesService categoriesService, IRepository<CategoryUser> categoryUsersRepository)
        {
            var categoryId = categoriesService.GetCategoryIdByName(categoryName);

            var categoryUser = new CategoryUser
            {
                User = user,
                CategoryId = categoryId,
            };

            await categoryUsersRepository.AddAsync(categoryUser);
            user.UserCategories.Add(categoryUser);
        }
    }
}
