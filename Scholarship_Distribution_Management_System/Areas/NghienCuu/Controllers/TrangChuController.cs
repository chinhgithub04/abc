using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Scholarship_Distribution_Management_System.Areas.NghienCuu.Controllers
{
    public class TrangChuController : Controller
    {
        [Area("NghienCuu")]
        [Route("NghienCuu/[controller]/[action]")]
        [Authorize(Roles = "NghienCuu")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
