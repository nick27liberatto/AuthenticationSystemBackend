namespace Application.Handlers
{
    using Application.Commands;
    using Application.Dtos.Response;
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;

    public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, UserDto>
    {
        public Task<UserDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
