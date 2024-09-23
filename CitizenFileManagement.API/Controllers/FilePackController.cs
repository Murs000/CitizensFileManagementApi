using CitizenFileManagement.Core.Application.Features.Commands.FilePack.Create;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CitizenFileManagement.API.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    [Authorize(Roles = "Enterprice, Admin")]
    public class FilePackController : ControllerBase
    {
        private readonly IMediator _mediator;

        public FilePackController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("create")]
        public async Task<IActionResult> GetAll(CreateFilePackCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
    }
}