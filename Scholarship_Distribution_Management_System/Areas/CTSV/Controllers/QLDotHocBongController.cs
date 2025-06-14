using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scholarship_Distribution_Management_System.Models;
using Scholarship_Distribution_Management_System.Models.Entities;
using System.Security.Claims;

namespace Scholarship_Distribution_Management_System.Areas.CTSV.Controllers
{
    [Area("CTSV")]
    public class QLDotHocBongController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public QLDotHocBongController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        // GET: QLDotHocBong
        public async Task<IActionResult> Index()
        {
            var list = await _context.DotHocBongs
                .Include(d => d.NhanVien)
                .OrderByDescending(d => d.TrangThai == 1)
                .ThenByDescending(d => d.NgayTao)
                .ToListAsync();
            ViewBag.Breadcrumbs = new List<BreadcrumbItem>
            {
                new BreadcrumbItem { Title = "Quản lý đợt học bổng", IsActive = true }
            };
            return View(list);
        }

        // GET: QLDotHocBong/Create
        public async Task<IActionResult> Create()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var last = await _context.DotHocBongs
                .OrderByDescending(d => d.ID)
                .FirstOrDefaultAsync();

            int lastNumber = 0;
            if (last != null && last.ID.Length > 3 && int.TryParse(last.ID.Substring(3), out var num))
            {
                lastNumber = num;
            }

            String id = "DOT" + (lastNumber + 1).ToString("D3");
            ViewBag.Breadcrumbs = new List<BreadcrumbItem>
            {
                new BreadcrumbItem { Title = "Quản lý đợt học bổng", Url = Url.Action("Index"), IsActive = false },
                new BreadcrumbItem { Title = "Thêm đợt học bổng", IsActive = true }
            };
            return View(new DotHocBong
            {
                ID = id,
                IDNhanVien = user.Id,
                NgayTao = DateTime.Now,
                NgayBatDauNop = DateTime.Today,
                NgayKetThucNop = DateTime.Today.AddDays(7),
                NgayHoiDongDuyet = DateTime.Today.AddDays(10),
                NgayPTCDuyet = DateTime.Today.AddDays(15),
                NgayKetThuc = DateTime.Today.AddDays(20),

            });
        }

        // POST: QLDotHocBong/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DotHocBong model)
        {
            if (ModelState.IsValid)
            {
                _context.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: QLDotHocBong/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            var dot = await _context.DotHocBongs.FindAsync(id);
            if (dot == null) return NotFound();
            ViewBag.Breadcrumbs = new List<BreadcrumbItem>
            {
                new BreadcrumbItem { Title = "Quản lý đợt học bổng", Url = Url.Action("Index"), IsActive = false },
                new BreadcrumbItem { Title = "Sửa " + @id, IsActive = true }
            };
            return View(dot);
        }

        // POST: QLDotHocBong/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, DotHocBong model)
        {
            if (id != model.ID) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(model);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.DotHocBongs.Any(e => e.ID == id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // POST: QLDotHocBong/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            var dot = await _context.DotHocBongs.FindAsync(id);
            if (dot != null)
            {
                dot.TrangThai = 0; // Đánh dấu là đã hủy hoặc ngừng hiệu lực
                _context.Update(dot);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelDelete(string id)
        {
            var dot = await _context.DotHocBongs.FindAsync(id);
            if (dot != null)
            {
                dot.TrangThai = 1;
                dot.NgayTao = DateTime.Now;
                _context.Update(dot);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
        // GET: QLDotHocBong/Details/5
        public async Task<IActionResult> Details(string id)
        {
            var dot = await _context.DotHocBongs
                .Include(d => d.NhanVien)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (dot == null) return NotFound();
            ViewBag.Breadcrumbs = new List<BreadcrumbItem>
            {
                new BreadcrumbItem { Title = "Quản lý đợt học bổng", Url = Url.Action("Index"), IsActive = false },
                new BreadcrumbItem { Title = "Chi tiết " + @id, IsActive = true }
            };
            return View(dot);
        }
    }
}