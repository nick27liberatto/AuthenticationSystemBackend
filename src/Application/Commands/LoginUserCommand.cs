namespace Application.Commands
{
    using Application.Dtos.Response;
    using MediatR;

    public class LoginUserCommand : IRequest<UserDto>
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
