using System.ComponentModel.DataAnnotations;

namespace PracticeProject.Models
{
    public class Admin
    {
        [Key]
        public Guid AdminId { get; set; }
        public virtual User User { get; set; }
    }
}
