using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scholarship_Distribution_Management_System.Models.Entities;

namespace Scholarship_Distribution_Management_System.Areas.CTSV.Controllers
{
    public class QLDotHocBongController : Controller
    {
        private readonly ApplicationDbContext _context;
        public QLDotHocBongController(ApplicationDbContext context)
        {
            _context = context;
        }
        // GET: QLDotHocBong
        public async Task<IActionResult> Index()
        {
            var list = await _context.DotHocBongs
                .Include(d => d.NhanVien) // Nếu có quan hệ cần load
                .ToListAsync();
            return View(list);
        }

        // GET: QLDotHocBong/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: QLDotHocBong/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DotHocBong model)
        {
            if (ModelState.IsValid)
            {
                model.NgayTao = DateTime.Now;
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

        // GET: QLDotHocBong/Details/5
        public async Task<IActionResult> Details(string id)
        {
            var dot = await _context.DotHocBongs
                .Include(d => d.NhanVien)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (dot == null) return NotFound();
            return View(dot);
        }
    }
}