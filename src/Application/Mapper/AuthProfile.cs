namespace Application.Mapper
{
    using Application.Commands;
    using Application.DTOs;
    using AutoMapper;
    using Domain.Models;

    public class AuthProfile : Profile
    {
        public AuthProfile()
        {

            CreateMap<SocialLoginDto, SocialLoginCommand>()
                .ConstructUsing(dto => new SocialLoginCommand(dto));

            CreateMap<RegisterUserDto, RegisterUserCommand>()
            .ConstructUsing(dto => new RegisterUserCommand(dto));

            CreateMap<LoginUserDto, LoginUserCommand>()
                .ConstructUsing(dto => new LoginUserCommand(dto));

            CreateMap<ForgotPasswordDto, ForgotPasswordCommand>()
                .ConstructUsing(dto => new ForgotPasswordCommand(dto));

            CreateMap<ResetPasswordDto, ResetPasswordCommand>()
                .ConstructUsing(dto => new ResetPasswordCommand(dto));

            CreateMap<User, AuthResultDto>();
            CreateMap<RegisterUserDto, User>();

        }
    }
}
