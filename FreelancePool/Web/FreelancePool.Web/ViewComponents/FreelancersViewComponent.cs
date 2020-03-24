namespace FreelancePool.Web.ViewComponents
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using FreelancePool.Data.Common.Repositories;
    using FreelancePool.Data.Models;
    using FreelancePool.Services.Data;
    using FreelancePool.Web.ViewModels.Components;
    using Microsoft.AspNetCore.Mvc;

    public class FreelancersViewComponent : ViewComponent
    {
        private readonly IUsersService usersService;

        public FreelancersViewComponent(
            IUsersService usersService)
        {
            this.usersService = usersService;
        }

        public IViewComponentResult Invoke()
        {
            var freelancers = this.usersService.GetRandomEightUsers<RandomFreelancersViewModel>();

            return this.View(freelancers);

        }
    }
}
