using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scholarship_Distribution_Management_System.Models;
using Scholarship_Distribution_Management_System.Models.Entities;

namespace Scholarship_Distribution_Management_System.Areas.NghienCuu.Controllers
{
    [Area("NghienCuu")]
    public class NghienCuuKhoaHocController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NghienCuuKhoaHocController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: NghienCuuKhoaHoc
        public async Task<IActionResult> Index()
        {
            var list = await _context.NghienCuuKhoaHocs.ToListAsync();
            ViewBag.Breadcrumbs = new List<BreadcrumbItem>
            {
                new BreadcrumbItem { Title = "Danh sách nghiên cứu khoa học", IsActive = true }
            };
            return View(list);
        }

        // GET: NghienCuuKhoaHoc/Details/5
        public async Task<IActionResult> Details(string? id)
        {
            if (id == null) return NotFound();

            var nghienCuu = await _context.NghienCuuKhoaHocs.FirstOrDefaultAsync(m => m.ID == id);
            if (nghienCuu == null) return NotFound();
            ViewBag.Breadcrumbs = new List<BreadcrumbItem>
            {
                new BreadcrumbItem { Title = "Danh sách nghiên cứu khoa học", Url = Url.Action("Index"), IsActive = false },
                new BreadcrumbItem { Title = "Chi tiết "+@id, IsActive = true }
            };
            return View(nghienCuu);
        }

        // GET: NghienCuuKhoaHoc/Create
        public IActionResult Create()
        {
            ViewBag.Breadcrumbs = new List<BreadcrumbItem>
            {
                new BreadcrumbItem { Title = "Danh sách nghiên cứu khoa học", Url = Url.Action("Index"), IsActive = false },
                new BreadcrumbItem { Title = "Tạo nghiên cứu khoa học ", IsActive = true }
            };
            return View();
        }

        // POST: NghienCuuKhoaHoc/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NghienCuuKhoaHoc nghienCuu)
        {
            if (ModelState.IsValid)
            {
                _context.Add(nghienCuu);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(nghienCuu);
        }

        // GET: NghienCuuKhoaHoc/Edit/5
        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null) return NotFound();

            var nghienCuu = await _context.NghienCuuKhoaHocs.FindAsync(id);
            if (nghienCuu == null) return NotFound();
            ViewBag.Breadcrumbs = new List<BreadcrumbItem>
            {
                new BreadcrumbItem { Title = "Danh sách nghiên cứu khoa học", Url = Url.Action("Index"), IsActive = false },
                new BreadcrumbItem { Title = "Sửa "+@id, IsActive = true }
            };
            return View(nghienCuu);
        }

        // POST: NghienCuuKhoaHoc/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, NghienCuuKhoaHoc nghienCuu)
        {
            if (id != nghienCuu.ID) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(nghienCuu);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.NghienCuuKhoaHocs.Any(e => e.ID == id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(nghienCuu);
        }

        // POST: NghienCuuKhoaHoc/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            var nghienCuu = await _context.NghienCuuKhoaHocs.FindAsync(id);
            if (nghienCuu != null)
            {
                _context.NghienCuuKhoaHocs.Remove(nghienCuu);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
