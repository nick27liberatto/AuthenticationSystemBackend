namespace Application.Handlers
{
    using Application.Commands;
    using Application.Constants.Response;
    using Application.DTOs;
    using Application.Interfaces;
    using AutoMapper;
    using Domain.Models;
    using FluentResults;
    using FluentValidation;
    using MediatR;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Logging;
    using System.Threading;
    using System.Threading.Tasks;

    public class LoginUserHandler : IRequestHandler<LoginUserCommand, Result<AuthResultDto>>
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IJwtTokenGenerator _jwtGenerator;
        private readonly IMapper _mapper;
        private readonly IValidator<LoginUserCommand> _validator;
        private readonly ILogger<LoginUserHandler> _logger;

        public LoginUserHandler(UserManager<User> userManager, 
            SignInManager<User> signInManager, 
            IJwtTokenGenerator jwtTokenGenerator, 
            IMapper mapper,
            IValidator<LoginUserCommand> validator,
            ILogger<LoginUserHandler> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtGenerator = jwtTokenGenerator;
            _mapper = mapper;
            _validator = validator;
            _logger = logger;
        }

        public async Task<Result<AuthResultDto>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var validation = await _validator.ValidateAsync(request, cancellationToken);
            if (!validation.IsValid)
                return Result.Fail<AuthResultDto>(MessageError.UserLoggingValidationFailed)
                    .WithErrors(validation.Errors.Select(e => e.ErrorMessage));

            try
            {
                var user = await _userManager.FindByEmailAsync(request.Dto.Email);
                if (user == null || !user.IsActive)
                    return Result.Fail<AuthResultDto>(MessageError.InvalidCredentials);

                var valid = await _signInManager.CheckPasswordSignInAsync(user, request.Dto.Password, false);
                if (!valid.Succeeded)
                    return Result.Fail<AuthResultDto>(MessageError.InvalidCredentials);

                var token = _jwtGenerator.GenerateToken(user);
                var dto = _mapper.Map<AuthResultDto>(user) with { Token = token };
                return Result.Ok(dto).WithSuccess(MessageSuccess.UserLoggedIn);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, MessageError.ErrorLoggingUser, request.Dto.Email);
                return Result.Fail<AuthResultDto>(MessageError.ErrorLoggingIn);
            }
        }
    }
}
