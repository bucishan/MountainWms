using Microsoft.AspNetCore.Mvc;

namespace MountainWMS.Controllers
{
    public class RoutingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}