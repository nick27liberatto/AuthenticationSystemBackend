namespace Application.Mapper
{
    using Application.Dtos.Response;
    using AutoMapper;
    using Domain.Models;

    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>()
                .ReverseMap();
        }
    }
}
