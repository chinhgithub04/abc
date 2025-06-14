using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scholarship_Distribution_Management_System.Models.Entities;
using Scholarship_Distribution_Management_System.Models.ViewModel;

namespace Scholarship_Distribution_Management_System.Areas.NghienCuu.Controllers
{
    [Area("NghienCuu")]
    public class DuyetNghienCuuController : Controller
    {
        private readonly ApplicationDbContext _context;
        public DuyetNghienCuuController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(String idDot)
        {
            var result = await (
                from ncsv in _context.nghienCuuCuaSinhViens
                join nc in _context.NghienCuuKhoaHocs on ncsv.IDNghienCuu equals nc.ID
                join don in _context.DonXinHocBongs on ncsv.IDDon equals don.ID
                where don.IDDot == idDot && don.TrangThai == 1
                select new NghienCuuCuaSinhVienViewModel
                {
                    IDDon = ncsv.IDDon,
                    IDNghienCuu = ncsv.IDNghienCuu,
                    IDSinhVien = don.IDSinhVien,
                    TenDeTai = nc.TenDeTai,
                    ThanhTich = nc.ThanhTich,
                    DaDuyet = _context.KqNghienCuu.Any(k =>
                        k.IDDon == ncsv.IDDon &&
                        k.IDSinhVien == don.IDSinhVien &&
                        k.IDNghienCuu == ncsv.IDNghienCuu)
                }
            ).ToListAsync();

            return View(result);
        }
    }
}
