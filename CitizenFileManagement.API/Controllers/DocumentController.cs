using CitizenFileManagement.Core.Application.Features.Commands.Document.Create;
using CitizenFileManagement.Core.Application.Features.Commands.Document.Delete;
using CitizenFileManagement.Core.Application.Features.Commands.Document.Update;
using CitizenFileManagement.Core.Application.Features.Queries.Document.Get;
using CitizenFileManagement.Core.Application.Features.Queries.Document.GetAll;
using CitizenFileManagement.Core.Application.Features.Queries.Document.GetMultiple;
using CitizenFileManagement.Core.Application.Features.Queries.Models;
using CitizenFileManagement.Core.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CitizenFileManagement.API.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    [Authorize(Roles = "Personal, Admin")]
    public class DocumentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DocumentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll([FromQuery] PaginationModel? paginationModel, [FromQuery] FilterModel? filterModel)
        {
            return Ok(await _mediator.Send(new GetAllDocumentQuery{ FilterModel = filterModel, PaginationModel = paginationModel}));
        }

        [HttpGet("get-by-id")]
        public async Task<IActionResult> GetDocument([FromQuery] int documentId)
        {
            var query = new GetDocumentQuery { DocumentId = documentId };
            var returnDocumentViewModel = await _mediator.Send(query);

            return File(returnDocumentViewModel.Bytes, returnDocumentViewModel.Type, returnDocumentViewModel.Name);
        }

        [HttpGet("get-multiple")]
        public async Task<IActionResult> GetMultipleDocument([FromQuery] List<int> documentIds)
        {
            var query = new GetMultipleDocumentQuery { DocumentIds = documentIds };

            var returnDocumentViewModel = await _mediator.Send(query);

            return File(returnDocumentViewModel.Bytes, returnDocumentViewModel.Type, returnDocumentViewModel.Name);
        }

        [HttpGet("get-by-type")]
        public async Task<IActionResult> GetMultipleDocument([FromQuery] DocumentType type)
        {
            var query = new GetByTypeDocumentQuery { Type = type };

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