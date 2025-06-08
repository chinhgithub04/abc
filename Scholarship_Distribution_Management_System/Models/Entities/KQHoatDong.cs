using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Scholarship_Distribution_Management_System.Models.Entities
{
    public class KQHoatDong
    {
        [Key]
        public string ID { get; set; }

        public string IDDon { get; set; }
        public string IDSinhVien { get; set; }
        public string IDHoatDong { get; set; }

        [ForeignKey("IDDon")]
        public DonXinHocBong Don { get; set; }

        [ForeignKey("IDSinhVien")]
        public ApplicationUser SinhVien { get; set; }

        [ForeignKey("IDHoatDong")]
        public HoatDong HoatDong { get; set; }
    }

}
