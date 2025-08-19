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

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserResponseDto>>> Search([FromQuery] UserResponseDto dto)
    {
        var query = _mapper.Map<SearchUsersQuery>(dto);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] int id)
    {
        var result = await _mediator.Send(new GetUserByIdQuery { Id = id });
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("/login")]
    public async Task<ActionResult<UserResponseDto>> Login([FromRoute] LoginUserRequestDto dto)
    {
        var query = _mapper.Map<LoginUserQuery>(dto);
        var response = await _mediator.Send(query);
        return response;
    }

    [HttpPost]
    public async Task<ActionResult<UserResponseDto>> Create([FromBody] UserResponseDto dto)
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
        var result = await _mediator.Send(command);
        return result is null ? NotFound() : Ok(result);

    }

    [HttpPatch("{id}/password/recover")]
    public async Task<IActionResult> RecoverPassword([FromRoute] int id, [FromBody] UserResponseDto dto)
    {
        var command = _mapper.Map<RecoverPasswordCommand>(dto);
        command.Id = id;
        var result = await _mediator.Send(command);
        return result is null ? NotFound() : Ok(result);

    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var result = await _mediator.Send(new DeleteUserCommand { Id = id });
        return result is null ? NotFound() : Ok(result);

    }
}
