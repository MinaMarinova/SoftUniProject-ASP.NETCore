namespace FreelancePool.Web.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using FreelancePool.Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class TestController : BaseController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<ApplicationRole> roleManager;

        public TestController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<ApplicationRole> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }

        public async Task<IActionResult> CreateUser()
        {
            var result = await this.userManager.CreateAsync(
                new ApplicationUser
                {
                    UserName = "Gosho",
                    Email = "gosho@boyan.bg",
                    PhoneNumber = "+359887232323",
                    EmailConfirmed = true,
                }, "Gosho@ivan.bg71911");

            if (!result.Succeeded)
            {
                return this.BadRequest(string.Join("; ", result.Errors.Select(r => r.Description)));
            }

            var result2 = await this.signInManager.PasswordSignInAsync("Gosho", "Gosho@ivan.bg71911", true, false);

            await this.roleManager.CreateAsync(new ApplicationRole { Name = "Admin" });
            var user = await this.userManager.GetUserAsync(this.User);
            await this.userManager.AddToRoleAsync(user, "Admin");

            return this.Ok();
        }
    }
}
