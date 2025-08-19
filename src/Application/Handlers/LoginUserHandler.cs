namespace Application.Handlers
{
    using Application.Dtos.Response;
    using Application.Queries;
    using AutoMapper;
    using Domain.Interfaces;
    using Domain.Models;
    using MediatR;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading;
    using System.Threading.Tasks;

    public class LoginUserHandler : IRequestHandler<LoginUserQuery, ActionResult<UserResponseDto>>
    {
        private readonly IRepository<User> _repository;
        private readonly IMapper _mapper;
        private readonly PasswordHasher<User> _passwordHasher;

        public LoginUserHandler(IRepository<User> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
            _passwordHasher = new PasswordHasher<User>();
        }

        public async Task<ActionResult<UserResponseDto>> Handle(LoginUserQuery request, CancellationToken cancellationToken)
        {
            var users = await _repository.GetAllAsync();
            
            var user = users.FirstOrDefault(x => x.Email == request.Email);

            if (user == null)
            {
                return new NotFoundResult();
            }

            var autenticacao = _passwordHasher.VerifyHashedPassword(user, user.Password, request.Password);

            if (autenticacao == PasswordVerificationResult.Failed)
            {
                return new UnauthorizedResult();
            }

            return new OkObjectResult(_mapper.Map<UserResponseDto>(user)); 
        }
    }
}
