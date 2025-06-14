using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Scholarship_Distribution_Management_System.Areas.TaiChinh.Controllers
{
    public class TrangChuController : Controller
    {
        [Area("TaiChinh")]
        [Route("TaiChinh/[controller]/[action]")]
        [Authorize(Roles = "TaiChinh")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
