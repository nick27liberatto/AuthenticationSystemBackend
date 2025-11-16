namespace Application.Interfaces
{
    using Application.DTOs;
    using Microsoft.AspNetCore.Http;

    public interface IExternalAuthService
    {
        Task<string> GetAuthorizationUrlAsync(string provider, HttpContext httpContext);
        Task<ExternalUserDto> ExchangeCodeForUserInfoAsync(string provider, string code, string state, HttpContext httpContext);
        Task<string> HandleExternalLoginAsync(ExternalUserDto info);

    }

}
