using System.ComponentModel.DataAnnotations;

namespace Scholarship_Distribution_Management_System.Models.ViewModel
{
    public class TaiKhoanDayDuViewModel
    {
        // Thông tin định danh
        public string ID { get; set; }

        // Thông tin cá nhân
        public string HoTen { get; set; }
        public DateTime? NgaySinh { get; set; }
        public string GioiTinh { get; set; }
        public string DiaChi { get; set; }

        // Thông tin liên hệ
        public string Email { get; set; }
        public string SDT { get; set; }

        // Trạng thái (hoạt động, bị khóa, ...)
        public int TrangThai { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
        // Identity
        public string ?UserName { get; set; }
        public string ?PasswordHash { get; set; }

        // Vai trò (1 tài khoản chỉ có 1 role)
        public string VaiTro { get; set; }

        // Thông tin sinh viên (nếu có)
        public string? TenLopSH { get; set; }
        public string? TenNganh { get; set; }
        public string? TenKhoa { get; set; }
    }

}
