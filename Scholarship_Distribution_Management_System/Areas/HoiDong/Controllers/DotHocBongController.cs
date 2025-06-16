
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;
using Scholarship_Distribution_Management_System.Models;
using Scholarship_Distribution_Management_System.Models.Entities;

namespace Scholarship_Distribution_Management_System.Areas.HoiDong.Controllers
{
    [Area("HoiDong")]
    public class DotHocBongController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public DotHocBongController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        // GET: DotHocBongs
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.DotHocBongs.Include(d => d.NhanVien).Where(d => (d.NgayHoiDongDuyet <= DateTime.Now && d.NgayPTCDuyet >= DateTime.Now) && d.TrangThai == 1);
            ViewBag.Breadcrumbs = new List<BreadcrumbItem>
            {
                new BreadcrumbItem { Title = "Danh sách đợt xét duyệt", IsActive = true }
            };
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: DotHocBongs/Details/5
        public async Task<IActionResult> Details(string id)
        {
            var dot = await _context.DotHocBongs
       .Include(d => d.NhanVien)
       .FirstOrDefaultAsync(d => d.ID == id);

            if (dot == null)
                return NotFound();

            ViewData["IdDot"] = id;
            ViewBag.Breadcrumbs = new List<BreadcrumbItem>
            {
                new BreadcrumbItem { Title = "Danh sách đợt xét duyệt", Url = Url.Action("Index"), IsActive = false },
                new BreadcrumbItem { Title = "Chi tiết "+@id, IsActive = true }
            };
            return View(dot);
        }
    }
}
