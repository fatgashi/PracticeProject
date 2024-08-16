using System.ComponentModel.DataAnnotations;

namespace PracticeProject.DTOs
{
    public class ClientModel
    {
        public Guid ClientId { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "Username cannot be longer than 20 characters")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
