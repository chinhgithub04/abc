using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scholarship_Distribution_Management_System.Models;
using Scholarship_Distribution_Management_System.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using System;
using System.Linq;

namespace Scholarship_Distribution_Management_System.Areas.DaoTao.Controllers
{
    [Area("DaoTao")]
    public class DiemHocTapController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DiemHocTapController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var diemHocTapList = await _context.Diems
                .Include(d => d.User)
                    .ThenInclude(u => u.LopSH)
                        .ThenInclude(l => l.Nganh)
                            .ThenInclude(n => n.Khoa)
                .Where(d => d.User != null)
                .ToListAsync();

            ViewBag.Breadcrumbs = new List<BreadcrumbItem>
            {
                new BreadcrumbItem { Title = "Danh sách điểm học tập", IsActive = true }
            };

            return View(diemHocTapList);
        }

        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var diem = await _context.Diems
                .Include(d => d.User)
                    .ThenInclude(u => u.LopSH)
                        .ThenInclude(l => l.Nganh)
                            .ThenInclude(n => n.Khoa)
                .FirstOrDefaultAsync(d => d.ID == id);

            if (diem == null)
            {
                return NotFound();
            }

            ViewBag.Breadcrumbs = new List<BreadcrumbItem>
            {
                new BreadcrumbItem { Title = "Danh sách điểm học tập", Url = Url.Action("Index"), IsActive = false },
                new BreadcrumbItem { Title = "Chi tiết điểm của " + diem.User?.HoTen, IsActive = true }
            };

            return View(diem);
        }        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var diem = await _context.Diems
                .Include(d => d.User)
                    .ThenInclude(u => u.LopSH)
                        .ThenInclude(l => l.Nganh)
                            .ThenInclude(n => n.Khoa)
                .FirstOrDefaultAsync(d => d.ID == id);

            if (diem == null)
            {
                return NotFound();
            }

            ViewBag.Breadcrumbs = new List<BreadcrumbItem>
            {
                new BreadcrumbItem { Title = "Danh sách điểm học tập", Url = Url.Action("Index"), IsActive = false },
                new BreadcrumbItem { Title = "Cập nhật điểm của " + diem.User?.HoTen, IsActive = true }
            };

            return View(diem);
        }        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ID,DiemHocTap,DiemRenLuyen")] Diem diem)
        {
            if (id != diem.ID)
            {
                return NotFound();
            }            // Always proceed regardless of ModelState.IsValid for this simple form
            try
            {
                var existingDiem = await _context.Diems.FindAsync(id);
                if (existingDiem == null)
                {
                    return NotFound();
                }

                existingDiem.DiemHocTap = diem.DiemHocTap;
                existingDiem.DiemRenLuyen = diem.DiemRenLuyen;

                _context.Update(existingDiem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DiemExists(diem.ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            
            // If we got this far, something failed, redisplay form
            // Load the user details again to properly display in the form
            var fullDiem = await _context.Diems
                .Include(d => d.User)
                    .ThenInclude(u => u.LopSH)
                        .ThenInclude(l => l.Nganh)
                            .ThenInclude(n => n.Khoa)
                .FirstOrDefaultAsync(d => d.ID == id);
                
            return View(fullDiem);
        }

        public async Task<IActionResult> ExportToExcel()
        {            // Get the data
            var diemHocTapList = await _context.Diems
                .Include(d => d.User)
                    .ThenInclude(u => u.LopSH)
                        .ThenInclude(l => l.Nganh)
                            .ThenInclude(n => n.Khoa)
                .Where(d => d.User != null)
                .ToListAsync();
            
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Điểm học tập");
                
                // Add header with styling
                worksheet.Cells[1, 1].Value = "STT";
                worksheet.Cells[1, 2].Value = "Mã sinh viên";
                worksheet.Cells[1, 3].Value = "Họ và tên";
                worksheet.Cells[1, 4].Value = "Khoa";
                worksheet.Cells[1, 5].Value = "Ngành";
                worksheet.Cells[1, 6].Value = "Lớp";
                worksheet.Cells[1, 7].Value = "Điểm học tập";
                worksheet.Cells[1, 8].Value = "Điểm rèn luyện";
                
                // Style the header
                using (var range = worksheet.Cells[1, 1, 1, 8])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                    range.Style.Font.Color.SetColor(Color.Black);
                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }
                
                // Add data
                int row = 2;
                for (int i = 0; i < diemHocTapList.Count; i++)
                {
                    var diem = diemHocTapList[i];
                    
                    worksheet.Cells[row, 1].Value = i + 1;
                    worksheet.Cells[row, 2].Value = diem.ID;
                    worksheet.Cells[row, 3].Value = diem.User?.HoTen;
                    worksheet.Cells[row, 4].Value = diem.User?.LopSH?.Nganh?.Khoa?.TenKhoa;
                    worksheet.Cells[row, 5].Value = diem.User?.LopSH?.Nganh?.TenNganh;
                    worksheet.Cells[row, 6].Value = diem.User?.LopSH?.TenLopSH;
                    
                    // Format the scores
                    if (diem.DiemHocTap.HasValue)
                    {
                        worksheet.Cells[row, 7].Value = diem.DiemHocTap.Value;
                        worksheet.Cells[row, 7].Style.Numberformat.Format = "0.00";
                    }
                    else
                    {
                        worksheet.Cells[row, 7].Value = "Chưa có";
                    }
                    
                    if (diem.DiemRenLuyen.HasValue)
                    {
                        worksheet.Cells[row, 8].Value = diem.DiemRenLuyen.Value;
                        worksheet.Cells[row, 8].Style.Numberformat.Format = "0.00";
                    }
                    else
                    {
                        worksheet.Cells[row, 8].Value = "Chưa có";
                    }
                    
                    row++;
                }
                
                // Auto-fit columns
                worksheet.Cells.AutoFitColumns();
                
                // Set column styles
                worksheet.Column(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; // STT
                worksheet.Column(7).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; // Điểm học tập
                worksheet.Column(8).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; // Điểm rèn luyện
                
                // Apply a banded row style for readability
                for (int i = 2; i < row; i++)
                {
                    if (i % 2 == 0)
                    {
                        using (var range = worksheet.Cells[i, 1, i, 8])
                        {
                            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(240, 240, 240));
                        }
                    }
                }
                
                // Generate a file name with timestamp
                string fileName = $"DanhSachDiemHocTap_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
                
                // Convert to bytes
                var content = package.GetAsByteArray();
                
                // Return the file
                return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
        }

        private bool DiemExists(string id)
        {
            return _context.Diems.Any(e => e.ID == id);
        }
    }
}
