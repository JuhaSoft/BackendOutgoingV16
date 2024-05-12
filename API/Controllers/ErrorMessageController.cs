using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Model;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Application.ErrorMessage;
using static Application.ErrorMessage.List;

namespace API.Controllers
{
    public class ErrorMessageController :BaseApiController

    {
        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ErrorMessageController(IMediator mediator, IHttpContextAccessor httpContextAccessor)
        {
            this._mediator = mediator;
            this._httpContextAccessor = httpContextAccessor;
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<ListResult<Domain.Model.ErrorMessage>>> GetDataError(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string searchQuery = "",
            [FromQuery] string category = "All"
)
        {
            var query = new Application.ErrorMessage.List.Query
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                SearchQuery = searchQuery,
                Category = category
            };
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<Domain.Model.ErrorMessage>> GetErrorMessage(Guid id)
        {
            try
            {
                var errormessage = await _mediator.Send(new Application.ErrorMessage.Details.Query { Id = id });

                if (errormessage == null)
                {
                    return NotFound();
                }

                return errormessage;
            }
            catch (Exception ex)
            {
                // Tangkap kesalahan dan kirim respons error ke client
                return StatusCode(StatusCodes.Status500InternalServerError, "Error Mengambil Pesan Error: " + ex.Message);
            }
        }
        [AllowAnonymous]

        [HttpPost]
        public async Task<IActionResult> CreateErrorMessage(ErrorMessage errorMessage)
        {
            try
            {
                return Ok(await _mediator.Send(new  Create.Command { ErrorMessage = errorMessage }));
            }
            catch (Exception ex)
            {
                // Tangkap kesalahan dan kirim respons error ke client
                return StatusCode(StatusCodes.Status500InternalServerError,  ex.Message);
            }
        }
        [AllowAnonymous]
        [HttpPut("{Id}")]
        public async Task<IActionResult> EditError(Guid id, ErrorMessage errorMessage)
        {
            try
            {
                errorMessage.Id = id;
                // Mengirim permintaan ke mediator untuk membuat user
                return Ok(await _mediator.Send(new Application.ErrorMessage.Edit.Command { Errormessage = errorMessage }));
            }
            catch (Exception ex)
            {
                // Tangkap kesalahan dan kirim respons error ke client
                return StatusCode(StatusCodes.Status500InternalServerError, "Terjadi kesalahan dalam edit error: " + ex.Message);
            }
        }
        [AllowAnonymous]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteError(Guid id)
        {
            try
            {
                // Mengirim permintaan ke mediator untuk menghapus ErrorMessage
                await _mediator.Send(new Application.ErrorMessage.Delete.Command { Id = id });
                return Ok();
            }
            catch (Exception ex)
            {
                // Tangkap kesalahan dan kirim respons error ke client
                return StatusCode(StatusCodes.Status500InternalServerError, "Terjadi kesalahan dalam delete ErrorMessage: " + ex.Message);
            }
        }
    }
}