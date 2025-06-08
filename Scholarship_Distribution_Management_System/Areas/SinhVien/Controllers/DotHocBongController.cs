using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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
        }

        // GET: DotHocBongs
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.DotHocBongs.Include(d => d.NhanVien);
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

            return View(dot);
        }

        // GET: DotHocBongs/Create
        public IActionResult Create()
        {
            ViewData["IDNhanVien"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: DotHocBongs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,IDNhanVien,TenDot,DieuKien,SoLuong,Tien,TrangThai,NgayTao,NgayBatDauNop,NgayKetThucNop,NgayHoiDongDuyet,NgayPTCDuyet,NgayKetThuc")] DotHocBong dotHocBong)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dotHocBong);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IDNhanVien"] = new SelectList(_context.Users, "Id", "Id", dotHocBong.IDNhanVien);
            return View(dotHocBong);
        }

        // GET: DotHocBongs/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dotHocBong = await _context.DotHocBongs.FindAsync(id);
            if (dotHocBong == null)
            {
                return NotFound();
            }
            ViewData["IDNhanVien"] = new SelectList(_context.Users, "Id", "Id", dotHocBong.IDNhanVien);
            return View(dotHocBong);
        }

        // POST: DotHocBongs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ID,IDNhanVien,TenDot,DieuKien,SoLuong,Tien,TrangThai,NgayTao,NgayBatDauNop,NgayKetThucNop,NgayHoiDongDuyet,NgayPTCDuyet,NgayKetThuc")] DotHocBong dotHocBong)
        {
            if (id != dotHocBong.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dotHocBong);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DotHocBongExists(dotHocBong.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["IDNhanVien"] = new SelectList(_context.Users, "Id", "Id", dotHocBong.IDNhanVien);
            return View(dotHocBong);
        }

        // GET: DotHocBongs/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dotHocBong = await _context.DotHocBongs
                .Include(d => d.NhanVien)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (dotHocBong == null)
            {
                return NotFound();
            }

            return View(dotHocBong);
        }

        // POST: DotHocBongs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var dotHocBong = await _context.DotHocBongs.FindAsync(id);
            if (dotHocBong != null)
            {
                _context.DotHocBongs.Remove(dotHocBong);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DotHocBongExists(string id)
        {
            return _context.DotHocBongs.Any(e => e.ID == id);
        }
    }
}
