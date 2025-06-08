using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Scholarship_Distribution_Management_System.Models.Entities
{
    public class KQNghienCuu
    {
        [Key]
        public string ID { get; set; }

        public string IDDon { get; set; }
        public string IDSinhVien { get; set; }
        public string IDNghienCuu { get; set; }

        [ForeignKey("IDDon")]
        public DonXinHocBong Don { get; set; }

        [ForeignKey("IDSinhVien")]
        public ApplicationUser SinhVien { get; set; }

        [ForeignKey("IDNghienCuu")]
        public NghienCuuKhoaHoc NghienCuu { get; set; }
    }

}
