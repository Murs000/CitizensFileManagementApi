using CitizenFileManagement.Core.Application.Features.Commands.FilePack.Create;
using CitizenFileManagement.Core.Application.Features.Commands.FilePack.Delete;
using CitizenFileManagement.Core.Application.Features.Commands.FilePack.Update;
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
        public async Task<IActionResult> Create(CreateFilePackCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
        [HttpPut("update")]
        public async Task<IActionResult> Update(UpdateFilePackCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
        [HttpDelete("delete")]
        public async Task<IActionResult> Delete(List<int> ids)
        {
            return Ok(await _mediator.Send(new DeleteFilePackCommand{ FileIds = ids}));
        }
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _mediator.Send(new DeleteFilePackCommand()));
        }
        [HttpGet("get-pack")]
        public async Task<IActionResult> GetPack()
        {
            return Ok(await _mediator.Send(new DeleteFilePackCommand()));
        }

    }
}