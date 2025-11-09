namespace Domain.Models
{
    using Microsoft.AspNetCore.Identity;

    public class User : IdentityUser<int>
    {
        public string? FullName { get; set; }
        public bool IsActive { get; set; } = true;
        public ICollection<RefreshToken>? RefreshTokens { get; set; }
    }
}
