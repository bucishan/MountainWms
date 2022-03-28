using Microsoft.AspNetCore.Mvc;

namespace MountainWMS.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
