using Application.Client.Commands.CreateClient;
using Application.Client.Commands.CreateClientImport;
using Application.Client.Commands.UpdateClient;
using Application.Client.Queries.AllClientsQuery;
using Application.Client.Queries.ClientByIdQuery;
using Application.Client.Queries.ClientDashboardQuery;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ClientController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ClientController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] CreateClientCommandRequest request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpPost("import")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        public async Task<IActionResult> Import([FromForm] IFormFile file)
        {
            await using var content = file?.OpenReadStream();
            var importId = await _mediator.Send(new CreateClientImportCommandRequest
            {
                FileName = file?.FileName,
                FileContent = content
            });

            return Accepted(new { importId, status = "Pending" });
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<AllClientsQueryResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ListAll([FromQuery] AllClientsQueryRequest request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpGet("dashboard")]
        [ProducesResponseType(typeof(ClientDashboardQueryResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDashboard()
        {
            var response = await _mediator.Send(new ClientDashboardQueryRequest());
            return Ok(response);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ClientByIdQueryResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var response = await _mediator.Send(new ClientByIdQueryRequest { Id = id });

            return Ok(response);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateClientCommandRequest request)
        {
            if (id != request.Id)
                return BadRequest();

            await _mediator.Send(request);
            return NoContent();
        }
    }
}
