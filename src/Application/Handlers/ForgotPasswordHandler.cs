namespace Application.Handlers
{
    using Application.Commands;
    using Application.Constants;
    using Application.Constants.Response;
    using Application.Interfaces;
    using Domain.Models;
    using FluentResults;
    using FluentValidation;
    using MediatR;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    public class ForgotPasswordHandler : IRequestHandler<ForgotPasswordCommand, Result>
    {
        private readonly UserManager<User> _userManager;
        private readonly IEmailService _emailSender;
        private readonly IValidator<ForgotPasswordCommand> _validator;
        private readonly ILogger<ForgotPasswordHandler> _logger;
        private readonly IFrontendSettings _frontendSettings;

        public ForgotPasswordHandler(UserManager<User> userManager, 
            IEmailService emailSender, 
            IValidator<ForgotPasswordCommand> validator,
            ILogger<ForgotPasswordHandler> logger,
            IFrontendSettings frontendSettings)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _validator = validator;
            _logger = logger;
            _frontendSettings = frontendSettings;
        }

        public async Task<Result> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            var validation = await _validator.ValidateAsync(request, cancellationToken);
            if (!validation.IsValid)
                return Result.Fail(MessageError.ForgotPasswordValidationFailed)
                             .WithErrors(validation.Errors.Select(e => e.ErrorMessage));

            try
            {
                var user = await _userManager.FindByEmailAsync(request.Dto.Email);
                if (user == null || !user.IsActive)
                    return Result.Ok().WithSuccess(MessageSuccess.PasswordResetEmailSent);

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var resetLink = $"{_frontendSettings.BaseUrl}/reset-password?email={user.Email}&token={Uri.EscapeDataString(token)}";

                try
                {
                    await _emailSender.SendEmailAsync(
                        user.Email!,
                        "Reset Password",
                        $"<p>Hello {user.UserName},</p><p>To reset your password, click in the link below:</p><p><a href='{resetLink}'>Reset Password</a></p>"
                    );
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, MessageError.EmailSendFailure);
                }

                return Result.Ok().WithSuccess(MessageSuccess.PasswordResetEmailSent);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, MessageError.ForgotPasswordFailedUser);
                return Result.Fail(MessageError.ForgotPasswordFailed);
            }
        }
    }
}
