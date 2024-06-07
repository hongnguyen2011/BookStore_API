using Microsoft.AspNetCore.Identity;

namespace BookStore_API.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
    }
}
