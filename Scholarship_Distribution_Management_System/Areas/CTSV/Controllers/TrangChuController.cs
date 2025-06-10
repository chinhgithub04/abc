using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Scholarship_Distribution_Management_System.Areas.CTSV.Controllers
{
    public class TrangChuController : Controller
    {
        [Area("CTSV")]
        [Route("CTSV/[controller]/[action]")]
        [Authorize(Roles = "CTSV")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
