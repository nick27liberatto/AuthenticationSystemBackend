namespace Application.Commands
{
    using Application.Dtos.Response;
    using MediatR;

    public class DeleteUserCommand : IRequest<UserDto>
    {
        public int Id { get; set; }
    }
}
