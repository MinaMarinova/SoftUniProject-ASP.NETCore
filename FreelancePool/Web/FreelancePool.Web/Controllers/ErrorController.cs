namespace FreelancePool.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class ErrorController : BaseController
    {
        [Route("Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            switch (statusCode)
            {
                case 404:
                    this.ViewBag.ErrorMessage = "Oooops!!! The resource could not be found!";
                    break;
            }

            return this.View("NotFound");
        }
    }
}
