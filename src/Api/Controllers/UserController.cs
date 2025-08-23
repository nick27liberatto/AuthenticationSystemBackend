using Application.Commands;
using Application.Dtos.Request;
using Application.Dtos.Response;
using Application.Queries;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/v1/users")]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public UserController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<UserResponseDto>>> Search([FromQuery] SearchUsersQuery dto)
    {
        var query = _mapper.Map<SearchUsersQuery>(dto);
        var response = await _mediator.Send(query);
        return Ok(response.Result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserResponseDto>> GetById([FromRoute] int id)
    {
        var response = await _mediator.Send(new GetUserByIdQuery { Id = id });
        return response;
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginUserResponseDto>> Login([FromBody] LoginUserRequestDto dto)
    {
        var command = _mapper.Map<LoginUserCommand>(dto);
        var response = await _mediator.Send(command);
        return response;
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserResponseDto>> Register([FromBody] RegisterUserRequestDto dto)
    {
        var command = _mapper.Map<RegisterUserCommand>(dto);
        var response = await _mediator.Send(command);
        return response;
    }

    [HttpPatch("{id}/password/update")]
    public async Task<IActionResult> UpdatePassword([FromRoute] int id, [FromBody] UserResponseDto dto)
    {
        var command = _mapper.Map<UpdatePasswordCommand>(dto);
        command.Id = id;
        var response = await _mediator.Send(command);
        return response is null ? NotFound() : Ok(response);

    }

    [HttpPatch("{id}/password/recover")]
    public async Task<IActionResult> RecoverPassword([FromRoute] int id, [FromBody] UserResponseDto dto)
    {
        var command = _mapper.Map<RecoverPasswordCommand>(dto);
        command.Id = id;
        var response = await _mediator.Send(command);
        return response is null ? NotFound() : Ok(response);

    }

    [HttpDelete("{id}/delete")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var response = await _mediator.Send(new DeleteUserCommand { Id = id });
        return response is null ? NotFound() : Ok(response);

    }
}
