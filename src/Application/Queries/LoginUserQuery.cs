namespace Application.Queries
{
    using Application.Dtos.Response;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;

    public class LoginUserQuery : IRequest<ActionResult<UserResponseDto>>
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
