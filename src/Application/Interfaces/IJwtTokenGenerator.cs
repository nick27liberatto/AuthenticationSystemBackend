namespace Application.Interfaces
{
    using Domain.Models;

    public interface IJwtTokenGenerator
    {
        string GenerateToken(User user);
    }
}
