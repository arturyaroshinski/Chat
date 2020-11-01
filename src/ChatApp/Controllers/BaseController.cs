using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.Controllers
{
    public class BaseController : Controller
    {
        protected string GetUserId() => User.FindFirst(ClaimTypes.NameIdentifier).Value;
    }
}
