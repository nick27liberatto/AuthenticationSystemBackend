namespace Api.Controllers
{
    using Application.Commands;
    using Application.DTOs;
    using Application.Extensions;
    using AutoMapper;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/v1/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public AuthController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpPost("social-login")]
        public async Task<IActionResult> SocialLogin([FromBody] SocialLoginDto dto)
        {
            var response = await _mediator.Send(new SocialLoginCommand(dto));
            return response.ToActionResult();
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto dto)
        {
            var response = await _mediator.Send(_mapper.Map<RegisterUserCommand>(dto));
            return response.ToActionResult("Register");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto dto)
        {
            var response = await _mediator.Send(_mapper.Map<LoginUserCommand>(dto));
            return response.ToActionResult("Login");
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
        {
            var response = await _mediator.Send(_mapper.Map<ForgotPasswordCommand>(dto));
            return response.ToActionResult();
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            var response = await _mediator.Send(_mapper.Map<ResetPasswordCommand>(dto));
            return response.ToActionResult();
        }
    }
}
