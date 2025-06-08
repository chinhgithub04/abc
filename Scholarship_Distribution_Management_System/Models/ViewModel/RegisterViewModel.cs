namespace Scholarship_Distribution_Management_System.Models.ViewModel
{
    using System.ComponentModel.DataAnnotations;

    public class RegisterViewModel
    {
        [EmailAddress]
        public required string Email { get; set; }

        public required string HoTen { get; set; }

        [DataType(DataType.Password)]
        public required string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Mật khẩu không khớp.")]
        public required string ConfirmPassword { get; set; }

        public required string Role { get; set; } // Nếu bạn chọn vai trò trong form
    }


}
