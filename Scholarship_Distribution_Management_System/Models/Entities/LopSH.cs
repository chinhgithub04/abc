using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Scholarship_Distribution_Management_System.Models.Entities
{
    public class LopSH
    {
        [Key]
        public string ID { get; set; }

        public string IDNganh { get; set; }
        public string TenLopSH { get; set; }

        [ForeignKey("IDNganh")]
        public Nganh Nganh { get; set; }
    }
}
