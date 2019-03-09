using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace BalanceWebApp.Controllers
{
    public abstract class BaseController : Controller
    {
        protected string GetAuthenticatedUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }
    }
}