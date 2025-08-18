namespace Application.Queries
{
    using Application.Dtos.Response;
    using MediatR;

    public class GetUserByIdQuery : IRequest<UserDto>
    {
        public int Id { get; set; }
    }
}   
