using Microsoft.AspNetCore.Identity;
using Scholarship_Distribution_Management_System.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using OfficeOpenXml;


var builder = WebApplication.CreateBuilder(args);

// Configure EPPlus license for version 6.x
ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

builder.Services.AddControllersWithViews();

builder.Services.AddMvc();
builder.Services.AddDbContext<ApplicationDbContext>(item =>
    item.UseSqlServer(builder.Configuration.GetConnectionString("myconn")));

// Add Identity (Login, Register, Roles)
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();


// Cấu hình đường dẫn login/logout
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(20); // hết hạn sau 20 phút
    options.SlidingExpiration = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication(); // cho phép hệ thống nhận biết người dùng đăng nhập.
app.UseAuthorization(); // cho phép kiểm tra quyền truy cập theo vai trò.

app.UseStaticFiles(); // nếu dùng wwwroot

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=TrangChu}/{action=Index}/{id?}");

app.MapDefaultControllerRoute();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=TrangChu}/{action=Index}/{id?}");

app.Run();
