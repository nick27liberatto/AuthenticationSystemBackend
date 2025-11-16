namespace Application.Handlers
{
    using Application.Interfaces;
    using Application.Queries;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;

    public class ExternalCallbackHandler : IRequestHandler<ExternalCallbackQuery, IActionResult>
    {
        private readonly IExternalAuthService _externalAuth;

        public ExternalCallbackHandler(IExternalAuthService externalAuth)
        {
            _externalAuth = externalAuth;
        }

        public async Task<IActionResult> Handle(ExternalCallbackQuery request, CancellationToken cancellationToken)
        {
            var userInfo = await _externalAuth.ExchangeCodeForUserInfoAsync(request.Provider, request.Code, request.State, request.HttpContext);
            var jwt = await _externalAuth.HandleExternalLoginAsync(userInfo);
            var redirectTo = $"https://app.meusite.com/login/callback?token={Uri.EscapeDataString(jwt)}";
            return new RedirectResult(redirectTo);
        }
    }
}
