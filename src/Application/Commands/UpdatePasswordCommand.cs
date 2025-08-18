namespace Application.Commands
{
    using Application.Dtos.Response;
    using MediatR;

    public class UpdatePasswordCommand : IRequest<UserDto>
    {
        public int Id { get; set; }
        public required string OldPassword { get; set; }
        public required string NewPassword { get; set; }
    }
}
