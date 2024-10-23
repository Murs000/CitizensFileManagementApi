using CitizenFileManagement.Core.Application.Features.Commands.FilePack.AddDocument;
using CitizenFileManagement.Core.Application.Features.Commands.FilePack.Create;
using CitizenFileManagement.Core.Application.Features.Commands.FilePack.Delete;
using CitizenFileManagement.Core.Application.Features.Commands.FilePack.RemoveDocument;
using CitizenFileManagement.Core.Application.Features.Commands.FilePack.Update;
using CitizenFileManagement.Core.Application.Features.Queries.FilePack.Get;
using CitizenFileManagement.Core.Application.Features.Queries.FilePack.GetAll;
using CitizenFileManagement.Core.Application.Features.Queries.Models;
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
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll([FromQuery] PaginationModel? paginationModel, [FromQuery] FilterModel? filterModel)
        {
            return Ok(await _mediator.Send(new GetAllFilePackQuery{ FilterModel = filterModel, PaginationModel = paginationModel}));
        }
        [HttpGet("get-pack")]
        public async Task<IActionResult> GetPack([FromQuery]int fileId)
        {
            var query = new GetFilePackQuery{ FilePackId = fileId};

            var returnDocumentViewModel = await _mediator.Send(query);

            return File(returnDocumentViewModel.Bytes, returnDocumentViewModel.Type, returnDocumentViewModel.Name);
        }
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm]CreateFilePackCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromForm]UpdateFilePackCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
        [HttpDelete("delete")]
        public async Task<IActionResult> Delete(DeleteFilePackCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
        [HttpDelete("add-document")]
        public async Task<IActionResult> AddDocument(AddDocumentFilePackCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
        [HttpDelete("remove-document")]
        public async Task<IActionResult> RemoveDocument(RemoveDocumentFilePackCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
        
    }
}