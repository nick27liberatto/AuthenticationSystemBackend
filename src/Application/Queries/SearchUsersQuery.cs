namespace Application.Queries
{
    using Application.Dtos.Response;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;

    public class SearchUsersQuery : IRequest<ActionResult<IEnumerable<UserResponseDto>>>
    {
        public string? Search { get; set; }
    }
}
