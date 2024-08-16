using System.ComponentModel.DataAnnotations;

namespace PracticeProject.DTOs
{
    public class UpdateClientDTO
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }
        public string Role { get; set; }
    }
}
