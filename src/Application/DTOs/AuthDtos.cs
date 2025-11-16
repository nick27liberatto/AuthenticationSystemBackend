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

    public class ExternalUserDto
    {
        public string Email { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Provider { get; set; } = null!;
        public string ProviderId { get; set; } = null!;
        public string Picture { get; set; } = null!;
        public string AccessToken { get; set; } = null!;
        public string IdToken { get; set; } = null!;
    };

    public class ExternalAuthResultDto
    {
        public bool Success { get; set; }
        public string AccessToken { get; set; } = string.Empty;
        public string Error { get; set; } = string.Empty;
    }
}
