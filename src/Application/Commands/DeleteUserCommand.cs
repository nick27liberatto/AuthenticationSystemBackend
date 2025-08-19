namespace Application.Commands
{
    using Application.Dtos.Response;
    using MediatR;

    public class DeleteUserCommand : IRequest<UserResponseDto>
    {
        public int Id { get; set; }
    }
}
