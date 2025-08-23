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
    using Microsoft.Extensions.Configuration;
    using Microsoft.IdentityModel.Tokens;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    public class LoginUserHandler : IRequestHandler<LoginUserCommand, ActionResult<LoginUserResponseDto>>
    {
        private readonly IRepository<User> _repository;
        private readonly IMapper _mapper;
        private readonly PasswordHasher<User> _passwordHasher;
        private readonly IConfiguration _configuration;

        public LoginUserHandler(IRepository<User> repository, IMapper mapper, IConfiguration configuration)
        {
            _repository = repository;
            _mapper = mapper;
            _passwordHasher = new PasswordHasher<User>();
            _configuration = configuration;
        }

        public async Task<ActionResult<LoginUserResponseDto>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
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

            var key = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("Jwt:Key"));
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
            }),
                Expires = DateTime.UtcNow.AddMinutes(_configuration.GetValue<int>("Jwt:ExpiresInMinutes")),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            var response = _mapper.Map<LoginUserResponseDto>(user);
            response.Token = tokenString;

            return new OkObjectResult(response); 
        }
    }
}
