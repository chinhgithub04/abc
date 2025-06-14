using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scholarship_Distribution_Management_System.Models;
using Scholarship_Distribution_Management_System.Models.Entities;
using X.PagedList;
using X.PagedList.Extensions;

namespace Scholarship_Distribution_Management_System.Areas.SinhVien.Controllers
{
    [Area("SinhVien")]
    public class DangKyHoSoController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DangKyHoSoController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        public async Task<IActionResult> Index(int? page)
        {
            int pageSize = 5;
            int pageNumber = page ?? 1;

            var user = await _userManager.GetUserAsync(User);
            var danhSach = _context.DonXinHocBongs
                .Include(d => d.DotHocBong)
                .Where(d => d.IDSinhVien == user.Id)
                .OrderByDescending(d => d.TrangThai == 1)
                .ThenByDescending(d => d.NgayNop)
                .ToPagedList(pageNumber, pageSize);
            ViewBag.Breadcrumbs = new List<BreadcrumbItem>
            {
                new BreadcrumbItem { Title = "Trạng thái xét duyệt học bổng ", IsActive = true }
            };
            return View(danhSach);
        }
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
                return NotFound();

            var don = await _context.DonXinHocBongs
                .Include(d => d.DotHocBong)
                .Include(d => d.SinhVien)
                .Include(d => d.NghienCuuCuaSinhViens!)
                    .ThenInclude(n => n.NghienCuu!)
                .Include(d => d.HoatDongCuaSinhViens!)
                    .ThenInclude(h => h.HoatDong!)
                .FirstOrDefaultAsync(m => m.ID == id);

            if (don == null)
                return NotFound();
            ViewBag.Breadcrumbs = new List<BreadcrumbItem>
            {
                new BreadcrumbItem { Title = "Trạng thái xét duyệt học bổng", Url = Url.Action("Index"), IsActive = false },
                new BreadcrumbItem { Title = "Chi tiết " + @id,IsActive = true }
            };
            return View(don);
        }
        public async Task<IActionResult> Create(string idDot)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var dotHocBong = await _context.DotHocBongs.FindAsync(idDot);
            if (dotHocBong == null)
            {
                return NotFound("Đợt học bổng không tồn tại.");
            }

            // Fetch lists for research and activities
            var nghienCuuList = await _context.NghienCuuKhoaHocs.Select(n => new { n.ID, Label = n.TenDeTai + " - " + n.ThanhTich }).ToListAsync();
            var hoatDongList = await _context.HoatDongs.Select(n => n.TenHoatDong).ToListAsync();

            ViewBag.NghienCuuList = nghienCuuList;
            ViewBag.HoatDongList = hoatDongList;
            ViewBag.DotHocBong = dotHocBong;
            ViewBag.Breadcrumbs = new List<BreadcrumbItem>
            {
                new BreadcrumbItem { Title = "Trạng thái xét duyệt học bổng", Url = Url.Action("Index"), IsActive = false },
                new BreadcrumbItem { Title = "Tạo hồ sơ xin cấp phát học bổng ", IsActive = true }
            };
            return View(new DonXinHocBong
            {
                IDDot = idDot,
                IDSinhVien = user.Id,
                NgayNop = DateTime.Now
            });
        }

        // POST: Submit scholarship application
        [HttpPost]
        public async Task<IActionResult> Create(DonXinHocBong model, string[] selectedNghienCuu, string[] selectedHoatDong)
        {

            foreach (var key in ModelState.Keys)
            {
                var state = ModelState[key];
                foreach (var error in state.Errors)
                {
                    Console.WriteLine($"Key: {key}, Error: {error.ErrorMessage}");
                }
            }
            if (ModelState.IsValid)
            {
                model.ID = Guid.NewGuid().ToString();
                model.TrangThai = 1;
                model.DuyetHoiDong = false;
                model.DuyetCapPhatHocBong = false;

                // Save DONXINHOCBONG
                _context.DonXinHocBongs.Add(model);

                // Save selected NGHIENCUUCUASINHVIEN
                if (selectedNghienCuu != null)
                {
                    foreach (var idNghienCuu in selectedNghienCuu.Distinct())
                    {
                        var exists = await _context.nghienCuuCuaSinhViens
                            .AnyAsync(n => n.IDDon == model.ID && n.IDNghienCuu == idNghienCuu);
                        if (!exists)
                        {
                            _context.nghienCuuCuaSinhViens.Add(new NghienCuuCuaSinhVien
                            {
                                IDDon = model.ID,
                                IDNghienCuu = idNghienCuu
                            });
                        }
                    }

                }

                // Save selected HOATDONGCUASINHVIEN
                if (selectedHoatDong != null)
                {
                    foreach (var tenHoatDong in selectedHoatDong)
                    {
                        var hoatDong = await _context.HoatDongs
                            .FirstOrDefaultAsync(h => h.TenHoatDong == tenHoatDong);
                        if (hoatDong != null)
                        {
                            _context.HoatDongCuaSinhViens.Add(new HoatDongCuaSinhVien
                            {
                                IDDon = model.ID,
                                IDHoatDong = hoatDong.ID
                            });
                        }
                    }
                }

                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            // If validation fails, repopulate the ViewBag and return the form
            ViewBag.NghienCuuList = await _context.NghienCuuKhoaHocs.Select(n => n.TenDeTai).ToListAsync();
            ViewBag.HoatDongList = await _context.HoatDongs.Select(h => h.TenHoatDong).ToListAsync();
            ViewBag.DotHocBong = await _context.DotHocBongs.FindAsync(model.IDDot);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> HuyDon(string id)
        {
            var don = await _context.DonXinHocBongs.FindAsync(id);
            if (don == null)
            {
                return NotFound();
            }

            if (don.TrangThai != 1)
            {
                return BadRequest("Chỉ có thể hủy đơn ở trạng thái chờ xử lý.");
            }

            don.TrangThai = 0; // Đã hủy
            await _context.SaveChangesAsync();

            return RedirectToAction("Index"); // hoặc trang danh sách đơn của sinh viên
        }

    }
}
