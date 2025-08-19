namespace Application.Commands
{
    using Application.Dtos.Response;
    using MediatR;

    public class RegisterUserCommand : IRequest<UserResponseDto>
    {
        public required string Username { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
