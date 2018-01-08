using Microsoft.AspNetCore.Mvc;

namespace BalanceWebApp.Controllers
{
    [Route("login")]
    public class LoginController : BaseController
    {
        public IActionResult Login()
        {
            return View();
        }
    }
}