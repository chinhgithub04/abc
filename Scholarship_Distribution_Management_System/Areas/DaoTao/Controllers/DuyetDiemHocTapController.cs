using Microsoft.AspNetCore.Mvc;
using Scholarship_Distribution_Management_System.Models.Entities;
using Scholarship_Distribution_Management_System.Models.ViewModel;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;


namespace Scholarship_Distribution_Management_System.Areas.DaoTao.Controllers
{
    [Area("DaoTao")]
    public class DuyetDiemHocTapController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DuyetDiemHocTapController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var result = from don in _context.DonXinHocBongs
                         join sinhvien in _context.Users on don.IDSinhVien equals sinhvien.Id
                         join lop in _context.LopSHs on sinhvien.IDLopSH equals lop.ID
                         join nganh in _context.Nganhs on lop.IDNganh equals nganh.ID
                         join khoa in _context.Khoas on nganh.IDKhoa equals khoa.ID
                         join dot in _context.DotHocBongs on don.IDDot equals dot.ID
                         where (don.TrangThai == 1 || don.TrangThai == -1) &&
                               (dot.NgayKetThucNop <= DateTime.UtcNow && dot.NgayHoiDongDuyet >= DateTime.UtcNow)
                         select new InfoHocBongViewModel
                         {
                             SinhVienId = sinhvien.Id,
                             HoTen = sinhvien.HoTen,
                             GioiTinh = sinhvien.GioiTinh,
                             NgaySinh = sinhvien.NgaySinh,
                             Email = sinhvien.Email,

                             TenLopSH = lop.TenLopSH,
                             TenNganh = nganh.TenNganh,
                             TenKhoa = khoa.TenKhoa,

                             DonHocBongId = don.ID,
                             NgayNop = don.NgayNop,
                             TrangThaiDon = don.TrangThai,
                             DiemHocTap = don.DiemHocTap,
                             DiemRenLuyen = don.DiemRenLuyen,
                             KQDiemHT = don.KQDiemHT,
                             KQDiemRL = don.KQDiemRL,

                             DotHocBongId = don.IDDot,
                             TenDot = dot.TenDot,
                             //TrangThaiDot = dot.TrangThai,
                             NgayTao = dot.NgayTao,
                             NgayBatDauNop = dot.NgayBatDauNop,
                             NgayKetThucNop = dot.NgayKetThucNop,
                             NgayHoiDongDuyet = dot.NgayHoiDongDuyet,
                             NgayPTCDuyet = dot.NgayPTCDuyet,
                             NgayKetThuc = dot.NgayKetThuc
                         };

            return View(result.ToList());
        }

        public async Task<IActionResult> Details(String id) // Nhận ID của đơn xin học bổng
        {
            if (id == null)
            {
                return NotFound(); // Trả về lỗi 404 nếu không có ID
            }

            // Lấy thông tin chi tiết của đơn xin học bổng bao gồm các thông tin liên quan
            var infoHocBong = await (from don in _context.DonXinHocBongs
                                     join sinhvien in _context.Users on don.IDSinhVien equals sinhvien.Id
                                     join lop in _context.LopSHs on sinhvien.IDLopSH equals lop.ID
                                     join nganh in _context.Nganhs on lop.IDNganh equals nganh.ID
                                     join khoa in _context.Khoas on nganh.IDKhoa equals khoa.ID
                                     where don.ID == id // Lọc theo DonHocBongId
                                     select new InfoHocBongViewModel
                                     {
                                         SinhVienId = sinhvien.Id,
                                         HoTen = sinhvien.HoTen,
                                         GioiTinh = sinhvien.GioiTinh,
                                         NgaySinh = sinhvien.NgaySinh,
                                         Email = sinhvien.Email,

                                         TenLopSH = lop.TenLopSH,
                                         TenNganh = nganh.TenNganh,
                                         TenKhoa = khoa.TenKhoa,

                                         DonHocBongId = don.ID,
                                         NgayNop = don.NgayNop,
                                         TrangThaiDon = don.TrangThai,
                                         DiemHocTap = don.DiemHocTap,
                                         DiemRenLuyen = don.DiemRenLuyen,
                                         KQDiemHT = don.KQDiemHT,
                                         KQDiemRL = don.KQDiemRL,

                                         DotHocBongId = don.IDDot,
                                     }).FirstOrDefaultAsync(); // Lấy một bản ghi duy nhất hoặc null

            if (infoHocBong == null)
            {
                return NotFound(); // Trả về lỗi 404 nếu không tìm thấy đơn
            }

            return View(infoHocBong); // Truyền ViewModel chi tiết đến View
        }
        [HttpPost]
        public async Task<IActionResult> ImportDiem(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Chưa chọn file Excel");

            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                stream.Position = 0;


                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets.First();
                    int rowCount = worksheet.Dimension.Rows;

                    for (int row = 2; row <= rowCount; row++)
                    {
                        string maSV = worksheet.Cells[row, 1].Text.Trim();
                        string diemStr = worksheet.Cells[row, 2].Text.Trim();

                        if (string.IsNullOrEmpty(maSV) || string.IsNullOrEmpty(diemStr))
                            continue;

                        var sinhVien = await _context.Users.FirstOrDefaultAsync(u => u.Id == maSV);
                        if (sinhVien == null)
                            continue;

                        var don = await _context.DonXinHocBongs
                            .Where(d => d.IDSinhVien == maSV)
                            .OrderByDescending(d => d.NgayNop)
                            .FirstOrDefaultAsync();

                        if (don != null && double.TryParse(diemStr, out double diem))
                        {
                            don.KQDiemHT = (float?)diem;
                            _context.DonXinHocBongs.Update(don);
                        }
                    }

                    await _context.SaveChangesAsync();
                }
            }

            TempData["Message"] = "Đã cập nhật điểm thành công!";
            return RedirectToAction("Index");
        }
    }
}

