namespace Application.Mapper
{
    using Application.Commands;
    using Application.Dtos.Request;
    using Application.Dtos.Response;
    using Application.Queries;
    using AutoMapper;
    using Domain.Models;

    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserResponseDto>()
                .ReverseMap();

            CreateMap<User, RegisterUserRequestDto>()
                .ReverseMap();

            CreateMap<List<User>, UserResponseDto>();

            CreateMap<User, LoginUserRequestDto>()
                .ForMember(d => d.Password, opts => opts
                .Ignore())
                .ReverseMap();

            CreateMap<RegisterUserRequestDto, RegisterUserCommand>();
            CreateMap<RegisterUserCommand, User>();

            CreateMap<LoginUserRequestDto, LoginUserCommand>();
            CreateMap<LoginUserCommand, User>();
            CreateMap<User, LoginUserResponseDto>();
        }
    }
}
