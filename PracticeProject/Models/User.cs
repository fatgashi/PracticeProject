using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace PracticeProject.Models
{
    public class User : IdentityUser<Guid>
    {
        public virtual Client Client { get; set; }

        public virtual Admin Admin {  get; set; }
    }
}
