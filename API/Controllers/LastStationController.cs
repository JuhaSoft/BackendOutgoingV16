using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.LastStationIDs;
using Common.DTOs.LastStationID;
using Domain.Model;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class LastStationController : BaseApiController
    {
        private readonly IMediator _mediator;
        public LastStationController(IMediator mediator)
        {
            this._mediator = mediator;
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<List<LastStationIDDTO>>> GetDataLine(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 999999,
            [FromQuery] string SearchQuery = "",
             [FromQuery] string Category = "All"
            )
        {
            var query = new List.Query
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                SearchQuery = SearchQuery,
                Category = Category
            };
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task <ActionResult<LastStationID>>GetLastStationID(Guid Id)
        {
             try
            {
               var laststationID =await Mediator.Send(new Application.LastStationIDs.Details.Query{Id = Id});
               if (laststationID == null)
            {
                return NotFound();
            }

            return laststationID;
            }
            catch (Exception ex)
            {
                // Tangkap kesalahan dan kirim respons error ke client
                return StatusCode(StatusCodes.Status500InternalServerError, "Error Mengambil data parameter: " + ex.Message);
            }


            
        }
        [AllowAnonymous]

        [HttpPost]
        public async Task<IActionResult> CreateLastID(LastStationID lastStationID)
        {
            try
            {
                return Ok(await _mediator.Send(new  Application.LastStationIDs.Create.Command { LastStationID = lastStationID }));
            }
            catch (Exception ex)
            {
                // Tangkap kesalahan dan kirim respons error ke client
                return StatusCode(StatusCodes.Status500InternalServerError, "Terjadi kesalahan dalam membuat Parameter: " + ex.Message);
            }
        }
        [AllowAnonymous]
        [HttpPut("{Id}")]
        public async Task<IActionResult> EditLastStation(Guid id,LastStationID lastStationID)
        {
            try
            {
                lastStationID.Id=id;
                // Mengirim permintaan ke mediator untuk membuat user
                return Ok(await _mediator.Send(new  Application.LastStationIDs.Edit.Command { LastStationID = lastStationID }));
            }
            catch (Exception ex)
            {
                // Tangkap kesalahan dan kirim respons error ke client
                return StatusCode(StatusCodes.Status500InternalServerError, "Terjadi kesalahan dalam edit user: " + ex.Message);
            }
        }
        [AllowAnonymous]
        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteLastStation(Guid Id)
        {
            try
            {
              
                // Mengirim permintaan ke mediator untuk membuat user
                return Ok(await _mediator.Send(new  Application.LastStationIDs.Delete.Command { Id = Id }));
            }
            catch (Exception ex)
            {
                // Tangkap kesalahan dan kirim respons error ke client
                return StatusCode(StatusCodes.Status500InternalServerError, "Terjadi kesalahan dalam delete Last Station: " + ex.Message);
            }
        } 
    }
}