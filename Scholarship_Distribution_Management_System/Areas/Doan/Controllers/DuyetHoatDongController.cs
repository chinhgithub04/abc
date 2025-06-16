using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scholarship_Distribution_Management_System.Models.Entities;
using Scholarship_Distribution_Management_System.Models.ViewModel;
using OfficeOpenXml;
using System.IO;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

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
            // Get all activities for applications in this scholarship period
            var activities = await (
                from hdsv in _context.HoatDongCuaSinhViens
                join hoatDong in _context.HoatDongs on hdsv.IDHoatDong equals hoatDong.ID
                join don in _context.DonXinHocBongs on hdsv.IDDon equals don.ID
                where don.IDDot == idDot &&
                      don.TrangThai == 1
                select new 
                {
                    IDDon = hdsv.IDDon,
                    IDHoatDong = hdsv.IDHoatDong,
                    IDSinhVien = don.IDSinhVien,
                    TenHoatDong = hoatDong.TenHoatDong,
                    IsDuyetHoatDong = don.isDuyetHoatDong
                }
            ).ToListAsync();

            // Group activities by IDDon and IDSinhVien
            var groupedActivities = activities
                .GroupBy(a => new { a.IDDon, a.IDSinhVien })
                .Select(group => {
                    // Get all activity IDs for this application
                    var activityIds = group.Select(a => a.IDHoatDong).ToList();
                    
                    // Get approved activity IDs from KQHoatDong
                    var approvedActivityIds = _context.KqHoatDong
                        .Where(k => k.IDDon == group.Key.IDDon && k.IDSinhVien == group.Key.IDSinhVien)
                        .Select(k => k.IDHoatDong)
                        .ToList();
                    
                    // Check if application has already been reviewed
                    var application = _context.DonXinHocBongs
                        .FirstOrDefault(d => d.ID == group.Key.IDDon);

                    return new HoatDongCuaSinhVienViewModel
                    {
                        IDDon = group.Key.IDDon,
                        IDSinhVien = group.Key.IDSinhVien,
                        TenHoatDong = string.Join(", ", group.Select(a => a.TenHoatDong)),
                        DaDuyet = application.isDuyetHoatDong,
                        AllActivitiesApproved = activityIds.Count > 0 && activityIds.All(id => approvedActivityIds.Contains(id)),
                        AnyActivityApproved = approvedActivityIds.Any(),
                        ActivityNames = group.Select(a => a.TenHoatDong).ToList()
                    };
                })                .ToList();

            // Get the scholarship period details for the view title
            var dotHocBong = await _context.DotHocBongs.FindAsync(idDot);
            if (dotHocBong != null)
            {
                ViewBag.ScholarshipName = dotHocBong.TenDot;
            }
            
            ViewBag.IdDot = idDot; // Pass the scholarship ID to the view for form submission

            return View(groupedActivities);
        }
        
        [HttpPost]
        public async Task<IActionResult> ImportFromExcel(string idDot, List<IFormFile> files, int maSVCol, int hoatDongCol)
        {
            if (files == null || !files.Any())
            {
                TempData["ErrorMessage"] = "Vui lòng chọn ít nhất một file Excel để tải lên.";
                return RedirectToAction("Index", new { idDot });
            }
            
            // Track statistics for user feedback
            int totalProcessed = 0;
            int totalApproved = 0;
            int totalErrors = 0;
            
            // Dictionary to cache activity IDs by name to improve performance
            Dictionary<string, string> activityIdCache = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            
            // Set the appropriate EPPlus license context for non-commercial use
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            
            foreach (var file in files)
            {
                if (file.Length == 0)
                    continue;
                
                try
                {
                    using (var stream = new MemoryStream())
                    {
                        await file.CopyToAsync(stream);
                        using (var package = new ExcelPackage(stream))
                        {
                            // Process each worksheet in the Excel file
                            foreach (var worksheet in package.Workbook.Worksheets)
                            {
                                // Find the last row with data
                                int rowCount = worksheet.Dimension?.Rows ?? 0;
                                
                                // Skip header row, start from row 2
                                for (int row = 2; row <= rowCount; row++)
                                {
                                    // Read student ID and activity name from the configured columns
                                    string studentId = worksheet.Cells[row, maSVCol].Text?.Trim();
                                    string activityName = worksheet.Cells[row, hoatDongCol].Text?.Trim();
                                    
                                    // Skip empty rows
                                    if (string.IsNullOrEmpty(studentId) || string.IsNullOrEmpty(activityName))
                                        continue;
                                    
                                    totalProcessed++;
                                    
                                    try
                                    {
                                        // Find the activity ID by name (using cache for performance)
                                        string activityId;
                                        
                                        if (!activityIdCache.TryGetValue(activityName, out activityId))
                                        {
                                            var activity = await _context.HoatDongs
                                                .FirstOrDefaultAsync(h => h.TenHoatDong == activityName);
                                                
                                            if (activity == null)
                                            {
                                                totalErrors++;
                                                continue; // Skip if activity not found
                                            }
                                            
                                            activityId = activity.ID;
                                            activityIdCache[activityName] = activityId;
                                        }
                                        
                                        // Find the application for this student in the current scholarship period
                                        var application = await _context.DonXinHocBongs
                                            .FirstOrDefaultAsync(d => d.IDSinhVien == studentId && 
                                                                     d.IDDot == idDot && 
                                                                     d.TrangThai == 1);
                                        
                                        if (application == null)
                                        {
                                            totalErrors++;
                                            continue; // Skip if application not found
                                        }
                                        
                                        // Check if the student has registered for this activity
                                        var registeredActivity = await _context.HoatDongCuaSinhViens
                                            .AnyAsync(h => h.IDDon == application.ID && h.IDHoatDong == activityId);
                                            
                                        if (!registeredActivity)
                                        {
                                            totalErrors++;
                                            continue; // Skip if student hasn't registered for this activity
                                        }
                                        
                                        // Check if this activity approval already exists
                                        var existingApproval = await _context.KqHoatDong
                                            .AnyAsync(k => k.IDSinhVien == studentId && 
                                                         k.IDHoatDong == activityId && 
                                                         k.IDDon == application.ID);
                                                         
                                        if (existingApproval)
                                            continue; // Skip if already approved
                                        
                                        // Create new activity approval record
                                        var kqHoatDong = new KQHoatDong
                                        {
                                            ID = Guid.NewGuid().ToString(),
                                            IDDon = application.ID,
                                            IDSinhVien = studentId,
                                            IDHoatDong = activityId
                                        };
                                        
                                        _context.KqHoatDong.Add(kqHoatDong);
                                        
                                        // Mark the application as having approved activities
                                        if (!application.isDuyetHoatDong)
                                        {
                                            application.isDuyetHoatDong = true;
                                            _context.DonXinHocBongs.Update(application);
                                        }
                                        
                                        totalApproved++;
                                    }
                                    catch (Exception)
                                    {
                                        totalErrors++;
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"Lỗi khi xử lý file: {ex.Message}";
                    return RedirectToAction("Index", new { idDot });
                }
            }
            
            // Save all changes to the database
            await _context.SaveChangesAsync();
            
            // Prepare success message with statistics
            TempData["SuccessMessage"] = $"Đã xử lý {totalProcessed} hoạt động từ các file Excel. " +
                                        $"Đã duyệt thành công {totalApproved} hoạt động. " +
                                        (totalErrors > 0 ? $"Có {totalErrors} lỗi xảy ra." : "");
                                        
            return RedirectToAction("Index", new { idDot });
        }
    }
}
