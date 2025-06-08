using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Scholarship_Distribution_Management_System.Models.Entities
{
    public class DonXinHocBong
    {
        [Key]
        public string? ID { get; set; }
        public ICollection<HoatDongCuaSinhVien>? HoatDongCuaSinhViens { get; set; }
        public ICollection<NghienCuuCuaSinhVien>? NghienCuuCuaSinhViens { get; set; }

        public required string IDSinhVien { get; set; }
        public required string IDDot { get; set; }

        public required DateTime NgayNop { get; set; }
        public int? TrangThai { get; set; }  // Changed from string
        public string? NoiDung { get; set; }
        public float? DiemHocTap { get; set; }
        public float? DiemRenLuyen { get; set; }
        public bool? DuyetHoiDong { get; set; }
        public bool? DuyetCapPhatHocBong { get; set; }
        public float? KQDiemRL { get; set; }
        public float? KQDiemHT { get; set; }

        [ForeignKey("IDSinhVien")]
        public ApplicationUser? SinhVien { get; set; }

        [ForeignKey("IDDot")]
        public DotHocBong? DotHocBong { get; set; }
        
    }

}
