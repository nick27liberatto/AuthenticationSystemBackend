namespace Application.DTOs
{
    public record RegisterUserDto(
        string? FullName,
        string Username,
        string Email,
        string Password
    );

    public record LoginUserDto(
        string Email,
        string Password
    );

    public record ForgotPasswordDto(
        string Email
    );

    public record ResetPasswordDto(
        string Email,
        string Token,
        string NewPassword
    );
    public record AuthResultDto
    {
        public string Token { get; init; } = string.Empty;
        public string Username { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
    };

    public record SocialLoginDto(
        string Provider, 
        string Token
    );
}
