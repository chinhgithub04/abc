namespace Scholarship_Distribution_Management_System.Models.ViewModel
{
    public class InfoHocBongViewModel
    {
        // Thông tin sinh viên
        public string? SinhVienId { get; set; }
        public string? HoTen { get; set; }
        public string? GioiTinh { get; set; }
        public DateTime? NgaySinh { get; set; }
        public string? TenLopSH { get; set; }
        public string? TenNganh { get; set; }
        public string? TenKhoa { get; set; }
        public string? Email { get; set; }

        // Thông tin đơn xin học bổng
        public string? DonHocBongId { get; set; }
        public DateTime? NgayNop { get; set; }
        public int? TrangThaiDon { get; set; }
        public double? DiemHocTap { get; set; }
        public double? DiemRenLuyen { get; set; }

        public double? KQDiemHT { get; set; }
        public double? KQDiemRL { get; set; }
        // Thông tin đợt học bổng
        public string? DotHocBongId { get; set; }
        public string? TenDot { get; set; }
        public int? TrangThaiDot { get; set; }
        public DateTime? NgayTao { get; set; }
        public DateTime? NgayBatDauNop { get; set; }
        public DateTime? NgayKetThucNop { get; set; }
        public DateTime? NgayHoiDongDuyet { get; set; }
        public DateTime? NgayPTCDuyet { get; set; }
        public DateTime? NgayKetThuc { get; set; }
    }

}
