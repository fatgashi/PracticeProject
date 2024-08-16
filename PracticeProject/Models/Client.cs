using System.ComponentModel.DataAnnotations;

namespace PracticeProject.Models
{
    public class Client
    {
        [Key]
        public Guid ClientId { get; set; }
        public virtual User User { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
