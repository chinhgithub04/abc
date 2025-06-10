using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Scholarship_Distribution_Management_System.Models.Entities
{
    public class DotHocBong
    {
        [Key]
        public string ID { get; set; }
        public string IDNhanVien { get; set; }
        public string TenDot { get; set; }
        public string DieuKien { get; set; }
        public int SoLuong { get; set; }
        [Precision(18, 2)]
        public long Tien { get; set; }
        public int TrangThai { get; set; }
        public DateTime NgayTao { get; set; }
        public DateTime NgayBatDauNop { get; set; }
        public DateTime NgayKetThucNop { get; set; }
        public DateTime NgayHoiDongDuyet { get; set; }
        public DateTime NgayPTCDuyet { get; set; }
        public DateTime NgayKetThuc { get; set; }

        [ForeignKey("IDNhanVien")]
        public ApplicationUser NhanVien { get; set; }
    }

}
