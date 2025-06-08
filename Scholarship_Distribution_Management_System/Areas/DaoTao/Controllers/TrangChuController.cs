using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Scholarship_Distribution_Management_System.Areas.DaoTao.Controllers
{
    public class TrangChuController : Controller
    {
        [Area("DaoTao")]
        [Route("DaoTao/[controller]/[action]")]
        [Authorize(Roles = "DaoTao")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
