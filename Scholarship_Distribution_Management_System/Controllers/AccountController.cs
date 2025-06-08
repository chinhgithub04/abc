using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Scholarship_Distribution_Management_System.Models.Entities;
using Scholarship_Distribution_Management_System.Models.ViewModel;
using System.Security.Claims;
using System.Data;
using Microsoft.EntityFrameworkCore;

namespace Scholarship_Distribution_Management_System.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        [HttpGet]
        public async Task<IActionResult> Login(string? returnUrl = null)
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(User);
                var roles = await _userManager.GetRolesAsync(user);
                var role = roles.FirstOrDefault();

                // Điều hướng theo vai trò
                if (role == "SinhVien")
                    return RedirectToAction("Index", "TrangChu", new { area = "SinhVien" });

                else if (role == "DaoTao")
                    return RedirectToAction("Index", "TrangChu", new { area = "DaoTao" });

                else if (role == "CTSV")
                    return RedirectToAction("Index", "TrangChu", new { area = "CTSV" });

                else if (role == "Doan")
                    return RedirectToAction("Index", "Home", new { area = "Doan" });

                else if (role == "NghienCuu")
                    return RedirectToAction("Index", "Home", new { area = "NghienCuu" });

                else if (role == "HoiDong")
                    return RedirectToAction("Index", "Home", new { area = "HoiDong" });

                else if (role == "TaiChinh")
                    return RedirectToAction("Index", "Home", new { area = "TaiChinh" });

                else if (role == "Admin")
                    return RedirectToAction("Index", "Home", new { area = "Admin" });

                return RedirectToAction("AccessDenied", "Account");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
            if (result.Succeeded)
            {

                // Lấy vai trò
                var roles = await _userManager.GetRolesAsync(user);
                var role = roles.FirstOrDefault();
                string vaiTroHienThi = role switch
                {
                    "SinhVien" => "Sinh viên",
                    "DaoTao" => "Phòng đào tạo",
                    "CTSV" => "Phòng CTSV",
                    "Doan" => "Phòng đoàn",
                    "NghienCuu" => "Phòng NC-KH",
                    "HoiDong" => "Phòng hội đồng",
                    "TaiChinh" => "Phòng tài chính",
                    "Admin" => "Quản trị viên",
                    _ => "Không xác định"
                };
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim("HoTen", user.HoTen ?? ""),
                    new Claim("VaiTro", vaiTroHienThi),
                    new Claim(ClaimTypes.Role, role)
                };

                await _signInManager.SignInWithClaimsAsync(user, isPersistent: model.RememberMe, additionalClaims: claims);

                // Điều hướng theo role
                if (role == "SinhVien")
                    return RedirectToAction("Index", "TrangChu", new { area = "SinhVien" });

                else if (role == "DaoTao")
                    return RedirectToAction("Index", "TrangChu", new { area = "DaoTao" });

                else if (role == "CTSV")
                    return RedirectToAction("Index", "TrangChu", new { area = "CTSV" });

                else if (role == "Doan")
                    return RedirectToAction("Index", "Home", new { area = "Doan" });

                else if (role == "NghienCuu")
                    return RedirectToAction("Index", "Home", new { area = "NghienCuu" });

                else if (role == "HoiDong")
                    return RedirectToAction("Index", "Home", new { area = "HoiDong" });

                else if (role == "TaiChinh")
                    return RedirectToAction("Index", "Home", new { area = "TaiChinh" });

                if (role == "Admin")
                    return RedirectToAction("Index", "Home", new { area = "Admin" });
                // Nếu không rõ vai trò
                return RedirectToAction("AccessDenied", "Account");
            }

            ModelState.AddModelError("", "Sai tài khoản hoặc mật khẩu");
            return View(model);
        }
        [HttpGet]
        public IActionResult Register()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Kiểm tra vai trò hợp lệ nếu có
            if (!string.IsNullOrEmpty(model.Role) && !await _roleManager.RoleExistsAsync(model.Role))
            {
                ModelState.AddModelError("", "Vai trò không tồn tại");
                return View(model);
            }

            var user = new ApplicationUser
            {
                UserName = model.Email, // Email là username
                Email = model.Email,
                HoTen = model.HoTen,
                TrangThai = 1
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            
            if (result.Succeeded)
            {
                Console.WriteLine(_userManager.GetRolesAsync);
                // Gán vai trò nếu có
                if (!string.IsNullOrEmpty(model.Role))
                {
                    await _userManager.AddToRoleAsync(user, model.Role);
                }

                return RedirectToAction("Login", "Account");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}

