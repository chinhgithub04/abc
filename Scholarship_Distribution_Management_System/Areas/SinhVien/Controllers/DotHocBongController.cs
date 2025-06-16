using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Scholarship_Distribution_Management_System.Models;
using Scholarship_Distribution_Management_System.Models.Entities;

namespace Scholarship_Distribution_Management_System.Areas.SinhVien.Controllers
{
    [Area("SinhVien")]
    public class DotHocBongController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public DotHocBongController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }        // GET: DotHocBongs
        public async Task<IActionResult> Index()
        {            var applicationDbContext = _context.DotHocBongs
                .Include(d => d.NhanVien)
                .Where(d => d.TrangThai == 1)
                .OrderByDescending(d => d.NgayBatDauNop); // Sort by start date in descending order (newest first)
                
            // Get the list of applications that the current user has already submitted
            var appliedScholarshipIds = new List<string>();
            if (User.Identity.IsAuthenticated && User.IsInRole("SinhVien"))
            {
                var user = await _userManager.GetUserAsync(User);
                appliedScholarshipIds = await _context.DonXinHocBongs
                    .Where(d => d.IDSinhVien == user.Id && d.TrangThai == 1)
                    .Select(d => d.IDDot)
                    .ToListAsync();
            }
            
            ViewBag.AppliedScholarshipIds = appliedScholarshipIds;
            ViewBag.Breadcrumbs = new List<BreadcrumbItem>
            {
                new BreadcrumbItem { Title = "Thông báo", IsActive = true }
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

            bool daDangKy = false;

            // Nếu đã đăng nhập và là Sinh Viên
            if (User.Identity.IsAuthenticated && User.IsInRole("SinhVien"))
            {
                var user = await _userManager.GetUserAsync(User);
                daDangKy = await _context.DonXinHocBongs
                    .AnyAsync(d => d.IDDot == id && d.IDSinhVien == user.Id && d.TrangThai == 1);
            }

            ViewBag.DaDangKy = daDangKy;
            ViewBag.IDDot = id;
            ViewBag.Breadcrumbs = new List<BreadcrumbItem>
            {
                new BreadcrumbItem { Title = "Thông báo", Url = Url.Action("Index"), IsActive = false },
                new BreadcrumbItem { Title = "Chi tiết "+@id, IsActive = true }
            };
            return View(dot);
        }

        public IActionResult Create()
        {
            ViewData["IDNhanVien"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }
    }
}
