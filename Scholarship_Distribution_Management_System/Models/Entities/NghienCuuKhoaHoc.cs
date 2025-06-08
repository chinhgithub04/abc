using System.ComponentModel.DataAnnotations;

namespace Scholarship_Distribution_Management_System.Models.Entities
{
    public class NghienCuuKhoaHoc
    {
        [Key]
        public string ID { get; set; }

        public string? TenDeTai { get; set; }
        public string? ThanhTich { get; set; }
        public ICollection<NghienCuuCuaSinhVien> NghienCuuCuaSinhViens { get; set; }
        public ICollection<KQNghienCuu> KqNghienCuus { get; set; }
    }

}
