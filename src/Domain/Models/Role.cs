using Microsoft.AspNetCore.Identity;

namespace Domain.Models
{
    public class Role : IdentityRole<int>
    {
        public string? Description { get; set; }
    }
}
