namespace Application.Commands
{
    using Application.Dtos.Response;
    using MediatR;

    public class RecoverPasswordCommand : IRequest<UserResponseDto>
    {
        public int Id { get; set; }
        public required string Email { get; set; }
        public required string NewPassword { get; set; }
    }
}
