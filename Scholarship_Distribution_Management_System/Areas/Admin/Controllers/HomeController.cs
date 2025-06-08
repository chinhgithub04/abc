using Microsoft.AspNetCore.Mvc;

namespace Scholarship_Distribution_Management_System.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
