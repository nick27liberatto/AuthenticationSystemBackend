namespace Application.Queries
{
    using Application.Dtos.Response;
    using MediatR;

    public class GetUserByIdQuery : IRequest<UserResponseDto>
    {
        public int Id { get; set; }
    }
}   
