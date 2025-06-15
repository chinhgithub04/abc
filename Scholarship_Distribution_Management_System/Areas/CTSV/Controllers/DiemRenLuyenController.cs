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

namespace Scholarship_Distribution_Management_System.Areas.CTSV.Controllers
{
    [Area("CTSV")]
    public class DiemRenLuyenController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DiemRenLuyenController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var diemRenLuyenList = await _context.Diems
                .Include(d => d.User)
                    .ThenInclude(u => u.LopSH)
                        .ThenInclude(l => l.Nganh)
                            .ThenInclude(n => n.Khoa)
                .Where(d => d.User != null)
                .ToListAsync();

            ViewBag.Breadcrumbs = new List<BreadcrumbItem>
            {
                new BreadcrumbItem { Title = "Danh sách điểm rèn luyện", IsActive = true }
            };

            return View(diemRenLuyenList);
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
                new BreadcrumbItem { Title = "Danh sách điểm rèn luyện", Url = Url.Action("Index"), IsActive = false },
                new BreadcrumbItem { Title = "Chi tiết điểm của " + diem.User?.HoTen, IsActive = true }
            };

            return View(diem);
        }

        public async Task<IActionResult> ExportToExcel()
        {
            // Get the data
            var diemRenLuyenList = await _context.Diems
                .Include(d => d.User)
                    .ThenInclude(u => u.LopSH)
                        .ThenInclude(l => l.Nganh)
                            .ThenInclude(n => n.Khoa)
                .Where(d => d.User != null)
                .ToListAsync();
            
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Điểm rèn luyện");
                
                // Add header with styling
                worksheet.Cells[1, 1].Value = "STT";
                worksheet.Cells[1, 2].Value = "Mã sinh viên";
                worksheet.Cells[1, 3].Value = "Họ và tên";
                worksheet.Cells[1, 4].Value = "Khoa";
                worksheet.Cells[1, 5].Value = "Ngành";
                worksheet.Cells[1, 6].Value = "Lớp";
                worksheet.Cells[1, 7].Value = "Điểm rèn luyện";
                
                // Style the header
                using (var range = worksheet.Cells[1, 1, 1, 7])
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
                for (int i = 0; i < diemRenLuyenList.Count; i++)
                {
                    var diem = diemRenLuyenList[i];
                    
                    worksheet.Cells[row, 1].Value = i + 1;
                    worksheet.Cells[row, 2].Value = diem.ID;
                    worksheet.Cells[row, 3].Value = diem.User?.HoTen;
                    worksheet.Cells[row, 4].Value = diem.User?.LopSH?.Nganh?.Khoa?.TenKhoa;
                    worksheet.Cells[row, 5].Value = diem.User?.LopSH?.Nganh?.TenNganh;
                    worksheet.Cells[row, 6].Value = diem.User?.LopSH?.TenLopSH;
                      // Format the scores
                    if (diem.DiemRenLuyen.HasValue)
                    {
                        worksheet.Cells[row, 7].Value = (int)diem.DiemRenLuyen.Value;
                        worksheet.Cells[row, 7].Style.Numberformat.Format = "0";
                    }
                    else
                    {
                        worksheet.Cells[row, 7].Value = "Chưa có";
                    }
                    
                    row++;
                }
                
                // Auto-fit columns
                worksheet.Cells.AutoFitColumns();
                
                // Set column styles
                worksheet.Column(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; // STT
                worksheet.Column(7).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; // Điểm rèn luyện
                
                // Apply a banded row style for readability
                for (int i = 2; i < row; i++)
                {
                    if (i % 2 == 0)
                    {
                        using (var range = worksheet.Cells[i, 1, i, 7])
                        {
                            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(240, 240, 240));
                        }
                    }
                }
                
                // Generate a file name with timestamp
                string fileName = $"DanhSachDiemRenLuyen_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
                
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
