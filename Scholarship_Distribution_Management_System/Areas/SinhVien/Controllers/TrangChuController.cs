using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Scholarship_Distribution_Management_System.Areas.SinhVien.Controllers
{
    public class TrangChuController : Controller
    {
        [Area("SinhVien")]
        [Route("SinhVien/[controller]/[action]")]
        [Authorize(Roles = "SinhVien")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
