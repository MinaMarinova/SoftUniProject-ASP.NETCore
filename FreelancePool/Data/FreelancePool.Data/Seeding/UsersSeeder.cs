namespace FreelancePool.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using FreelancePool.Common;
    using FreelancePool.Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;

    internal class UsersSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

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
    }
}
