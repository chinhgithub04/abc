using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scholarship_Distribution_Management_System.Models.Entities;

namespace Scholarship_Distribution_Management_System.Areas.HoiDong.Controllers
{
    [Area("HoiDong")]
    public class XetDuyetHoSoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public XetDuyetHoSoController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(String idDot)
        {
            var danhSach = await _context.DonXinHocBongs
                .Include(d => d.SinhVien)
                .Include(d => d.DotHocBong)

                .Where(d => d.TrangThai == 1 && d.IDDot == idDot)
                .OrderByDescending(d => d.NgayNop)
                .ToListAsync();

            return View(danhSach);
        }

        // Xem chi tiết hồ sơ
        public async Task<IActionResult> Details(string id)
        {
            if (id == null) return NotFound();

            var don = await _context.DonXinHocBongs
                .Include(d => d.DotHocBong)
                .Include(d => d.SinhVien)
                .Include(d => d.NghienCuuCuaSinhViens!)
                    .ThenInclude(n => n.NghienCuu!)
                .Include(d => d.HoatDongCuaSinhViens!)
                    .ThenInclude(h => h.HoatDong!)
                .FirstOrDefaultAsync(m => m.ID == id);

            if (don == null) return NotFound();

            return View(don);
        }

        // POST: Duyệt hồ sơ
        [HttpPost]
        public async Task<IActionResult> Duyet(string id)
        {
            var don = await _context.DonXinHocBongs.FindAsync(id);
            if (don == null) return NotFound();

            don.TrangThai = 2; // Đã duyệt
            don.DuyetHoiDong = true;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: Từ chối duyệt hồ sơ
        [HttpPost]
        public async Task<IActionResult> TuChoi(string id)
        {
            var don = await _context.DonXinHocBongs.FindAsync(id);
            if (don == null) return NotFound();

            don.TrangThai = -1; // Bị từ chối
            don.DuyetHoiDong = false;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
