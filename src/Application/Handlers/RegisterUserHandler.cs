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
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;

    public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, Result<AuthResultDto>>
    {
        private readonly UserManager<User> _userManager;
        private readonly IJwtTokenGenerator _jwtGenerator;
        private readonly IMapper _mapper;
        private readonly IValidator<RegisterUserCommand> _validator;
        private readonly ILogger<RegisterUserHandler> _logger;

        public RegisterUserHandler(UserManager<User> userManager, 
            IJwtTokenGenerator jwtTokenGenerator, 
            IMapper mapper, 
            IValidator<RegisterUserCommand> validator,
            ILogger<RegisterUserHandler> logger)
        {
            _userManager = userManager;
            _jwtGenerator = jwtTokenGenerator;
            _mapper = mapper;
            _validator = validator;
            _logger = logger;
        }

        public async Task<Result<AuthResultDto>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var validation = await _validator.ValidateAsync(request, cancellationToken);
            if (!validation.IsValid)
                return Result.Fail<AuthResultDto>(MessageError.UserRegistrationValidationFailed)
                    .WithErrors(validation.Errors.Select(e => e.ErrorMessage));

            if (await _userManager.FindByEmailAsync(request.Dto.Email) != null)
                return Result.Fail<AuthResultDto>(MessageError.EmailAlreadyRegistered);

            var user = _mapper.Map<User>(request.Dto);
            user.IsActive = true;

            IdentityResult result;
            try
            {
                result = await _userManager.CreateAsync(user, request.Dto.Password);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, MessageError.UserRegistrationFailed);
                return Result.Fail<AuthResultDto>(MessageError.UserRegistrationFailed);
            }

            if (!result.Succeeded)
                return Result.Fail<AuthResultDto>(MessageError.UserRegistrationFailed)
                    .WithErrors(result.Errors.Select(e => e.Description));

            var token = _jwtGenerator.GenerateToken(user);
            var dto = _mapper.Map<AuthResultDto>(user) with { Token = token };
            return Result.Ok(dto).WithSuccess(MessageSuccess.UserRegistered);
        }
    }
}
