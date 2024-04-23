using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DataLines;
using Common.DTOs;
using Domain.Model;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class DataLineController : BaseApiController

    {
        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DataLineController(IMediator mediator, IHttpContextAccessor httpContextAccessor)
        {
            this._mediator = mediator;
            this._httpContextAccessor = httpContextAccessor;
        }
         [AllowAnonymous]
                 [HttpGet]
              public async Task<ActionResult<List<DataLineDTO>>> GetDataLine(
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
        // Endpoint u
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task <ActionResult<DataLine>>GetLine(Guid Id)
        {
             try
            {
               var dataLine =await Mediator.Send(new Application.DataLines.Detail.Query{Id = Id});
               if (dataLine == null)
            {
                return NotFound();
            }

            return dataLine;
            }
            catch (Exception ex)
            {
                // Tangkap kesalahan dan kirim respons error ke client
                return StatusCode(StatusCodes.Status500InternalServerError, "Error Mengambil data parameter: " + ex.Message);
            }


            
        }
        [AllowAnonymous]

        [HttpPost]
        public async Task<IActionResult> CreateLine(DataLine dataLine)
        {
            try
            {
                return Ok(await _mediator.Send(new  Application.DataLines.Create.Command { DataLine = dataLine }));
            }
            catch (Exception ex)
            {
                // Tangkap kesalahan dan kirim respons error ke client
                return StatusCode(StatusCodes.Status500InternalServerError, "Terjadi kesalahan dalam membuat Parameter: " + ex.Message);
            }
        }
        [AllowAnonymous]

        [HttpPut("{Id}")]
        public async Task<IActionResult> EditLine(Guid id,DataLine dataLine)
        {
            try
            {
                dataLine.Id=id;
                // Mengirim permintaan ke mediator untuk membuat user
                return Ok(await _mediator.Send(new  Application.DataLines.Edit.Command { dataLine = dataLine }));
            }
            catch (Exception ex)
            {
                // Tangkap kesalahan dan kirim respons error ke client
                return StatusCode(StatusCodes.Status500InternalServerError, "Terjadi kesalahan dalam edit user: " + ex.Message);
            }
        }
         [AllowAnonymous]
           [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteLine(Guid Id)
        {
            try
            {
              
                // Mengirim permintaan ke mediator untuk membuat user
                return Ok(await _mediator.Send(new  Delete.Command { Id = Id }));
            }
            catch (Exception ex)
            {
                // Tangkap kesalahan dan kirim respons error ke client
                return StatusCode(StatusCodes.Status500InternalServerError, "Terjadi kesalahan dalam delete Parameter: " + ex.Message);
            }
        }
        
    }
}