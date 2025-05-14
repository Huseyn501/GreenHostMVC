using System.ComponentModel.DataAnnotations;

namespace GreenHost.ViewModels
{
    public class RegisterVm
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Username { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is required")]
        [StringLength(20,MinimumLength =6,ErrorMessage = "Password must be between 6 and 20 characters")]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Confirm Password is required")]
        [Compare("Password", ErrorMessage = "Password and Confirm Password do not match")]
        public string ConfirmPassword { get; set; }

    }
}
