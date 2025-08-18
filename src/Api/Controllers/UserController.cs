using Application.Commands;
using Application.Dtos.Response;
using Application.Queries;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/v1/users")]
public class ElementController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public ElementController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] UserDto dto)
    {
        var command = _mapper.Map<SearchUsersQuery>(dto);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] int id)
    {
        var result = await _mediator.Send(new GetUserByIdQuery { Id = id });
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] UserDto dto)
    {
        var command = _mapper.Map<RegisterUserCommand>(dto);
        var result = await _mediator.Send(command);
        return CreatedAtAction(
                nameof(Get),
                new { id = result.Id },
                result
            );
    }

    [HttpPatch("{id}/password/update")]
    public async Task<IActionResult> UpdatePassword([FromRoute] int id, [FromBody] UserDto dto)
    {
        var command = _mapper.Map<UpdatePasswordCommand>(dto);
        command.Id = id;
        var result = await _mediator.Send(command);
        return result is null ? NotFound() : Ok(result);

    }

    [HttpPatch("{id}/password/recover")]
    public async Task<IActionResult> RecoverPassword([FromRoute] int id, [FromBody] UserDto dto)
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
