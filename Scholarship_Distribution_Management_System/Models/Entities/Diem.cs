using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Scholarship_Distribution_Management_System.Models.Entities
{
    public class Diem
    {
        [Key]
        public string ID { get; set; }

        public float? DiemHocTap { get; set; }
        public float? DiemRenLuyen { get; set; }

        [ForeignKey("ID")]
        public ApplicationUser User { get; set; }
    }
}