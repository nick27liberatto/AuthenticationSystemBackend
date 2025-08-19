namespace Application.Mapper
{
    using Application.Dtos.Request;
    using Application.Dtos.Response;
    using AutoMapper;
    using Domain.Models;

    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserResponseDto>()
                .ReverseMap();

            CreateMap<User, LoginUserRequestDto>()
                .ForMember(d => d.Password, opts => opts
                .Ignore())
                .ReverseMap();
        }
    }
}
