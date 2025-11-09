namespace Application.Handlers
{
    using Application.Commands;
    using Application.Constants.Response;
    using Application.DTOs;
    using Application.Interfaces;
    using AutoMapper;
    using Domain.Models;
    using FluentResults;
    using Google.Apis.Auth;
    using MediatR;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Logging;
    using System.Net.Http.Headers;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    public class SocialLoginHandler : IRequestHandler<SocialLoginCommand, Result<AuthResultDto>>
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IJwtTokenGenerator _jwtGenerator;
        private readonly ILogger<SocialLoginHandler> _logger;

        public SocialLoginHandler(UserManager<User> userManager, IMapper mapper, IJwtTokenGenerator jwtGenerator, ILogger<SocialLoginHandler> logger)
        {
            _userManager = userManager;
            _mapper = mapper;
            _jwtGenerator = jwtGenerator;
            _logger = logger;
        }

        public async Task<Result<AuthResultDto>> Handle(SocialLoginCommand request, CancellationToken ct)
        {
            try
            {
                string email = null!;
                string username = null!;
                string fullName = null!;

                if (string.Equals(request.Dto.Provider, "Google", StringComparison.OrdinalIgnoreCase))
                {
                    var payload = await GoogleJsonWebSignature.ValidateAsync(request.Dto.Token);
                    email = payload.Email;
                    username = payload.Email.Split('@')[0];
                    fullName = payload.Name;
                }
                else if (string.Equals(request.Dto.Provider, "GitHub", StringComparison.OrdinalIgnoreCase))
                {
                    using var client = new HttpClient();

                    var tokenRequest = new Dictionary<string, string>
                    {
                        ["client_id"] = Environment.GetEnvironmentVariable("GITHUB_CLIENT_ID")!,
                        ["client_secret"] = Environment.GetEnvironmentVariable("GITHUB_CLIENT_SECRET")!,
                        ["code"] = request.Dto.Token
                    };

                    var tokenResponse = await client.PostAsync(
                        "https://github.com/login/oauth/access_token",
                        new FormUrlEncodedContent(tokenRequest)
                    );

                    tokenResponse.EnsureSuccessStatusCode();

                    var tokenContent = await tokenResponse.Content.ReadAsStringAsync();
                    var queryParams = System.Web.HttpUtility.ParseQueryString(tokenContent);
                    var accessToken = queryParams["access_token"];

                    if (string.IsNullOrEmpty(accessToken))
                        return Result.Fail<AuthResultDto>("Falha ao obter access token do GitHub.");

                    client.DefaultRequestHeaders.Add("User-Agent", "AuthenticationSystem");
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                    var response = await client.GetAsync("https://api.github.com/user");
                    response.EnsureSuccessStatusCode();

                    var json = await response.Content.ReadAsStringAsync();
                    var userData = JsonSerializer.Deserialize<GitHubUser>(json)!;

                    username = userData.Login;
                    fullName = userData.Name;

                    var responseEmails = await client.GetAsync("https://api.github.com/user/emails");
                    responseEmails.EnsureSuccessStatusCode();

                    var jsonEmails = await responseEmails.Content.ReadAsStringAsync();
                    var emails = JsonSerializer.Deserialize<List<GitHubEmail>>(jsonEmails);

                    email = emails?.FirstOrDefault(e => e.Primary && e.Verified)?.Email
                            ?? emails?.FirstOrDefault()?.Email
                            ?? $"{userData.Login}@github.local";
                }


                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    user = new User
                    {
                        Email = email,
                        UserName = username,
                        FullName = fullName,
                        IsActive = true
                    };

                    var result = await _userManager.CreateAsync(user);
                    if (!result.Succeeded)
                        return Result.Fail<AuthResultDto>(MessageError.SocialRegistrationFailed)
                            .WithErrors(result.Errors.Select(e => e.Description));
                }

                var tokenJwt = _jwtGenerator.GenerateToken(user);
                var dto = _mapper.Map<AuthResultDto>(user) with { Token = tokenJwt };

                return Result.Ok(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, MessageError.SocialLoginFailedUser, request.Dto.Provider);
                return Result.Fail<AuthResultDto>(MessageError.SocialLoginFailed);
            }
        }

        private record GitHubUser
        {
            [JsonPropertyName("name")]
            public string Name { get; set; } = string.Empty;

            [JsonPropertyName("login")]
            public string Login { get; set; } = string.Empty;

            [JsonPropertyName("email")]
            public string Email { get; set; } = string.Empty;

        };

        private record GitHubEmail
        {
            [JsonPropertyName("email")]
            public string Email { get; set; } = string.Empty;

            [JsonPropertyName("primary")]
            public bool Primary { get; set; }

            [JsonPropertyName("verified")]
            public bool Verified { get; set; }

            [JsonPropertyName("visibility")]
            public string? Visibility { get; set; }
        }
    }

}
