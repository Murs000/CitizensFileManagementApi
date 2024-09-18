
using CitizenFileManagement.Core.Application.Features.Commands.Document.Create;
using CitizenFileManagement.Core.Application.Features.Commands.Document.Update;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CitizenFileManagement.API.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DocumentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm] CreateDocumentCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromForm] UpdateDocumentCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete(UpdateDocumentCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
    }
}