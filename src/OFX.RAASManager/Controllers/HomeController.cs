using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OFX.RAASManager.Controllers
{
    public class HomeController : ControllerBase
    {
        [AllowAnonymous]
        public IActionResult Swagger()
        {
            return Redirect("swagger/ui");
        }
    }
}