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
            // Get all applications for this scholarship period
            var applications = await _context.DonXinHocBongs
                .Where(d => d.IDDot == idDot && d.TrangThai == 1)
                .Select(d => new {
                    IDDon = d.ID,
                    IDSinhVien = d.IDSinhVien,
                    IsDuyetHoatDong = d.isDuyetHoatDong
                })
                .ToListAsync();

            // Get all activities for these applications
            var activities = await (
                from hdsv in _context.HoatDongCuaSinhViens
                join hoatDong in _context.HoatDongs on hdsv.IDHoatDong equals hoatDong.ID
                join don in _context.DonXinHocBongs on hdsv.IDDon equals don.ID
                where don.IDDot == idDot && don.TrangThai == 1
                select new
                {
                    IDDon = hdsv.IDDon,
                    IDHoatDong = hdsv.IDHoatDong,
                    IDSinhVien = don.IDSinhVien,
                    TenHoatDong = hoatDong.TenHoatDong
                }
            ).ToListAsync();

            // Get all approved activities
            var approvedActivities = await (
                from kq in _context.KqHoatDong
                join don in _context.DonXinHocBongs on kq.IDDon equals don.ID
                where don.IDDot == idDot
                select new
                {
                    IDDon = kq.IDDon,
                    IDSinhVien = kq.IDSinhVien,
                    IDHoatDong = kq.IDHoatDong
                }
            ).ToListAsync();

            // Create view models for all applications, including those without activities
            var groupedActivities = new List<HoatDongCuaSinhVienViewModel>();

            foreach (var app in applications)
            {
                // Get activities for this application
                var appActivities = activities
                    .Where(a => a.IDDon == app.IDDon)
                    .ToList();

                // Get approved activity IDs for this application
                var approvedIds = approvedActivities
                    .Where(a => a.IDDon == app.IDDon && a.IDSinhVien == app.IDSinhVien)
                    .Select(a => a.IDHoatDong)
                    .ToList();

                // Get activity names and IDs
                var activityNames = appActivities.Select(a => a.TenHoatDong).ToList();
                var activityIds = appActivities.Select(a => a.IDHoatDong).ToList();                // Create view model
                var viewModel = new HoatDongCuaSinhVienViewModel
                {
                    IDDon = app.IDDon,
                    IDSinhVien = app.IDSinhVien,
                    TenHoatDong = string.Join(", ", activityNames),
                    DaDuyet = app.IsDuyetHoatDong,
                    AllActivitiesApproved = activityIds.Count == 0 || activityIds.All(id => approvedIds.Contains(id)),
                    AnyActivityApproved = approvedIds.Any(),
                    ActivityNames = activityNames
                };

                groupedActivities.Add(viewModel);
            }

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
            
            // Find and approve applications without any activities
            var applicationsWithoutActivities = await _context.DonXinHocBongs
                .Where(d => d.IDDot == idDot && 
                       d.TrangThai == 1 && 
                       !d.isDuyetHoatDong &&
                       !_context.HoatDongCuaSinhViens.Any(h => h.IDDon == d.ID))
                .ToListAsync();
                
            int noActivityApprovals = 0;
            
            foreach (var app in applicationsWithoutActivities)
            {
                app.isDuyetHoatDong = true;
                _context.DonXinHocBongs.Update(app);
                noActivityApprovals++;
            }
            
            if (noActivityApprovals > 0)
            {
                await _context.SaveChangesAsync();
                totalApproved += noActivityApprovals;
            }
            
            // Prepare success message with statistics
            TempData["SuccessMessage"] = $"Đã xử lý {totalProcessed} hoạt động từ các file Excel. " +
                                        $"Đã duyệt thành công {totalApproved} hoạt động" + 
                                        (noActivityApprovals > 0 ? $" (bao gồm {noActivityApprovals} sinh viên không có hoạt động)" : "");
                                        
            return RedirectToAction("Index", new { idDot });
        }

        public async Task<IActionResult> Details(string idDon, string idSinhVien)
        {
            if (string.IsNullOrEmpty(idDon) || string.IsNullOrEmpty(idSinhVien))
            {
                return NotFound();
            }
            
            // Get the student's application
            var application = await _context.DonXinHocBongs
                .Include(d => d.SinhVien)
                .Include(d => d.DotHocBong)
                .FirstOrDefaultAsync(d => d.ID == idDon && d.IDSinhVien == idSinhVien);
                
            if (application == null)
            {
                return NotFound();
            }
            
            // Get all activities registered by the student for this application
            var registeredActivities = await (
                from hdsv in _context.HoatDongCuaSinhViens
                join hoatDong in _context.HoatDongs on hdsv.IDHoatDong equals hoatDong.ID
                where hdsv.IDDon == idDon
                select new ActivityItem
                {
                    ID = hoatDong.ID,
                    Name = hoatDong.TenHoatDong
                }
            ).ToListAsync();
            
            // Get all approved activities for this application
            var approvedActivities = await (
                from kq in _context.KqHoatDong
                join hoatDong in _context.HoatDongs on kq.IDHoatDong equals hoatDong.ID
                where kq.IDDon == idDon && kq.IDSinhVien == idSinhVien
                select new ActivityItem
                {
                    ID = hoatDong.ID,
                    Name = hoatDong.TenHoatDong
                }
            ).ToListAsync();
            
            // Calculate pending activities (registered but not approved)
            var approvedIds = approvedActivities.Select(a => a.ID).ToHashSet();
            var pendingActivities = registeredActivities
                .Where(a => !approvedIds.Contains(a.ID))
                .ToList();
            
            // Get student information
            var student = await _context.Users.FindAsync(idSinhVien);
            var studentClass = await _context.LopSHs.FirstOrDefaultAsync(l => l.ID == student.IDLopSH);
              // Create the view model
            var viewModel = new ActivityDetailViewModel
            {
                IDDon = idDon,
                IDSinhVien = idSinhVien,
                StudentName = student?.HoTen ?? "Unknown",
                StudentClass = studentClass?.TenLopSH ?? "Unknown",
                ScholarshipName = application.DotHocBong?.TenDot ?? "Unknown",
                RegisteredActivities = registeredActivities,
                ApprovedActivities = approvedActivities,
                PendingActivities = pendingActivities,
                IsDuyetHoatDong = application.isDuyetHoatDong
            };
            
            // Pass the scholarship ID for the back button
            ViewBag.IdDot = application.IDDot;

            return View(viewModel);
        }
    }
}
