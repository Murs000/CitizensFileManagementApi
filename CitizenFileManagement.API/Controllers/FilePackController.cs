using CitizenFileManagement.Core.Application.Features.Commands.Document.Create;
using CitizenFileManagement.Core.Application.Features.Commands.Document.Delete;
using CitizenFileManagement.Core.Application.Features.Commands.Document.Update;
using CitizenFileManagement.Core.Application.Features.Queries.Document.Get;
using CitizenFileManagement.Core.Application.Features.Queries.Document.GetAll;
using CitizenFileManagement.Core.Application.Features.Queries.Document.GetMultiple;
using CitizenFileManagement.Core.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CitizenFileManagement.API.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    [Authorize(Roles = "Personal, Admin")]
    public class FilePackController : ControllerBase
    {
        private readonly IMediator _mediator;

        public FilePackController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
    }
}