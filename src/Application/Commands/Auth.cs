namespace Application.Commands
{
    using Application.DTOs;
    using FluentResults;
    using MediatR;

    public record RegisterUserCommand(RegisterUserDto Dto) : IRequest<Result<AuthResultDto>>;
    public record LoginUserCommand(LoginUserDto Dto) : IRequest<Result<AuthResultDto>>;
    public record ForgotPasswordCommand(ForgotPasswordDto Dto) : IRequest<Result>;
    public record ResetPasswordCommand(ResetPasswordDto Dto) : IRequest<Result>;
    public record SocialLoginCommand(SocialLoginDto Dto) : IRequest<Result<AuthResultDto>>;
}
