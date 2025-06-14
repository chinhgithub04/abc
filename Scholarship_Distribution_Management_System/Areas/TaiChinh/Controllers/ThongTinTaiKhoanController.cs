using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using Scholarship_Distribution_Management_System.Models.ViewModel;
using Scholarship_Distribution_Management_System.Models;
using Scholarship_Distribution_Management_System.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Scholarship_Distribution_Management_System.Areas.TaiChinh.Controllers
{
    [Area("TaiChinh")]
    public class ThongTinTaiKhoanController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        public ThongTinTaiKhoanController(UserManager<ApplicationUser> userManager, ApplicationDbContext context, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _context = context;
            _roleManager = roleManager;
        }
        public async Task<IActionResult> Details()
        {
            var id = _userManager.GetUserId(User);
            var user = await _userManager.Users
             .Include(u => u.LopSH)
                 .ThenInclude(sv => sv.Nganh)
                    .ThenInclude(l => l.Khoa)
             .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null) return NotFound();

            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault() ?? "Không xác định";

            var model = new TaiKhoanDayDuViewModel
            {
                ID = user.Id,
                HoTen = user.HoTen,
                NgaySinh = user.NgaySinh,
                GioiTinh = user.GioiTinh,
                DiaChi = user.DiaChi,
                Email = user.Email,
                SDT = user.PhoneNumber,
                TrangThai = (int)user.TrangThai,
                UserName = user.UserName,
                PasswordHash = user.PasswordHash,
                VaiTro = role,
                TenLopSH = user.LopSH?.TenLopSH,
                TenNganh = user.LopSH?.Nganh?.TenNganh,
                TenKhoa = user.LopSH?.Nganh?.Khoa?.TenKhoa
            };
            ViewBag.Breadcrumbs = new List<BreadcrumbItem>
            {
                new BreadcrumbItem { Title = "Thông tin cá nhân", IsActive = true }
            };
            return View(model);
        }
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var model = new TaiKhoanDayDuViewModel
            {
                ID = user.Id,
                HoTen = user.HoTen,
                NgaySinh = user.NgaySinh,
                GioiTinh = user.GioiTinh,
                DiaChi = user.DiaChi,
                Email = user.Email,
                SDT = user.PhoneNumber,
                TrangThai = (int)user.TrangThai,
            };
            ViewBag.Breadcrumbs = new List<BreadcrumbItem>
            {
                new BreadcrumbItem { Title = "Thông tin cá nhân", Url = Url.Action("Details"), IsActive = false },
                new BreadcrumbItem { Title = "Sửa thông tin", IsActive = true }
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(TaiKhoanDayDuViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.ID);
            if (user == null) return NotFound();

            user.HoTen = model.HoTen;
            user.NgaySinh = model.NgaySinh;
            user.GioiTinh = model.GioiTinh;
            user.DiaChi = model.DiaChi;
            user.Email = model.Email;
            user.PhoneNumber = model.SDT;
            user.TrangThai = model.TrangThai;

            await _userManager.UpdateAsync(user);
            return RedirectToAction("Details");
        }
    }
}
