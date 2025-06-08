using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Scholarship_Distribution_Management_System.Models.Entities
{
    public class Nganh
    {
        [Key]
        public string ID { get; set; }

        public string IDKhoa { get; set; }
        public string TenNganh { get; set; }

        [ForeignKey("IDKhoa")]
        public Khoa Khoa { get; set; }
    }
}
