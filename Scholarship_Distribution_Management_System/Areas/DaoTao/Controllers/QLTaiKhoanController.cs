using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scholarship_Distribution_Management_System.Models;
using Scholarship_Distribution_Management_System.Models.Entities;
using Scholarship_Distribution_Management_System.Models.ViewModel;

namespace Scholarship_Distribution_Management_System.Areas.DaoTao.Controllers
{
    [Area("DaoTao")]
    public class QLTaiKhoanController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;

        public QLTaiKhoanController(UserManager<ApplicationUser> userManager, ApplicationDbContext context,  RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _context = context;
            _roleManager = roleManager;
        }

        // Danh sách tài khoản
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users
                .Include(u => u.LopSH)
                    .ThenInclude(sv => sv.Nganh)
                        .ThenInclude(l => l.Khoa)
                .ToListAsync();

            var userViewModels = new List<TaiKhoanDayDuViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var role = roles.FirstOrDefault() ?? "Không xác định";

                var model = new TaiKhoanDayDuViewModel
                {
                    ID = user.Id,
                    HoTen = user.HoTen ?? "",
                    NgaySinh = user.NgaySinh,
                    GioiTinh = user.GioiTinh,
                    DiaChi = user.DiaChi,
                    Email = user.Email,
                    SDT = user.PhoneNumber,
                    TrangThai = (int)user.TrangThai,
                    UserName = user.UserName,
                    PasswordHash = user.PasswordHash,
                    VaiTro = role
                };

                if (role == "SinhVien")
                {
                    model.TenLopSH = user.LopSH?.TenLopSH;
                    model.TenNganh = user.LopSH?.Nganh?.TenNganh;
                    model.TenKhoa = user.LopSH?.Nganh?.Khoa?.TenKhoa;
                }

                userViewModels.Add(model);
            }
            ViewBag.Breadcrumbs = new List<BreadcrumbItem>
            {
                new BreadcrumbItem { Title = "Quản lý tài khoản ", IsActive = true }
            };
            return View(userViewModels);
        }
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Breadcrumbs = new List<BreadcrumbItem>
            {
                new BreadcrumbItem { Title = "Quản lý tài khoản", Url = Url.Action("Index"), IsActive = false },
                new BreadcrumbItem { Title = "Tạo tài khoản", IsActive = true }
            };
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TaiKhoanDayDuViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Kiểm tra xem email đã tồn tại chưa
            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError("Email", "Email đã được sử dụng.");
                return View(model);
            }

            var user = new ApplicationUser
            {
                Id = model.ID,
                UserName = model.Email,
                Email = model.Email,
                HoTen = model.HoTen,
                NgaySinh = model.NgaySinh,
                GioiTinh = model.GioiTinh,
                DiaChi = model.DiaChi,
                PhoneNumber = model.SDT,
                TrangThai = 1, // hoạt động mặc định
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                // Gán vai trò
                if (!await _roleManager.RoleExistsAsync(model.VaiTro))
                {
                    await _roleManager.CreateAsync(new IdentityRole(model.VaiTro));
                }

                await _userManager.AddToRoleAsync(user, model.VaiTro);
                return RedirectToAction("Index");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View(model);
        }
        // Xem chi tiết
        public async Task<IActionResult> Details(string id)
        {
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
                new BreadcrumbItem { Title = "Quản lý tài khoản", Url = Url.Action("Index"), IsActive = false },
                new BreadcrumbItem { Title = "Chi tiết "+@id, IsActive = true }
            };
            return View(model);
        }

        // Sửa thông tin cơ bản
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
                new BreadcrumbItem { Title = "Quản lý tài khoản", Url = Url.Action("Index"), IsActive = false },
                new BreadcrumbItem { Title = "Sửa tài khoản "+@id, IsActive = true }
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
            return RedirectToAction("Index");
        }

        // Xóa tài khoản
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null && user.TrangThai!=0)
            {
                user.TrangThai = 0;
            }
            else
            {
                user.TrangThai = 1;
                
            }
            await _userManager.UpdateAsync(user);
            return RedirectToAction("Index");
        }

    }
}
