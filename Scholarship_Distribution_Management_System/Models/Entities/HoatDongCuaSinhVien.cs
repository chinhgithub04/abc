using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Scholarship_Distribution_Management_System.Models.Entities
{
    public class HoatDongCuaSinhVien
    {
        public string IDDon { get; set; }

        public string IDHoatDong { get; set; }

        [ForeignKey("IDDon")]
        public DonXinHocBong DonXinHocBong { get; set; }

        [ForeignKey("IDHoatDong")]
        public HoatDong HoatDong { get; set; }
    }

}
