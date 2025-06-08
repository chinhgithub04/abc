using System.ComponentModel.DataAnnotations;

namespace Scholarship_Distribution_Management_System.Models.ViewModel
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập email.")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu.")]
        public string? Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
