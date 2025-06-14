using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Scholarship_Distribution_Management_System.Areas.Doan.Controllers
{
    public class TrangChuController : Controller
    {
        [Area("Doan")]
        [Route("Doan/[controller]/[action]")]
        [Authorize(Roles = "Doan")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
