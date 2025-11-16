namespace Infrastructure.Services
{
    using Application.DTOs;
    using Application.Interfaces;
    using Domain.Models;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Configuration;
    using System.Net.Http.Headers;
    using System.Text.Json;

    public class ExternalAuthService : IExternalAuthService
    {
        private readonly IConfiguration _config;
        private readonly UserManager<User> _userManager;
        private readonly IJwtTokenGenerator _jwt;

        public ExternalAuthService(
            IConfiguration config,
            UserManager<User> userManager,
            IJwtTokenGenerator jwt)
        {
            _config = config;
            _userManager = userManager;
            _jwt = jwt;
        }

        public Task<string> GetAuthorizationUrlAsync(string provider, HttpContext httpContext)
        {
            provider = provider.ToLower();

            var redirectUri = $"{_config["OAuth:RedirectUri"]}?provider={provider}";

            return provider switch
            {
                "google" => Task.FromResult(BuildGoogleAuthUrl(redirectUri)),
                "github" => Task.FromResult(BuildGitHubAuthUrl(redirectUri)),
                _ => throw new Exception("Provedor não suportado")
            };
        }

        private string BuildGoogleAuthUrl(string redirectUri)
        {
            var clientId = _config["OAuth:Google:ClientId"];
            var scope = "openid email profile";

            return
                "https://accounts.google.com/o/oauth2/v2/auth?" +
                $"client_id={clientId}" +
                $"&redirect_uri={Uri.EscapeDataString(redirectUri)}" +
                "&response_type=code" +
                $"&scope={Uri.EscapeDataString(scope)}";
        }

        private string BuildGitHubAuthUrl(string redirectUri)
        {
            var clientId = _config["OAuth:GitHub:ClientId"];

            return
                "https://github.com/login/oauth/authorize?" +
                $"client_id={clientId}" +
                $"&redirect_uri={Uri.EscapeDataString(redirectUri)}" +
                "&scope=user:email";
        }

        public async Task<ExternalUserDto> ExchangeCodeForUserInfoAsync(string provider, string code, string state, HttpContext httpContext)
        {
            provider = provider.ToLower();

            return provider switch
            {
                "google" => await HandleGoogleCallbackAsync(code),
                "github" => await HandleGitHubCallbackAsync(code),
                _ => throw new Exception("Provedor não suportado")
            };
        }

        private async Task<ExternalUserDto> HandleGoogleCallbackAsync(string code)
        {
            var clientId = _config["OAuth:Google:ClientId"];
            var clientSecret = _config["OAuth:Google:ClientSecret"];
            var redirectUri = $"{_config["OAuth:RedirectUri"]}?provider=google";

            using var http = new HttpClient();

            var tokenResponse = await http.PostAsync(
                "https://oauth2.googleapis.com/token",
                new FormUrlEncodedContent(new Dictionary<string, string>
                {
                { "client_id", clientId },
                { "client_secret", clientSecret },
                { "code", code },
                { "redirect_uri", redirectUri },
                { "grant_type", "authorization_code" }
                })
            );

            var tokenJson = JsonDocument.Parse(await tokenResponse.Content.ReadAsStringAsync());
            var accessToken = tokenJson.RootElement.GetProperty("access_token").GetString()!;
            var idToken = tokenJson.RootElement.GetProperty("id_token").GetString()!;

            http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var userInfoJson = JsonDocument.Parse(await http.GetStringAsync("https://www.googleapis.com/oauth2/v3/userinfo"));

            return new ExternalUserDto
            {
                Provider = "google",
                ProviderId = userInfoJson.RootElement.GetProperty("sub").GetString()!,
                Email = userInfoJson.RootElement.GetProperty("email").GetString()!,
                Name = userInfoJson.RootElement.GetProperty("name").GetString()!,
                Picture = userInfoJson.RootElement.GetProperty("picture").GetString()!,
                AccessToken = accessToken,
                IdToken = idToken
            };
        }

        private async Task<ExternalUserDto> HandleGitHubCallbackAsync(string code)
        {
            var clientId = _config["OAuth:GitHub:ClientId"];
            var clientSecret = _config["OAuth:GitHub:ClientSecret"];
            var redirectUri = $"{_config["OAuth:RedirectUri"]}?provider=github";

            using var http = new HttpClient();

            var tokenResponse = await http.PostAsync(
                "https://github.com/login/oauth/access_token",
                new FormUrlEncodedContent(new Dictionary<string, string>
                {
                { "client_id", clientId },
                { "client_secret", clientSecret },
                { "code", code },
                { "redirect_uri", redirectUri }
                })
            );

            var content = await tokenResponse.Content.ReadAsStringAsync();
            var accessToken = content.Split("access_token=")[1].Split("&")[0];

            http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var userJson = JsonDocument.Parse(await http.GetStringAsync("https://api.github.com/user"));
            var emailJson = JsonDocument.Parse(await http.GetStringAsync("https://api.github.com/user/emails"));

            var primaryEmail = emailJson.RootElement.EnumerateArray().First(e => e.GetProperty("primary").GetBoolean())
                .GetProperty("email").GetString()!;

            return new ExternalUserDto
            {
                Provider = "github",
                ProviderId = userJson.RootElement.GetProperty("id").GetInt64().ToString(),
                Name = userJson.RootElement.GetProperty("name").GetString() ?? "",
                Email = primaryEmail,
                Picture = userJson.RootElement.GetProperty("avatar_url").GetString()!,
                AccessToken = accessToken
            };
        }
        public async Task<string> HandleExternalLoginAsync(ExternalUserDto info)
        {
            var user = await _userManager.FindByEmailAsync(info.Email);

            if (user == null)
            {
                user = new User
                {
                    UserName = info.Email,
                    Email = info.Email,
                    FullName = info.Name,
                    ImageUrl = info.Picture
                };

                await _userManager.CreateAsync(user);
            }

            return _jwt.GenerateToken(user);
        }
    }

}
