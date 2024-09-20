using CitizenFileManagement.Core.Application.Features.Commands.Document.Create;
using CitizenFileManagement.Core.Application.Features.Commands.Document.Delete;
using CitizenFileManagement.Core.Application.Features.Commands.Document.Update;
using CitizenFileManagement.Core.Application.Features.Queries.Document.Get;
using CitizenFileManagement.Core.Application.Features.Queries.Document.GetAll;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CitizenFileManagement.API.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    [Authorize]
    public class DocumentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DocumentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("get-all")]
        public async Task<IActionResult> GetAll(GetAllDocumentQuery command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpGet("get/{documentId}")]
        public async Task<IActionResult> GetDocument(int documentId)
        {
            var query = new GetDocumentQuery { DocumentId = documentId };
            var returnDocumentViewModel = await _mediator.Send(query);

            return File(returnDocumentViewModel.Bytes, returnDocumentViewModel.Type, returnDocumentViewModel.Name);
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
        public async Task<IActionResult> Delete(DeleteDocumentCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
    }
}