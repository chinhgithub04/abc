using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Scholarship_Distribution_Management_System.Models.Entities
{
    public class NghienCuuCuaSinhVien
    {
        public string IDDon { get; set; }

        public string IDNghienCuu { get; set; }

        [ForeignKey("IDDon")]
        public DonXinHocBong DonXinHocBong { get; set; }

        [ForeignKey("IDNghienCuu")]
        public NghienCuuKhoaHoc NghienCuu { get; set; }
    }

}
