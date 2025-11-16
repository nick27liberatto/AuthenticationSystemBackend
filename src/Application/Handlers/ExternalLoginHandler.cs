namespace Application.Handlers
{
    using Application.Interfaces;
    using Application.Queries;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;

    public class ExternalLoginHandler : IRequestHandler<ExternalLoginQuery, IActionResult>
    {
        private readonly IExternalAuthService _externalAuth;

        public ExternalLoginHandler(IExternalAuthService externalAuth)
        {
            _externalAuth = externalAuth;
        }

        public async Task<IActionResult> Handle(ExternalLoginQuery request, CancellationToken cancellationToken)
        {
            var redirectUrl = await _externalAuth.GetAuthorizationUrlAsync(request.Provider, request.HttpContext);
            return new RedirectResult(redirectUrl);
        }
    }
}
