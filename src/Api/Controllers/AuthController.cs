namespace Api.Controllers
{
    using Application.Commands;
    using Application.DTOs;
    using Application.Extensions;
    using Application.Queries;
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

        [HttpGet("external/{provider}")]
        public async Task<IActionResult> ExternalLogin([FromRoute] string provider)
        {
            var response = await _mediator.Send(new ExternalLoginQuery(provider, HttpContext));
            return response;
        }

        [HttpGet("external/callback")]
        public async Task<IActionResult> ExternalCallback([FromQuery] string code, [FromQuery] string state, [FromQuery] string provider)
        {
            var response = await _mediator.Send(new ExternalCallbackQuery(code, state, provider, HttpContext));
            return response;
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
