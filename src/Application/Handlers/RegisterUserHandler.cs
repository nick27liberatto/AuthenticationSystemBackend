namespace Application.Handlers
{
    using Application.Commands;
    using Application.Dtos.Response;
    using AutoMapper;
    using Domain.Interfaces;
    using Domain.Models;
    using MediatR;
    using Microsoft.AspNetCore.Identity;
    using System.Threading;
    using System.Threading.Tasks;

    public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, UserResponseDto>
    {
        private readonly IRepository<User> _repository;
        private readonly IMapper _mapper;
        private readonly PasswordHasher<User> _passwordHasher;

        public RegisterUserHandler(IRepository<User> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
            _passwordHasher = new PasswordHasher<User>();
        }

        public async Task<UserResponseDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var user = _mapper.Map<User>(request);
            
            user.Password = _passwordHasher.HashPassword(user, request.Password);

            await _repository.AddAsync(user);
            
            return _mapper.Map<UserResponseDto>(user);
        }
    }
}
