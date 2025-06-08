using System.ComponentModel.DataAnnotations;

namespace Scholarship_Distribution_Management_System.Models.Entities
{
    public class HoatDong
    {
        [Key]
        public string ID { get; set; }
        public ICollection<HoatDongCuaSinhVien> HoatDongCuaSinhViens { get; set; }
        public ICollection<KQHoatDong> KQHoatDongs { get; set; }

        public string? TenHoatDong { get; set; }
    }

}
