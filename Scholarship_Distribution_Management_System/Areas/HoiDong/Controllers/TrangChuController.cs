using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scholarship_Distribution_Management_System.Models.Entities;
using Scholarship_Distribution_Management_System.Models.ViewModel;

namespace Scholarship_Distribution_Management_System.Areas.HoiDong.Controllers
{
    public class TrangChuController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public TrangChuController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Area("HoiDong")]
        public IActionResult Index()
        {
            return View();
        }
       
    }
}
