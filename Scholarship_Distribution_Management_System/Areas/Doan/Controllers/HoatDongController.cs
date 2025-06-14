using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scholarship_Distribution_Management_System.Models;
using Scholarship_Distribution_Management_System.Models.Entities;

namespace Scholarship_Distribution_Management_System.Areas.Doan.Controllers
{
    [Area("Doan")]
    public class HoatDongController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HoatDongController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            ViewBag.Breadcrumbs = new List<BreadcrumbItem>
            {
                new BreadcrumbItem { Title = "Danh sách hoạt động", IsActive = true }
            };
            return View(await _context.HoatDongs.ToListAsync());
        }
        // GET: HoatDongs/Create
        [HttpGet]
        public IActionResult Details(String? id)
        {
            if (id == null) return NotFound();
            var hoatDong = _context.HoatDongs.FirstOrDefault(m => m.ID == id);
            if (hoatDong == null) return NotFound();
            ViewBag.Breadcrumbs = new List<BreadcrumbItem>
            {
                new BreadcrumbItem { Title = "Danh sách hoạt động", Url = Url.Action("Index"), IsActive = false },
                new BreadcrumbItem { Title = "Chi tiết "+@id, IsActive = true }
            };
            return View(hoatDong);
        }
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Breadcrumbs = new List<BreadcrumbItem>
            {
                new BreadcrumbItem { Title = "Danh sách hoạt động", Url = Url.Action("Index"), IsActive = false },
                new BreadcrumbItem { Title = "Tạo hoạt động ", IsActive = true }
            };
            return View();
        }

        // POST: HoatDongs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(HoatDong hoatDong)
        {
            if (ModelState.IsValid)
            {
                _context.Add(hoatDong);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(hoatDong);
        }

        // GET: HoatDongs/Edit/5
        public async Task<IActionResult> Edit(String? id)
        {
            if (id == null) return NotFound();

            var hoatDong = await _context.HoatDongs.FindAsync(id);
            if (hoatDong == null) return NotFound();
            ViewBag.Breadcrumbs = new List<BreadcrumbItem>
            {
                new BreadcrumbItem { Title = "Danh sách hoạt động", Url = Url.Action("Index"), IsActive = false },
                new BreadcrumbItem { Title = "Sửa "+@id, IsActive = true }
            };
            return View(hoatDong);
        }

        // POST: HoatDongs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(String id, HoatDong hoatDong)
        {
            if (id != hoatDong.ID) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hoatDong);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.HoatDongs.Any(e => e.ID == id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(hoatDong);
        }

        public async Task<IActionResult> Delete(String id)
        {
            var hoatDong = await _context.HoatDongs.FindAsync(id);
            _context.HoatDongs.Remove(hoatDong);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
