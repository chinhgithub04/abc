using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scholarship_Distribution_Management_System.Models.Entities;
using Scholarship_Distribution_Management_System.Models.ViewModel;

namespace Scholarship_Distribution_Management_System.Areas.Doan.Controllers
{
    [Area("Doan")]
    public class DuyetHoatDongController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DuyetHoatDongController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(String idDot)
        {
            var result = await (
                from hdsv in _context.HoatDongCuaSinhViens
                join hoatDong in _context.HoatDongs on hdsv.IDHoatDong equals hoatDong.ID
                join don in _context.DonXinHocBongs on hdsv.IDDon equals don.ID
                where don.IDDot == idDot &&
                      don.TrangThai == 1
                select new HoatDongCuaSinhVienViewModel
                {
                    IDDon = hdsv.IDDon,
                    IDHoatDong = hdsv.IDHoatDong,
                    IDSinhVien = don.IDSinhVien,
                    TenHoatDong = hoatDong.TenHoatDong,
                    DaDuyet = _context.KqHoatDong.Any(k =>
                        k.IDDon == hdsv.IDDon &&
                        k.IDSinhVien == don.IDSinhVien &&
                        k.IDHoatDong == hdsv.IDHoatDong)
                }
            ).ToListAsync();

            return View(result);
        }
    }
}
