using System.ComponentModel.DataAnnotations;

namespace Scholarship_Distribution_Management_System.Models.Entities
{
    public class Khoa
    {
        [Key]
        public string ID { get; set; }

        public string TenKhoa { get; set; }
    }
}
