using CitizenFileManagement.Core.Application.Features.Commands.User.Login;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CitizenFileManagement.API.Controllers;

[Route("api/user")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(CreateAuthorizationTokenCommand command)
    {
        return Ok(await _mediator.Send(command));
    }
}