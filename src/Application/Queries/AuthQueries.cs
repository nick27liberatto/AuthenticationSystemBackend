namespace Application.Queries
{
    using MediatR;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    public record ExternalLoginQuery(string Provider, HttpContext HttpContext): IRequest<IActionResult>;
    public record ExternalCallbackQuery(string Code, string State, string Provider, HttpContext HttpContext): IRequest<IActionResult>;
}
