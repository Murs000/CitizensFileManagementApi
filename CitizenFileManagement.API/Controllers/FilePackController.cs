using CitizenFileManagement.Core.Application.Features.Commands.FilePack.Create;
using CitizenFileManagement.Core.Application.Features.Commands.FilePack.Delete;
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
        public async Task<IActionResult> GetAll([FromQuery] PaginationModel? paginationModel, [FromQuery] FilterModel? filterModel, string? searchTerm)
        {
            return Ok(await _mediator.Send(new GetAllFilePackQuery{ FilterModel = filterModel, PaginationModel = paginationModel, SearchTerm = searchTerm}));
        }
        [HttpGet("get-pack")]
        public async Task<IActionResult> GetPack([FromQuery]int fileId)
        {
            var query = new GetFilePackQuery{ FilePackId = fileId};

            var returnDocumentViewModel = await _mediator.Send(query);

            return File(returnDocumentViewModel.Bytes, returnDocumentViewModel.Type, returnDocumentViewModel.Name);
        }
    }
}