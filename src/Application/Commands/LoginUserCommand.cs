namespace Application.Queries
{
    using Application.Dtos.Response;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;

    public class LoginUserCommand : IRequest<ActionResult<LoginUserResponseDto>>
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
