namespace Application.Queries
{
    using Application.Dtos.Response;
    using MediatR;

    public class SearchUsersQuery : IRequest<UserDto>
    {
        public string? Search { get; set; }
    }
}
