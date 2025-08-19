namespace Application.Dtos.Request
{
    public class LoginUserRequestDto
    {
        public required string Username { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
