namespace Application.Handlers
{
    using Application.Commands;
    using Application.Constants.Response;
    using Domain.Models;
    using FluentResults;
    using FluentValidation;
    using MediatR;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Logging;
    using System.Threading;
    using System.Threading.Tasks;

    public class ResetPasswordHandler : IRequestHandler<ResetPasswordCommand, Result>
    {
        private readonly UserManager<User> _userManager;
        private readonly IValidator<ResetPasswordCommand> _validator;
        private readonly ILogger<ResetPasswordHandler> _logger;

        public ResetPasswordHandler(UserManager<User> userManager, 
            IValidator<ResetPasswordCommand> validator, 
            ILogger<ResetPasswordHandler> logger)
        {
            _userManager = userManager;
            _validator = validator;
            _logger = logger;
        }

        public async Task<Result> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var validation = await _validator.ValidateAsync(request, cancellationToken);
            if (!validation.IsValid)
                return Result.Fail(MessageError.ResetPasswordValidationFailed)
                             .WithErrors(validation.Errors.Select(e => e.ErrorMessage));

            try
            {
                var user = await _userManager.FindByEmailAsync(request.Dto.Email);
                if (user == null || !user.IsActive)
                    return Result.Fail(MessageError.UserNotFound);

                var result = await _userManager
                    .ResetPasswordAsync(user, request.Dto.Token, request.Dto.NewPassword);

                if (!result.Succeeded)
                    return Result.Fail(MessageError.ResetPasswordFailed)
                        .WithErrors(result.Errors.Select(e => e.Description));

                return Result.Ok().WithSuccess(MessageSuccess.PasswordResetSuccessful);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, MessageError.ResetPasswordFailedUser);
                return Result.Fail(MessageError.ResetPasswordFailed);
            }
            
        }
    }
}
