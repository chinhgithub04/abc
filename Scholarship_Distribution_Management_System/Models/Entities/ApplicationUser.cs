namespace Scholarship_Distribution_Management_System.Models.Entities
{
    using Microsoft.AspNetCore.Identity;

    public class ApplicationUser : IdentityUser
    {
        // Bổ sung các thông tin từ bảng NguoiDung
        public string? HoTen { get; set; }
        public string? GioiTinh { get; set; }
        public DateTime? NgaySinh { get; set; }
        public string? DiaChi { get; set; }
        public string? IDLopSH { get; set; }
        public int? TrangThai { get; set; }

        // Điều hướng
        public LopSH? LopSH { get; set; }
    }

}
